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

                // Returnerar DTO till UI/API (inte DAL-entiteter)
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


        //private async Task<SubProgress> GetSubCategoryProgressAsync(string userId, int subCategoryId)
        //{
            
        //}



        //Intern hjälpklass för att hålla progress
        private class SubProgress
        {
            public int QuestionCount { get; set; }
            public double ScorePercent { get; set; }
            public bool IsCompleted { get; set; }
        }
    }
}
