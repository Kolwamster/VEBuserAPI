namespace VEBuserAPI
{
    using Microsoft.EntityFrameworkCore;

    public class UserContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;
        public UserContext() { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=vebuser.db");
        }
    }
}
