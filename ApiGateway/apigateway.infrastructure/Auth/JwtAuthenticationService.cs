using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace apigateway.infrastructure.Auth
{
    public class JwtAuthenticationService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public JwtAuthenticationService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetUserIdFromToken()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return userId;
        }

        public bool IsUserAuthorizedForModule(string userId, int moduleId)
        {
            // Validate if user is authorized to access the module
            // This can be based on Redis cached data for quick checks
            return true;  
        }
    }

}
