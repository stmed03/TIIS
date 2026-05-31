using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PR1_Elasticsearch.Data;
using PR1_Elasticsearch.Services;
using PR1_Elasticsearch.Models;

namespace PR1_Elasticsearch.Controllers;

[ApiController]
[Route("api/search")]
public class SearchController : ControllerBase
{
    private readonly ArticleSearchService _service;
    private readonly ApplicationDbContext _db;

    public SearchController(ArticleSearchService service, ApplicationDbContext db)
    {
        _service = service;
        _db = db;
    }

    [HttpPost("index")]
    public async Task<IActionResult> Index([FromBody] ArticleDocument article)
    {
        // Записываем ТОЛЬКО в MariaDB
        _db.ArticleDocuments.Add(article);
        await _db.SaveChangesAsync();

        // Kafka producer и Elasticsearch убираем — CDC делает Debezium
        return Accepted();
    }

    [HttpGet]
    public async Task<IActionResult> Search([FromQuery] string q)
    {
        var result = await _service.SearchAsync(q);
        return Ok(result);
    }
}