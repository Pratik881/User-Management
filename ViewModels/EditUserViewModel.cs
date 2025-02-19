namespace UserManagementSystem.ViewModels
{
    public class EditUserViewModel
    {
        public string UserId { get; set; }
        public string FullName{ get; set; }
        public string Email { get; set; }
        public List<string> AllRoles { get; set; }
        public List<string> SelectedRoles { get; set; }
    }

}
