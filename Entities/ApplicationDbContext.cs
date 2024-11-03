using Microsoft.EntityFrameworkCore;

namespace TodoApp.Entities
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        public DbSet<UserAccount> UserAccounts { get; set; }
        public DbSet<Task> Tasks { get; set; }
    }
}
