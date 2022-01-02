using System.ComponentModel.DataAnnotations;

namespace Asynchronous_TextProcessing.Models
{
    public class baseRequestModel
    {
        [Required]
        public string Name { get; set; }

    }
    public class TextProcessRequestModel :baseRequestModel
    {
        [Required]
        public string Text { get; set; }
    }

}
