using CyberQuiz.API.Services;
using CyberQuiz.DAL.Data;
using CyberQuiz.Shared.AI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace CyberQuiz.API.Controllers;

[ApiController]
[Route("api/[controller]")]

public class AiController : ControllerBase
{
    private readonly AiService _ai;
    private readonly CyberQuizDbContext _db;

    public AiController(AiService ai, CyberQuizDbContext db)
    {
        ArgumentNullException.ThrowIfNull(ai);
        ArgumentNullException.ThrowIfNull(db);
        _ai = ai;
        _db = db;
    }

    [HttpPost("chat")]
    public async Task<IActionResult> Chat([FromBody] AiChatRequestDto? req, CancellationToken cancellationToken)
    {
        //Validate the request body, recieve the promp from the user
        if (req is null || string.IsNullOrWhiteSpace(req.Prompt))
        {
            return BadRequest("Prompt is required.");
        }

        //Hämta quiz topics från db
        var categories = await _db.Categories.ToListAsync(cancellationToken);
        var subCategories = await _db.SubCategories.ToListAsync(cancellationToken);

        var context = string.Join("\n", categories
            .OrderBy(c => c.Id)
            .Select(c =>
            {
                var subs = subCategories
                    .Where(sc => sc.CategoryId == c.Id)
                    .OrderBy(sc => sc.Id)
                    .Select(sc => $"- {sc.Name}");

                return $"Category: {c.Name}\n{string.Join("\n", subs)}";
            })
        );
        // finalPrompt will be the prompt we send to the AI.
        // If context is provided, we include it in the prompt.

        var finalPrompt = $@"
        You are a CyberQuiz chatbot.
        Only answer questions related to the topics listed below.
        If outside topics, say you only help with CyberQuiz topics.
        Answer clearly and simply for a student.

        Topics:
        {context}

        User question:
        {req.Prompt}
        ";



        //Promt: inputText from the user
        // Context: additional information that can help the AI provide a better answer(ex. quiz questions, code snippets, etc.)

        //if (!string.IsNullOrWhiteSpace(req.Context))
        //{
        //    finalPrompt = $"Context (quiz): {req.Context}\n\n User question: {req.Prompt}";
        //}


        // Call AiService (AskAsyn in Service: Send promt to AI and Recieve data from AI as string)
        var answer = await _ai.AskAsync(finalPrompt, cancellationToken);


        // Return an answer to the Client as JSON (Answer from AI)
        return Ok(new AiChatResponseDto
        {
            Answer = answer
        });
    }
}



