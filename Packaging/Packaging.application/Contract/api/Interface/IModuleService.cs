using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Packaging.application.Contract.api.Interface
{
    public interface IModuleService
    {
        Task UpdateModuleAsync(int id, Module updatedModule);
    }
}
