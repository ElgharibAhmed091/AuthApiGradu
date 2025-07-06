using AuthenticationFirst.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationFirst.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
        {

        }
        public ApplicationDbContext(DbContextOptions options) : base(options) { }
        public DbSet<PasswordResetCode> PasswordResetCodes { get; set; }
        public DbSet<ChangePass> changePasses { get; set; }
        public DbSet<DeleteAccount> DeleteAccounts { get; set; }

    }
}
 