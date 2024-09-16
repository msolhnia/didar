using EventBus.Interface;
using Packaging.application.Contract.api.Interface;
using Packaging.application.Contract.infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Packaging.application.Service
{
    public class ModuleService : IModuleService
    {
        private readonly IModuleRepository _moduleRepository;
        private readonly IEventBus _eventBus;

        public ModuleService(IModuleRepository moduleRepository, IEventBus eventBus)
        {
            _moduleRepository = moduleRepository;
            _eventBus = eventBus;
        }

        public async Task UpdateModuleAsync(int id, Module updatedModule)
        {
            var module = await _moduleRepository.GetByIdAsync(id);

            if (module == null)
            {
                throw new Exception("Module not found");
            }

            // Update the module properties
            module.Name = updatedModule.Name;
            module.Description = updatedModule.Description;
            module.Uri = updatedModule.Uri;             
            await _moduleRepository.UpdateAsync(module);

            _eventBus.Publish("ModuleUpdated", new { Id = module.Id, Name = module.Name });
        }
    }

}
