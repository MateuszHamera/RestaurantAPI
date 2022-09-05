using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace RestaurantAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    //[Authorize]
    public class FileController : ControllerBase
    {
        [HttpGet]
        [ResponseCache(Duration =1200, VaryByQueryKeys = new string[] { "fileName"})]
        public async Task<IActionResult> Get(string fileName)
        {
            var root = Directory.GetCurrentDirectory();

            var filePath = @$"{root}\PrivateFiles\{fileName}";

            if(!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            var provider = new FileExtensionContentTypeProvider();
            provider.TryGetContentType(filePath, out var contentType);

            var fileContents = System.IO.File.ReadAllBytes(filePath);

            return File(fileContents, contentType, fileName);
        }

        [HttpPost]
        public async Task<IActionResult> Upload([FromForm]IFormFile file)
        {
            if(file == null && file.Length == 0)
                return BadRequest();

            var root = Directory.GetCurrentDirectory();

            var filePath = @$"{root}\PrivateFiles\{file.FileName}";

            using var stream = new FileStream(filePath, FileMode.Create);

            file.CopyTo(stream);

            return Ok();
        }
    }
}
