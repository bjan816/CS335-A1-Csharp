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
            var contentTypeMapping = new Dictionary<string, string>()
            {
                {"jpg", "jpeg" },
                {"PDF", "pdf" }
            };

            string extension = Path.GetExtension(fileName).ToLower();

            if (extension.Length >= 1)
            {
                // Trim the first character
                extension = extension[1..];
            }

            if (contentTypeMapping.ContainsKey(extension))
            {
                extension = contentTypeMapping[extension];
            }

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
        public ActionResult GetVersion()
        {
            const string upi = "bjan816";
            const string versionString = $"1.0.0 (Ngongotahā) by {upi}";
            return Ok(versionString);
        }

        // GET /webapi/Logo
        [HttpGet("Logo")]
        public ActionResult GetLogo()
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string logoImageFilePath = Path.Combine(currentDirectory, "Logos/Logo.png");
            string responseHeader = "image/png";
            if (System.IO.File.Exists(logoImageFilePath))
            {
                return PhysicalFile(logoImageFilePath, responseHeader);
            }
            return NotFound();
        }

        // GET /webapi/AllItems

        [HttpGet("AllItems")]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllProducts()
        {
            IEnumerable<Product> products = await _repository.GetAllProducts();

            return Ok(products);
        }

        // GET / webapi/Items/{searchTerm}

        [HttpGet("Items/{searchTerm}")]
        public async Task<ActionResult> GetItems(string searchTerm)
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

        // GET /webapi/ItemImage/{itemId}

        [HttpGet("ItemImage/{itemId}")]
        public ActionResult GetItemImage(string itemId)

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

            string contentType = GetContentType("image/", imageFilePath);
            return PhysicalFile(imageFilePath, contentType);
        }

        // GET /webapi/GetComment/{commentId}

        [HttpGet("GetComment/{commentId}")]
        public async Task<ActionResult> GetComment(int commentId)
        {
            Comment comment = await _repository.GetCommentById(commentId);

            if (comment == null)
            {
                string errorMessage = $"Comment {commentId} does not exist.";
                return BadRequest(errorMessage);
            }

            return Ok(comment);
        }

        // POST /webapi/WriteComment

        [HttpPost("WriteComment")]
        public async Task<ActionResult> WriteComment([FromBody] CommentInput commentInput)
        {
            if (string.IsNullOrWhiteSpace(commentInput.UserComment) || string.IsNullOrWhiteSpace(commentInput.Name))
            {
                return BadRequest("Both UserComment and Name must be provided.");
            }

            Comment newComment = new Comment
            {
                UserComment = commentInput.UserComment,
                Name = commentInput.Name,
                Time = DateTime.UtcNow.ToString("yyyyMMddTHHmmssZ"),
                IP = Request.HttpContext.Connection.RemoteIpAddress?.ToString() ?? ""
            };

            await _repository.AddComment(newComment);

            return CreatedAtAction(nameof(GetComment), new { commentId = newComment.Id }, newComment);
        }

        // GET /webapi/Comments

        [HttpGet("Comments/{count?}")]
        public async Task<ActionResult> Comments(int count = 5)
        {
            var enumerableAllComments = await _repository.GetAllComments();
            var allComments = enumerableAllComments.ToList();

            List<Comment> comments = new List<Comment>();

            if (allComments.Any())
            {
                if (count > allComments.Count())
                {
                    count = allComments.Count();
                }

                comments = allComments
                    .OrderByDescending(c => c.Time)
                    .Take(count)
                    .ToList();
            }

            return Ok(comments);
        }
    }
}