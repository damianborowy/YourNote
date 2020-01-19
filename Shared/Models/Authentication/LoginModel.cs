using System.ComponentModel.DataAnnotations;

namespace YourNote.Shared.Models
{
    public class LoginModel
    {
        [Required]
        public string EmailAddress { get; set; }

        [Required]
        public string Password { get; set; }
    }
}