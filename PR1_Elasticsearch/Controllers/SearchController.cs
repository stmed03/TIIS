using Microsoft.AspNetCore.Mvc;
using PR1_Elasticsearch.Models;
using PR1_Elasticsearch.Services;

namespace PR1_Elasticsearch.Controllers
{
    [ApiController]
    [Route("api/search")]
    public class SearchController : ControllerBase
    {
        private readonly ArticleSearchService _service;
        private readonly ArticleKafkaProducer _kafkaProducer;

        public SearchController(ArticleSearchService service, ArticleKafkaProducer kafkaProducer)
        {
            _service = service;
            _kafkaProducer = kafkaProducer;
        }

        [HttpPost("index")]
        public async Task<IActionResult> Index([FromBody] ArticleDocument article)
        {
            await _kafkaProducer.PublishAsync(article);
            return Accepted(); // или 202 Accepted
        }

        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] string q)
        {
            var result = await _service.SearchAsync(q);
            return Ok(result);
        }
    }
}