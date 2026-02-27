using CyberQuiz.BLL.Interfaces;
using CyberQuiz.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace CyberQuiz.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuizController : ControllerBase
    {
        //  Koppling till BLL (Dependency Injection)
        private readonly IQuizService _quizService;

        public QuizController(IQuizService quizService)
        {
            _quizService = quizService;
        }

        // GET: api/quiz/categories?userId=xxx
        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories([FromQuery] string userId)
        {
            // Validering (bra att ha i API också)
            if (string.IsNullOrEmpty(userId))
                return BadRequest("userId is required");

            var result = await _quizService.GetCategoriesAsync(userId);

            return Ok(result); // returnerar JSON till UI
        }



        // GET: api/quiz/subcategories?categoryId=1&userId=xxx
        [HttpGet("subcategories")]
        public async Task<IActionResult> GetSubCategories(
            [FromQuery] int categoryId,
            [FromQuery] string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return BadRequest("userId is required");

            var result = await _quizService.GetSubCategoriesAsync(categoryId, userId);

            return Ok(result);
        }


        // GET: api/quiz/questions?subCategoryId=1&userId=xxx
        [HttpGet("questions")]
        public async Task<IActionResult> GetQuestions(
            [FromQuery] int subCategoryId,
            [FromQuery] string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return BadRequest("userId is required");

            var result = await _quizService.GetQuestionsAsync(subCategoryId, userId);

            return Ok(result);
        }


        // POST: api/quiz/submit-answer?userId=xxx
        [HttpPost("submit-answer")]
        public async Task<IActionResult> SubmitAnswer(
            [FromQuery] string userId,
            [FromBody] SubmitAnswerRequestDto request)
        {
            if (string.IsNullOrEmpty(userId))
                return BadRequest("userId is required");

            var result = await _quizService.SubmitAnswerAsync(userId, request);

            return Ok(result);
        }
    }
}