// MinioApi/Controllers/FilesController.cs
using Microsoft.AspNetCore.Mvc;

namespace MinioApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FilesController : ControllerBase
    {
        [HttpGet("list")]
        public IActionResult ListFiles()
        {
            return Ok(new List<string> { "test-file.txt" });
        }
    }
}