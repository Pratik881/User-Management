using System.ComponentModel.DataAnnotations;
using UserManagementSystem.Data;

namespace UserManagementSystem.Models
{
    public class DocumentUsers
    {
        [Key]
        public int Id { get; set; }

        public string DocumentId {  get; set; }
        public Document Document { get; set; }

        public string UserId { get; set; }  
        public ApplicationUser User { get; set; }

        public string Role { get; set; }

    }

}
