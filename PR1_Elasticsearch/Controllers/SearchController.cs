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

        private readonly KafkaProducerService _kafkaProducer;

        public SearchController(ArticleSearchService service, KafkaProducerService kafkaProducer)
        {
            _service = service;
            _kafkaProducer = kafkaProducer;

        }

        [HttpGet("index")]
        public async Task<IActionResult> Index()
        {
            await _kafkaProducer.ProduceAsync(
                topic: "demo-topic",
                message: "Hello from ASP.NET Core"
            );

            //var documents = new[]
            //{
            //    new ArticleDocument { Id = 1, Name = "Первый", Content = "Моя первая статья по ASP.NET Core и Elasticsearch" },
            //    new ArticleDocument { Id = 2, Name = "Второй", Content = "Полнотекстовый поиск в .NET" },
            //    new ArticleDocument { Id = 3, Name = "Третий", Content = "Работа с Elasticsearch 9 - быстрый старт" }
            //};

            //await _service.IndexAsync(documents);



            return Ok("Документы проиндексированы");
        }

        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] string q)
        {
            var result = await _service.SearchAsync(q);
            return Ok(result);
        }
    }
}