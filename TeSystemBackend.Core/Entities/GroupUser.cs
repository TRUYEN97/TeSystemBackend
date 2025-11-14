using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeSystemBackend.Core.Entities
{
    public class GroupUser
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public List<UserMixGroupUser> Members { get; set; } = new();
    }
}
