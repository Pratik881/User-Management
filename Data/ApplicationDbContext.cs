using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using UserManagementSystem.Models;
namespace UserManagementSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            :base(options)
        {
        }
        public DbSet<Document> Documents { get; set; }

        public DbSet<DocumentUsers> DocumentUsers { get; set; }
    }                                                                                                                                                                                                                                                                                                                                                                                           
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public ICollection<Document> Documents { get; set; }
        public ICollection<DocumentUsers> DocumentUsers { get; set; }

    }
        
    }


