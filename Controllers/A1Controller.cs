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
    }
}