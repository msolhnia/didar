using apigateway.application.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace apigateway.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ModuleAccessService _moduleAccessService;

        public AuthController(ModuleAccessService moduleAccessService)
        {
            _moduleAccessService = moduleAccessService;
        }

        [HttpGet("authorize/{moduleId}")]
        public async Task<IActionResult> Authorize(int moduleId)
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var isAuthorized = await _moduleAccessService.CanAccessModuleAsync(token, moduleId);

            if (isAuthorized)
                return Ok("Authorized");
            return Unauthorized();
        }
    }
}
