using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Vault.Index.IServices;

namespace Vault.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentsController : ControllerBase
    {
        private readonly IElasticSearchService _elasticService;
        private readonly ILogger<DocumentsController> _logger;

        public DocumentsController(IElasticSearchService elasticService, ILogger<DocumentsController> logger)
        {
            _elasticService = elasticService;
            _logger =  logger;
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string query)
        {
            if (!ModelState.IsValid || string.IsNullOrWhiteSpace(query))
            {
                return BadRequest("Model State is Invalid or query was empty");
            }

            try
            {
                var results = await _elasticService.SearchDocumentAsync(query);
                return Ok(results);
            }catch(Exception)
            {
                _logger.LogError("Error searching the document for query: " + query);
                return StatusCode(500, "Internal Server Error");
            }
            
        }
    }
}
