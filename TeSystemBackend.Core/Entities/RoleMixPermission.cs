using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeSystemBackend.Core.Entities
{
    public class RoleMixPermission
    {
        public long RoleId { get; set; }
        public Role Role { get; set; } = null!;

        public long PermissionId { get; set; }
        public Permission Permission { get; set; } = null!;
    }
}
