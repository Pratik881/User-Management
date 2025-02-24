using System.ComponentModel.DataAnnotations;

namespace UserManagementSystem.ViewModels
{
    public class ForgetPasswordViewModel
    {
        [Required, EmailAddress]
        public string Email { get; set; }
    }
}
