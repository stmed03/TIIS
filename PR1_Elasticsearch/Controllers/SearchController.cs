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

            //Вариант 1 -- Прямая загрузка by Степашка
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