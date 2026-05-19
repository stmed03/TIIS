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

        public SearchController(ArticleSearchService service)
        {
            _service = service;
        }

        [HttpGet("index")]
        public async Task<IActionResult> Index()
        {
            var documents = new[]
            {
            new ArticleDocument { Id = 1, Content = "Моя первая статья по ASP.NET Core и Elasticsearch" },
            new ArticleDocument { Id = 2, Content = "Полнотекстовый поиск в .NET" },
            new ArticleDocument { Id = 3, Content = "Работа с Elasticsearch 9 - быстрый старт" }
        };

            await _service.IndexAsync(documents);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] string q)
        {
            var result = await _service.SearchAsync(q);
            return Ok(result);
        }
    }
}
