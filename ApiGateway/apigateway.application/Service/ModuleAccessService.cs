using apigateway.infrastructure.Auth;
using apigateway.infrastructure.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace apigateway.application.Service
{
    public class ModuleAccessService
    {
        private readonly RedisCacheService _cacheService;
        private readonly JwtAuthenticationService _authService;

        public ModuleAccessService(RedisCacheService cacheService, JwtAuthenticationService authService)
        {
            _cacheService = cacheService;
            _authService = authService;
        }

        public async Task<bool> CanAccessModuleAsync(string token, int moduleId)
        {
            var userId = _authService.GetUserIdFromToken();
            if (string.IsNullOrEmpty(userId)) return false;

            // Check the user's access level from the cache
            var cachedModule = await _cacheService.GetModuleCacheAsync(moduleId.ToString());
            if (cachedModule != null)
            {
                var module = JsonSerializer.Deserialize<Module>(cachedModule);
                return _authService.IsUserAuthorizedForModule(userId, module.Id);
            }

            return false;
        }
    }

}
