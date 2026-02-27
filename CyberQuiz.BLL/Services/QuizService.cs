using CyberQuiz.BLL.Interfaces;
using CyberQuiz.DAL.Entities;
using CyberQuiz.DAL.Repositories.Interfaces;
using CyberQuiz.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace CyberQuiz.BLL.Services
{
    //Implementation av quiz logik (progress, låsning, rätt/fel)
    public class QuizService : IQuizService
    {
        private const double CompletionThreshold = 80.0; //80% regel

        private readonly IUnitOfWork _uow; //DAL-access

        public QuizService(IUnitOfWork uow) //DI
        {
            _uow = uow;
        }

        public async Task<List<CategoryDto>> GetCategoriesAsync(string userId)
        {
            //Måste ha en användare
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException("userId is required.");

            //Hämtar data från DAL 
            var categories = await _uow.Categories.GetAllAsync();
            var subCategories = await _uow.SubCategories.GetAllAsync();

            var result = new List<CategoryDto>();


            foreach (var c in categories)
            {
                //Filtrerar SubCategories som tillhör den aktuella kategorin
                var subsInCategory = subCategories.Where(sc => sc.CategoryId == c.Id).ToList();

                //Räknar hur många subkategorier som är klara för användaren
                var completed = 0;
                foreach (var sub in subsInCategory)
                {
                    var progress = await GetSubCategoryProgressAsync(userId, sub.Id); //progress-beräkning
                    if (progress.IsCompleted)
                        completed++;
                }

                //Returnerar DTO till UI/API
                result.Add(new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    TotalSubCategories = subsInCategory.Count,
                    CompletedSubCategories = completed
                });
            }

            return result;
        }

        public async Task<List<SubCategoryDto>> GetSubCategoriesAsync (int categoryId, string userId)
        {
            //Måste ha användare (id)
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException("userId is required.");

            //Hämtar subkategorier + frågor
            var subCategories = await _uow.SubCategories.GetAllAsync();
            var questions = await _uow.Questions.GetAllAsync();

            var subsInCategory = subCategories
                .Where(sc => sc.CategoryId == categoryId) //filtrerar subkategorier i denna kategori
                .OrderBy(sc => sc.Id) //sorterar på Id 
                .ToList();

            //SubCategory1 -> SubProgress -> ScorePercent=80, IsCompleted=true
            var progressBySubId = new Dictionary<int, SubProgress>(); 
            var dtos = new List<SubCategoryDto>();



            foreach (var sub in subsInCategory)
            {
                //Antal frågor i subkategorin
                var questionCount = questions.Count(q => q.SubCategoryId == sub.Id);

                
                //Beräknar användarens progress för subkategorin
                var progress = await GetSubCategoryProgressAsync(userId, sub.Id);

                
                progress.QuestionCount = questionCount;
                progressBySubId[sub.Id] = progress;


                //Lägger till DTO med IsLocked=true som default (kommer justeras i ApplyLockStates)
                dtos.Add(new SubCategoryDto
                {
                    Id = sub.Id,
                    Name = sub.Name,
                    QuestionCount = questionCount,
                    IsLocked = true //sätts korrekt i ApplyLockStates
                });
            }

            //Sätter IsLocked baserat på föregående subkategori (intern metod längre ner)
            ApplyLockStates(dtos, progressBySubId);

            return dtos;
        }


        public async Task<List<QuestionDto>> GetQuestionsAsync(int subCategoryId, string userId)
        {
            //Måste ha användare
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException("userId is required.");

            //Hämtar subkategorier för att kunna kontrollera låsning
            var subCategories = await _uow.SubCategories.GetAllAsync();
            var currentSub = subCategories.SingleOrDefault(s => s.Id == subCategoryId);

            //Om subkategori inte finns -> tom lista
            if (currentSub == null)
                return new List<QuestionDto>();


            //Hämtar alla subs i samma kategori för att räkna låsning
            var subsInCategory = subCategories
                .Where(s => s.CategoryId == currentSub.CategoryId)
                .OrderBy(s => s.Id)
                .ToList();

            //Kollar om subkategorin är låst för användaren
            var lockedSet = await ComputeLockedSubCategoriesAsync(userId, subsInCategory); //intern metod längre ner
            if (lockedSet.Contains(subCategoryId))
                return new List<QuestionDto>();

            
            //Hämtar frågor och filtrerar på subkategori
            var questions = await _uow.Questions.GetAllAsync();

            //Returnerar frågor som DTOs
            return questions
                .Where(q => q.SubCategoryId == subCategoryId)
                .Select(q => new QuestionDto
                {
                    Id = q.Id,
                    Text = q.Text
                })
                .ToList();
        }


        public async Task<SubmitAnswerResponseDto> SubmitAnswerAsync(string userId, SubmitAnswerRequestDto request)
        {
            //Måste ha användare
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException("userId is required.");

            //Måste ha request (SubmitAnswerRequestDto)
            if (request == null)
                throw new ArgumentNullException(nameof(request));


            //Hämtar frågan från DAL
            var question = await _uow.Questions.GetByIdAsync(request.QuestionId);
            if (question == null)
                throw new InvalidOperationException("Question not found."); //Om frågan inte finns -> exception


            //Hämtar valt svarsalternativ och kontrollerar att det tillhör frågan
            var selected = await _uow.AnswerOptions.GetByIdAsync(request.AnswerOptionId);
            if (selected == null)
                throw new InvalidOperationException("AnswerOption not found.");

            if (selected.QuestionId != question.Id)
                throw new InvalidOperationException("AnswerOption belongs to another Question.");

            //Hämtar alla AnswerOptions för question för att kunna returnera rätt svarsalternativ i response
            var allOptions = await _uow.AnswerOptions.GetAllAsync();
            var optionsForQuestion = allOptions.Where(o => o.QuestionId == question.Id).ToList();


            //Kontrollerar om det valda alternativet är korrekt
            var correct = optionsForQuestion.FirstOrDefault(o => o.IsCorrect);
            if (correct == null)
                throw new InvalidOperationException("Question has no correct option.");

            var isCorrect = selected.Id == correct.Id;

            //Skapar UserResult (sparar varje svar)
            var userResult = new UserResult
            {
                UserId = userId,
                QuestionId = question.Id,
                AnswerOptionId = selected.Id,
                IsCorrect = isCorrect
            };

            
            await _uow.UserResults.AddAsync(userResult);
            await _uow.SaveAsync(); //Sparar ändringar i DB

            //Beräknar ny progress för subkategorin efter sparat svar
            var progress = await GetSubCategoryProgressAsync(userId, question.SubCategoryId);

            //Returnerar DTO med resultat och progress-info
            return new SubmitAnswerResponseDto
            {
                QuestionId = question.Id,
                AnswerOptionId = selected.Id,
                IsCorrect = isCorrect,
                CorrectAnswerOptionId = correct.Id,
                SubCategoryScorePercent = progress.ScorePercent,
                SubCategoryCompleted = progress.IsCompleted
            };
        }




        //-------------------------------------------------------------------------------------------
        //--------------------------Interna hjälpmetoder---------------------------------------------
        //-------------------------------------------------------------------------------------------




        //Intern hjälpmetod för att beräkna vilka subkategorier som är låsta för användaren
        private async Task<HashSet<int>> ComputeLockedSubCategoriesAsync(string userId, List<SubCategory> orderedSubs)
        {
            //Returnerar vilka subCategoryId som är låsta för användaren
            var locked = new HashSet<int>();
            if(orderedSubs.Count == 0) return locked;

            //För varje SubCategory efter första, kolla om föregående är completed
            for (int i = 1; i < orderedSubs.Count; i++)
            {
                var prevSubId = orderedSubs[i - 1].Id;
                var prevProgress = await GetSubCategoryProgressAsync(userId, prevSubId);

                if(!prevProgress.IsCompleted)
                    locked.Add(orderedSubs[i].Id);
            }
            return locked;
        }


        //Hjälpmetod för att sätta IsLocked på subkategorier baserat på progress
        private static void ApplyLockStates(List<SubCategoryDto> subsOrdered, Dictionary<int, SubProgress> progressBySubId)
        {
            // Skydd om listan är tom
            if (subsOrdered == null || subsOrdered.Count == 0)
                return;

            // Första subkategorin är alltid upplåst
            subsOrdered[0].IsLocked = false;

            // Resten låses upp när föregående är completed (>= 80%)
            for (int i = 1; i < subsOrdered.Count; i++)
            {
                var prev = subsOrdered[i - 1];
                subsOrdered[i].IsLocked = !progressBySubId[prev.Id].IsCompleted;
            }
        }



        //Hjälpmetod för att beräkna användarens progress i en subkategori
        private async Task<SubProgress> GetSubCategoryProgressAsync(string userId, int subCategoryId)
        {
            //Hämtar Questions för att få total antal frågor i subkategorin
            var questions = await _uow.Questions.GetAllAsync();
            var questionsInSub = questions.Where(q => q.SubCategoryId == subCategoryId).ToList();
            var totalQuestions = questionsInSub.Count;

            //Hämtar UserResults och filtrerar till denna subkategori
            var results = await _uow.UserResults.GetAllAsync();
            var questionIds = new HashSet<int>(questionsInSub.Select(q => q.Id));

            var userResultsInSub = results
                .Where(r => r.UserId == userId && questionIds.Contains(r.QuestionId))
                .ToList();


            //Tar "senaste" per fråga via högsta Id
            var latestByQuestion = userResultsInSub
                .GroupBy(r => r.QuestionId)
                .Select(g => g.OrderBy(x => x.Id).Last())
                .ToList();

            //Räknar antal rätt
            var correctCount = latestByQuestion.Count(r => r.IsCorrect);

            //Score% = correct / total * 100
            var score = CalculateScorePercent(totalQuestions, correctCount);

            //Returnerar progress-info till andra metoder
            return new SubProgress
            {
                QuestionCount = totalQuestions,
                ScorePercent = score,
                IsCompleted = score >= CompletionThreshold //>= 80%
            };
        }


        //Hjälpmetod som räknar score % och hanterar division med 0
        private static double CalculateScorePercent(int totalQuestions, int correctAnswers)
        {
            //Skydd mot division med 0
            if (totalQuestions <= 0) return 0.0;

            return Math.Round((double)correctAnswers / totalQuestions * 100.0, 2);
        }



        //Intern hjälpklass för att hålla progress
        private class SubProgress
        {
            public int QuestionCount { get; set; }
            public double ScorePercent { get; set; }
            public bool IsCompleted { get; set; }
        }
    }
}
