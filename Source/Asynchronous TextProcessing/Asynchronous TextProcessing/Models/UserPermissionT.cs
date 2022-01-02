using System;
using System.Collections.Generic;

namespace Asynchronous_TextProcessing.Models
{
    public partial class UserPermissionT
    {
        public long UserPermissionId { get; set; }
        public long UserId { get; set; }
        public long PermissionId { get; set; }

        public virtual PermissionT Permission { get; set; } = null!;
        public virtual UserT User { get; set; } = null!;
    }
}
