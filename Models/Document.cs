using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.ComponentModel.DataAnnotations;
using UserManagementSystem.Data;

namespace UserManagementSystem.Models
{
    public class Document
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }


        public string OwnerId { get; set; }
        public ApplicationUser Owner { get; set; }  


        public ICollection<DocumentUsers> DocumentUsers { get; set; }    
    }
}
