using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeSystemBackend.Core.Entities
{
    public class UserMixGroupUser
    {
        public long UserId { get; set; }
        public AppUser User { get; set; } = null!;

        public long GroupUserId { get; set; }
        public GroupUser GroupUser { get; set; } = null!;

        public bool IsApproved { get; set; } = false;
    }
}
