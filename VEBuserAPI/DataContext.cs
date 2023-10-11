namespace VEBuserAPI
{
    using Microsoft.EntityFrameworkCore;

    public class DataContext : DbContext
    {
        private string _dataSource;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;

        public DataContext(IConfiguration config)
        {
            _dataSource = config.GetValue<string>("DbConnection");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(_dataSource);
        }
    }
}
