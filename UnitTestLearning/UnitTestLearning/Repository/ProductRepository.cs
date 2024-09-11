using Microsoft.AspNetCore.Mvc;
using UnitTestLearning.Models;

namespace UnitTestLearning.Repository
{
    public class ProductRepository : IProductRepository
    {
        List<Product> _products = new List<Product>();

        public ProductRepository() { }

        public ProductRepository(List<Product> products)
        {
            _products = products;
        }

        public void AddProduct(Product product)
        {
            _products.Add(product);

            return;
        }

        public void Delete(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if(product == null)
            {
                throw new Exception("Not found");
            }

            _products.Remove(product);
        }

        public List<Product> GetAllProducts()
        {
            return _products;
        }

        public Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            throw new NotImplementedException();
        }

        public Product GetById(int id)
        {
            var product = _products.SingleOrDefault(p => p.Id ==  id);

            //if (product == null)
            //{
            //    throw new Exception("Not found");
            //}

            return product;
        }

        public IActionResult GetProduct(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> GetProductAsync(int id)
        {
            throw new NotImplementedException();
        }

    }
}
