using CyberQuiz.BLL.Interfaces;
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

        public async Task <SubCategoryDto> GetSubCategoryAsync (int categoryId, string userId)
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

            //Sätter IsLocked baserat på föregående subkategori (intern metod längst ner)
            ApplyLockStates(dtos, progressBySubId);

            return dtos;
        }


        public async Task<List<QuestionDto>> GetQuestionsAsync(int subCategoryId, string userId)
        {
            // Validering
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException("userId is required.");

            // Hämtar subkategorier för att kunna kontrollera låsning
            var subCategories = await _uow.SubCategories.GetAllAsync();
            var currentSub = subCategories.SingleOrDefault(s => s.Id == subCategoryId);

            // Om subkategori inte finns -> tom lista
            if (currentSub == null)
                return new List<QuestionDto>();

            // Hämtar alla subs i samma kategori för att räkna låsning
            var subsInCategory = subCategories
                .Where(s => s.CategoryId == currentSub.CategoryId)
                .OrderBy(s => s.Id)
                .ToList();

            // Kollar om subkategorin är låst för användaren
            var lockedSet = await ComputeLockedSubCategoriesAsync(userId, subsInCategory); // intern metod längst ner
            if (lockedSet.Contains(subCategoryId))
                return new List<QuestionDto>();

            // Hämtar frågor och filtrerar på subkategori
            var questions = await _uow.Questions.GetAllAsync();

            // Returnerar frågor som DTOs
            return questions
                .Where(q => q.SubCategoryId == subCategoryId)
                .Select(q => new QuestionDto
                {
                    Id = q.Id,
                    Text = q.Text
                })
                .ToList();
        }


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
