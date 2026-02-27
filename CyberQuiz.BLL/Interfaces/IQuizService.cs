using CyberQuiz.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace CyberQuiz.BLL.Interfaces
{
    //BLL-kontrakt för quiz-logiken
    //Anropas av API
    public interface IQuizService
    {
        Task<List<CategoryDto>> GetCategoriesAsync(string userId);
        Task<List<SubCategoryDto>> GetSubCategoriesAsync(int categoryId, string userId);
        Task<List<QuestionDto>> GetQuestionsAsync(int subCategoryId, string userId);
        Task<SubmitAnswerResponseDto> SubmitAnswerAsync(string userId, SubmitAnswerRequestDto request);
    }
}
