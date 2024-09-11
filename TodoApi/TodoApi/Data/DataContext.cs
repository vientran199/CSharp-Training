
namespace dotnet_rpg.Data
{
    public class DataContext : DbContext //DataContext nay dat tuy thich
    {
        protected readonly IConfiguration Configuration;

        public DataContext(DbContextOptions<DbContext> options) : base(options) //Cai nay no se lay gia tri option tu file Program.cs
        {
            
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    base.OnConfiguring(optionsBuilder);
        //    optionsBuilder.UseSqlServer("Server=.\\SQLExpress;Database=test;Trusted_Connection=true;TrustServerCertificate=true;");
        //}

        public DbSet<Character> Characters => Set<Character>(); //Set Db character ddeer co the su dung query, No se tao ra bang Character nhu dinh nghia cua class trong DB
    }
}
