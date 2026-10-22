using EnterpriseKnowledgeAssistant.Application.Abstractions.AI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EnterpriseKnowledgeAssistant.API.Controllers
{
    [ApiController]
    [Route("api/search")]
    public class SearchController : ControllerBase
    {
        private readonly ISemanticSearchService _searchService;

        public SearchController(
            ISemanticSearchService searchService)
        {
            _searchService = searchService;
        }
        //search chunks of text from the vector database using semantic search
        [HttpGet]
        public async Task<IActionResult> Search(
            [FromQuery] string query,
            [FromQuery] int topK = 5,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest("Query is required.");
            }

            var results =
                await _searchService.SearchAsync(
                    query,
                    topK,
                    cancellationToken);

            return Ok(results);
        }
    }
}
