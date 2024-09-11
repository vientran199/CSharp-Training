using Microsoft.AspNetCore.Mvc;
using UnitTestLearning.Models;

namespace UnitTestLearning.Repository
{
    public interface IProductRepository
    {
        public List<Product> GetAllProducts();
        public Task<IEnumerable<Product>> GetAllProductsAsync();
        public IActionResult GetProduct(int id);
        public Task<IActionResult> GetProductAsync(int id);
        public Product GetById(int id); 

        public void AddProduct(Product product);

        public void Delete(int id);
    }
}
