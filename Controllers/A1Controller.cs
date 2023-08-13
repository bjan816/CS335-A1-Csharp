using Microsoft.AspNetCore.Mvc;

namespace A1.Controllers
{
    [Route("webapi")]
    [ApiController]
    public class A1Controller : Controller
    {
        // GET /webapi/GetVersion
        [HttpGet("GetVersion")]
        public IActionResult GetVersion()
        {
            const string upi = "bjan816";
            const string versionString = $"1.0.0 (Ngongotahā) by {upi}";
            return Ok(versionString);
        }

        private string GetContentType(string fileName)
        {
            switch (Path.GetExtension(fileName).ToLower())
            {
                case ".png":
                {
                    return "image/png";
                }
                case ".jpg":
                case ".jpeg":
                {
                    return "image/jpeg";
                }
                case ".gif":
                {
                    return "image/gif";
                }
                default:
                {
                    return "application/octet-stream";
                }
            }
        }

        // GET /webapi/GetLogo
        [HttpGet("GetLogo")]
        public IActionResult GetLogo()
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string logoImageFilePath = Path.Combine(currentDirectory, "Logos/Logo.png");

            if (!System.IO.File.Exists(logoImageFilePath))
            {
                return NotFound();
            }

            byte[] imageBytes = System.IO.File.ReadAllBytes(logoImageFilePath);

            string contentType = GetContentType(logoImageFilePath);

            return File(imageBytes, contentType);
        }
    }
}