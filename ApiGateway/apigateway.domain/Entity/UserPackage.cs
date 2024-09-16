using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace apigateway.domain.Entity
{
    public class UserPackage
    {
        public string UserId { get; set; }
        public int OrganizationId { get; set; }
        public List<Module> Modules { get; set; }
    }
}
