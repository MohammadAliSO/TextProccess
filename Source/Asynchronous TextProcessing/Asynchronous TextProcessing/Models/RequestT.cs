using System;
using System.Collections.Generic;

namespace Asynchronous_TextProcessing.Models
{
    public partial class RequestT
    {
        public long Id { get; set; }
        public long? UserId { get; set; }
        public string Name { get; set; } = null!;
        public string? RequestData { get; set; }
        public byte Type { get; set; }
        public byte? State { get; set; }
        public long? ResultId { get; set; }
        public DateTime? CreateDateTime { get; set; }
        public byte? Status { get; set; }
        public string? Description { get; set; }

        public virtual ResultT? Result { get; set; }
        public virtual UserT? User { get; set; }
    }
}
