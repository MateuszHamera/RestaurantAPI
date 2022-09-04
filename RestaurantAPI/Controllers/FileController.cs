using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace RestaurantAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class FileController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetFile(string fileName)
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
    }
}
