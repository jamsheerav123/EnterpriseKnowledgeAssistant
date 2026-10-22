using EnterpriseKnowledgeAssistant.Application.Abstractions.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EnterpriseKnowledgeAssistant.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentsController : ControllerBase
    {
        private readonly IDocumentService _documentService;

        public DocumentsController(
            IDocumentService documentService)
        {
            _documentService = documentService;
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Upload(
            IFormFile file,
            CancellationToken cancellationToken)
        {
            if (file is null || file.Length == 0)
            {
                return BadRequest("A file is required.");
            }

            if (!string.Equals(
                    Path.GetExtension(file.FileName),
                    ".pdf",
                    StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest("Only PDF files are supported.");
            }

            await using var stream = file.OpenReadStream();

            var result = await _documentService.UploadAsync(
                stream,
                file.FileName,
                file.ContentType,
                file.Length,
                cancellationToken);

            return CreatedAtAction(
                nameof(GetById),
                new { id = result.Id },
                result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(
            Guid id,
            CancellationToken cancellationToken)
        {
            // We'll implement this next.
            return Ok();
        }
    }
}
