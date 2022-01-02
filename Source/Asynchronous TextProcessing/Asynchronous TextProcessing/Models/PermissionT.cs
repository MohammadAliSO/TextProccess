using System;
using System.Collections.Generic;

namespace Asynchronous_TextProcessing.Models
{
    public partial class PermissionT
    {
        public PermissionT()
        {
            UserPermissionTs = new HashSet<UserPermissionT>();
        }

        public long PermissionId { get; set; }
        public string? Name { get; set; }
        public string? Content { get; set; }

        public virtual ICollection<UserPermissionT> UserPermissionTs { get; set; }
    }
}
