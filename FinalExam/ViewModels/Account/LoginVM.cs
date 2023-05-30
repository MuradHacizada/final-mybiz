using System.ComponentModel.DataAnnotations;

namespace FinalExam.ViewModels.Account
{
    public class LoginVM
    {
        [Required]
        [MinLength(3)]
        [MaxLength(25)]
        public string UsernameOreEmail { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool IsRemember { get; set; }
    }
}
