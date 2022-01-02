using System.ComponentModel.DataAnnotations;

namespace Asynchronous_TextProcessing.Models
{
    public class UserModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
 


}
