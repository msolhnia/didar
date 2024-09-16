using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Packaging.application.Contract.api.Interface;
using System.Reflection;

namespace Packaging.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModuleController : ControllerBase
    {
        private readonly IModuleService _moduleService;

        public ModuleController(IModuleService moduleService)
        {
            _moduleService = moduleService;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateModule(int id, [FromBody] Module updatedModule)
        {
            try
            {
                await _moduleService.UpdateModuleAsync(id, updatedModule);
                return Ok("Module updated successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
