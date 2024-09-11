using System.Data.Entity;

namespace UnitTestLearning.Models
{
    public class StoreAppContext : DbContext, IStoreAppContext
    {
        public StoreAppContext() : base("name=StoreAppContext")
        {
        }

        public DbSet<Product> Products { get; set; }

        public void MarkAsModified(Product item)
        {
            Entry(item).State = EntityState.Modified;
        }
    }
}
