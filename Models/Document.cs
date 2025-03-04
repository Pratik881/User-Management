using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UserManagementSystem.Data;

namespace UserManagementSystem.Models
{
    public class Document
    {
        [Key]
        public string? Id  { get; set; }

        [Required(ErrorMessage ="title is required")]
        public string Title { get; set; }

        [Required(ErrorMessage ="Content is required")]
        public string Content { get; set; }


        public string? OwnerId { get; set; }
        public ApplicationUser? Owner { get; set; }  


        public ICollection<DocumentUsers>? DocumentUsers { get; set; }
		[NotMapped]
		public bool IsOwner { get; set; }

	}
}
