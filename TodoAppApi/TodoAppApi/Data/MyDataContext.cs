using Microsoft.EntityFrameworkCore;
using TodoAppApi.Models;

namespace TodoAppApi.Data
{
    public class MyDataContext : DbContext
    {
        //public MyDataContext(DbContextOptions<DbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Data Source=111.111.11.1; Initial Catalog=TodoAppTraining; User Id=user; Password=pass;Integrated Security=False;Trusted_Connection=False;TrustServerCertificate=true;");
        }

        public DbSet<Job> Jobs { get; set; }
        public DbSet<Image> Images => Set<Image>();

        public DbSet<Category> Categories => Set<Category>();

        public DbSet<User> Users => Set<User>();

        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
        //Dung fluent Api de tao constraints

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.UserName).IsUnique();
                entity.Property(e => e.Name).IsRequired().HasMaxLength(256);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(256);
            });
        }
    }
}
