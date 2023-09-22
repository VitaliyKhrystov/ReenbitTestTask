using Microsoft.AspNetCore.Mvc;
using WebAppReenbitTest.Services;

namespace WebAppReenbitTest.Controllers
{
    [Route("{controller}")]
    public class FilesController : Controller
    {
        private readonly FileService fileService;
        private readonly ILogger<FilesController> logger;

        public FilesController(FileService fileService, ILogger<FilesController> logger)
        {
            this.fileService = fileService;
            this.logger = logger;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadAsync(IFormFile file)
        {
            string email = "";
            foreach (var key in Request.Form.Keys)
            {
                if (key == "email")
                {
                    email = Request.Form[key];
                    break;
                }
                   
            }

            try
            {
                var status = await fileService.UploadFileAsync(file, email);
                return Ok(status);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return BadRequest();
            }
            
        }
    }
}
