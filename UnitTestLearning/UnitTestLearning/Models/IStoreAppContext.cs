using System.Collections.Generic;
using System.Data.Entity;

namespace UnitTestLearning.Models
{
    public interface IStoreAppContext : IDisposable
    {
        DbSet<Product> Products { get; }
        int SaveChanges();
        void MarkAsModified(Product item);
    }
}
