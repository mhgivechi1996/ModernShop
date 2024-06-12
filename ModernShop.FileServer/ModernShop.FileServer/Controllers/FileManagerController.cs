using ModernShop.FileServer.Dto;
using ModernShop.FileServer.Services;
using Microsoft.AspNetCore.Mvc;

namespace ModernShop.FileServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileManagerController : ControllerBase
    {
        private readonly IFileService fileService;

        public FileManagerController(IFileService fileService)
        {
            this.fileService = fileService;
        }

        [HttpPost("Upload")]
        public async Task<IActionResult> Upload([FromForm] FileUploadDto fileUploadInfo)
        {
            var result = await fileService.Upload(fileUploadInfo);
            return Ok(result);
        }

        [HttpPost("Uploads")]
        public async Task<IActionResult> Uploads([FromForm] List<FileUploadDto> files)
        {
            var result = await fileService.Uploads(files);
            return Ok(result);
        }
    }
}
