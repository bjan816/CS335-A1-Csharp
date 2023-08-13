using A1.Data;
using A1.Models;
using Microsoft.AspNetCore.Mvc;

namespace A1.Controllers
{
    [Route("webapi")]
    [ApiController]
    public class A1Controller : Controller
    {
        private readonly IA1Repo _repository;

        public A1Controller(IA1Repo repository)
        {
            _repository = repository;
        }

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

        [HttpGet("AllItems")]
        public async Task<IActionResult> GetAllProducts()
        {
            IEnumerable<Product> products = await _repository.GetAllProducts();

            return Ok(products);
        }

        [HttpGet("Items/{searchTerm}")]
        public async Task<IActionResult> GetItems(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return BadRequest("Search term must not be empty.");
            }

            IEnumerable<Product> matchedProducts = await _repository.GetAllProducts();

            matchedProducts = matchedProducts
                .Where(p => p.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                .ToList();

            return Ok(matchedProducts);
        }
    }
}