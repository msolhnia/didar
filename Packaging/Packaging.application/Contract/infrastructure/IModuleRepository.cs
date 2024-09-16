using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Packaging.application.Contract.infrastructure
{
    public interface IModuleRepository
    {
        Task<Module> GetByIdAsync(int id);
        Task UpdateAsync(Module module);
    }
}
