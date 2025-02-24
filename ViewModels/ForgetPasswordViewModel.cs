using System.ComponentModel.DataAnnotations;

namespace UserManagementSystem.ViewModels
{
    public class ForgetPasswordViewModel
    {
        [Required]
        public string email;
    }
}
