using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityDemoApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        [Authorize]
        [HttpGet]
        public IActionResult GetLogin()
        {
            return Ok($"��� �����: {User.Identity?.Name}");
        }
    }
}