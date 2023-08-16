using A1.Data;
using A1.Dtos;
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

        private string GetContentType(string prefix, string fileName)
        {
            string extension = Path.GetExtension(fileName).ToLower();
            return $"{prefix}{extension}";
        }

        private byte[]? ReadAllBytes(string filePath)
        {
            if (!System.IO.File.Exists(filePath))
            {
                return null;
            }

            byte[] imageBytes = System.IO.File.ReadAllBytes(filePath);

            return imageBytes;
        }

        // GET /webapi/GetVersion
        [HttpGet("GetVersion")]
        public IActionResult GetVersion()
        {
            const string upi = "bjan816";
            const string versionString = $"1.0.0 (Ngongotahā) by {upi}";
            return Ok(versionString);
        }

        // GET /webapi/GetLogo
        [HttpGet("GetLogo")]
        public IActionResult GetLogo()
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string logoImageFilePath = Path.Combine(currentDirectory, "Logos/Logo.png");

            byte[]? imageBytes = ReadAllBytes(logoImageFilePath);

            if (imageBytes == null)
            {
                return NotFound();
            }

            string contentType = GetContentType("image/", logoImageFilePath);

            return File(imageBytes, contentType);
        }

        // GET /webapi/AllItems

        [HttpGet("AllItems")]
        public async Task<IActionResult> GetAllProducts()
        {
            IEnumerable<Product> products = await _repository.GetAllProducts();

            return Ok(products);
        }

        // GET / webapi/Items/{searchTerm}

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

        // GET /webapi/{itemId}

        [HttpGet("{itemId}")]
        public IActionResult GetItemImage(int itemId)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string itemImagesDirectory = Path.Combine(currentDirectory, "ItemsImages/");
            string[] files = Directory.GetFiles(itemImagesDirectory, itemId + ".*");
            string imageFilePath;

            if (files.Length <= 0)
            {
                imageFilePath = Path.Combine(currentDirectory, "ItemsImages/default.png");
            }
            else
            {
                imageFilePath = files[0];
            }

            byte[]? imageBytes = ReadAllBytes(imageFilePath);

            if (imageBytes == null)
            {
                return NotFound();
            }

            string contentType = GetContentType("image/", imageFilePath);

            return File(imageBytes, contentType);
        }

        // GET /webapi/GetComment/{commentId}

        [HttpGet("GetComment/{commentId}")]
        public async Task<IActionResult> GetComment(int commentId)
        {
            Comment? comment = await _repository.GetCommentById(commentId);

            if (comment == null)
            {
                string errorMessage = $"Comment {commentId} does not exist.";
                return BadRequest(errorMessage);
            }

            return Ok(comment);
        }

        // POST /webapi/WriteComment

        [HttpPost("WriteComment")]
        public async Task<IActionResult> WriteComment([FromBody] CommentInput commentInput)
        {
            if (string.IsNullOrWhiteSpace(commentInput.UserComment) || string.IsNullOrWhiteSpace(commentInput.Name))
            {
                return BadRequest("Both UserComment and Name must be provided.");
            }

            Comment newComment = new Comment
            {
                UserComment = commentInput.UserComment,
                Name = commentInput.Name,
                Time = DateTime.UtcNow,
                IP = Request.HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown"
            };

            await _repository.AddComment(newComment);

            return CreatedAtAction(nameof(GetComment), new { commentId = newComment.Id }, newComment);
        }

        // GET /webapi/Comments

        [HttpGet("Comments")]
        public IActionResult Comments(int? count = 5)
        {
            if (count <= 0)
            {
                return BadRequest("Count parameter must be a positive integer.");
            }

            List<Comment> comments = _repository.GetMostRecentComments(count.Value).ToList();

            return Ok(comments);
        }
    }
}