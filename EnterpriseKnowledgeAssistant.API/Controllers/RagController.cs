using EnterpriseKnowledgeAssistant.Application.Abstractions.AI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EnterpriseKnowledgeAssistant.API.Controllers
{
    [ApiController]
    [Route("api/rag")]
    public class RagController : ControllerBase
    {
        private readonly IRagService _ragService;

        public RagController(IRagService ragService)
        {
            _ragService = ragService;
        }

        [HttpGet("ask")]
        public async Task<IActionResult> Ask(
            [FromQuery] string question,
            [FromQuery] int topK = 5,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(question))
                return BadRequest("Question is required.");

            var response =
                await _ragService.AskAsync(
                    question,
                    topK,
                    cancellationToken);

            return Ok(response);
        }
    }
}
