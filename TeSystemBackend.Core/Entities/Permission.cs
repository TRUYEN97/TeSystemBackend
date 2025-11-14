using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeSystemBackend.Core.Entities
{
    public class Permission
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public List<RoleMixPermission> RoleMixPermissions { get; set; } = new();
    }
}
