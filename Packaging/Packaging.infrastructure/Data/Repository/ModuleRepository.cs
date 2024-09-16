using Packaging.application.Contract.infrastructure;
using Packaging.infrastructure.Data.Persist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Packaging.infrastructure.Data.Repository
{
    public class ModuleRepository : IModuleRepository
    {
        private readonly PackagingContext _dbContext;

        public ModuleRepository(PackagingContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Module> GetByIdAsync(int id)
        {
            return await _dbContext.Modules.FindAsync(id);
        }

        public async Task UpdateAsync(Module module)
        {
            _dbContext.Modules.Update(module);
            await _dbContext.SaveChangesAsync();
        }
    }
}
