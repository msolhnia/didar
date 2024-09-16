using Packaging.domain.Entity.BaseEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packaging.domain.Entity
{
    public class ModuleModel : BaseEntity<int>
    {
        public int? ParentId { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool? IsChildFree { get; set; } = false;
        public string Uri { get; set; }
        public List<ModuleModel> Modules { get; set; }
    }
}
