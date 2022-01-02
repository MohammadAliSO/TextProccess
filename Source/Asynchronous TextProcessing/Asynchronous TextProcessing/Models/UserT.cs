using System;
using System.Collections.Generic;

namespace Asynchronous_TextProcessing.Models
{
    public partial class UserT
    {
        public UserT()
        {
            RequestTs = new HashSet<RequestT>();
            UserPermissionTs = new HashSet<UserPermissionT>();
        }

        public long UserId { get; set; }
        public string Name { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public DateTime? CreateTime { get; set; }

        public virtual ICollection<RequestT> RequestTs { get; set; }
        public virtual ICollection<UserPermissionT> UserPermissionTs { get; set; }
    }
}
