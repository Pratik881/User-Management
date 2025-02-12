using System.Collections.Generic;

namespace UserManagementSystem.Models
{
    public class AssignRoleViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public List<RoleModel> Roles { get; set; } = new List<RoleModel>();
    }

    public class RoleModel
    {
        public string RoleName { get; set; }
        public bool IsAssigned { get; set; }
    }
}
