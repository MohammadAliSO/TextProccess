using System;
using System.Collections.Generic;

namespace Asynchronous_TextProcessing.Models
{
    public partial class ResultT
    {
        public ResultT()
        {
            RequestTs = new HashSet<RequestT>();
        }

        public long Id { get; set; }
        public string? ResultData { get; set; }
        public DateTime? StartProcessTime { get; set; }
        public DateTime? EndProcessTime { get; set; }
        public byte? Type { get; set; }
        public byte? Status { get; set; }
        public string? Description { get; set; }

        public virtual ICollection<RequestT> RequestTs { get; set; }
    }
}
