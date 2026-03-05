using System.Security.Claims;
using CyberQuiz.API.Services;
using CyberQuiz.Shared.AI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CyberQuiz.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class AiController : ControllerBase
{
    private readonly AiService _ai;

    public AiController(AiService ai)
    {
        _ai = ai;
    }

    [HttpPost("chat")]
    public async Task<ActionResult<AiChatResponseDto>> Chat(
    [FromBody] AiChatRequestDto req,
    CancellationToken cancellationToken)
    {
        if (req is null)
        {
            return BadRequest("Request body is required.");
        }

        var finalPrompt = req.Context is null
            ? req.Prompt
            : $"Context (quiz): {req.Context}\n\nUser question: {req.Prompt}";

        var answer = await _ai.AskAsync(finalPrompt, cancellationToken);

        return Ok(new AiChatResponseDto
        {
            Answer = answer
        });
    }
}
