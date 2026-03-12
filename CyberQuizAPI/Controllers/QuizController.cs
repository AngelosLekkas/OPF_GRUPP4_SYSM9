using CyberQuiz.BLL.Interfaces;
using CyberQuiz.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace CyberQuiz.API.Controllers
{
    //  API Controller for “Quiz endpoints” 
    [ApiController]
    [Route("api/[controller]")]
    public class QuizController : ControllerBase
    {
        private readonly IQuizService _quizService;

        public QuizController(IQuizService quizService)
        {
            _quizService = quizService;
        }

        // 1. Returns all Categories

        // GET: /api/quiz/categories
        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories([FromQuery] string userId)
        {
            // Validering (bra att ha i API också)
            if (string.IsNullOrEmpty(userId))
                return BadRequest("userId is required");

            var result = await _quizService.GetCategoriesAsync(userId);

            return Ok(result); // returnerar JSON till UI
        }


        // 2. Return SubCategories of this category

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



        //3. Return Questions of this subcategory
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


        // 4. Submit an answer
        // POST: api/quiz/submit-answer?userId=xxx
        [HttpPost("submit-answer")]
        public async Task<IActionResult> SubmitAnswer(string userId, SubmitAnswerRequestDto request)
        {
            try
            {
                var result = await _quizService.SubmitAnswerAsync(userId, request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message }); // 🔥 JSON istället
            }
        }


        // 5. Get user progress for all subcategories (for progress bar in UI)
        //hämta (GET) userprogress : 
        [HttpGet("user-progress")]
        public async Task<ActionResult<UserProgressDto>> GetUserProgress(string userId)
        {
            var progress = await _quizService.GetUserProgressAsync(userId);
            return Ok(progress);
        }
    }
}


