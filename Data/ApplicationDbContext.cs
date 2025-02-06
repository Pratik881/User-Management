using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
namespace UserManagementSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            :base(options)
        {
        }
    }                                                                                                                                                                                                                                                                                                                                                                                           
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
    }
        
    }


