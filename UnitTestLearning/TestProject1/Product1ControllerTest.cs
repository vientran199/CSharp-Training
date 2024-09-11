using Microsoft.AspNetCore.Mvc;
using UnitTestLearning.Controllers;
using UnitTestLearning.Models;
using UnitTestLearning.Repository;

namespace TestProject1
{
    [TestClass]
    public class Product1ControllerTest
    {
        [TestInitialize]
        public void Initialize()
        {

        }


        [TestMethod]
        public void GetReturnsProduct_NotFound()
        {
            // Arrange
            var repository = new ProductRepository();
            var controller = new Products1Controller(repository);

            // Act
            var response = controller.Get(10);

            // Assert
            Assert.IsInstanceOfType(response, typeof(NotFoundResult));
        }

        [TestMethod]
        public void GetReturnsProduct_Found()
        {
            // Arrange
            var products = GetTestProducts();
            var repository = new ProductRepository(products);
            var controller = new Products1Controller(repository);

            // Act
            var response = controller.Get(1) as ObjectResult;

            // Assert
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Value);
            Assert.IsInstanceOfType(response, typeof(OkObjectResult));
            //Assert.AreEqual(products[0].Name, (response.Value as Product).Name);
            Assert.AreEqual(products[0], response.Value);
        }

        [TestMethod]
        public void PostProduct_1()
        {
            // Arrange
            var products = GetTestProducts();
            var repository = new ProductRepository(products);
            var controller = new Products1Controller(repository);
            Product newProduct = new Product
            {
                Id = 6,
                Name = "Demo6",
                Price = 123,
            };

            // Act
            var actionResult = controller.Post(newProduct) as ObjectResult;

            // Assert
            Assert.IsNotNull(actionResult);

            var result = actionResult.Value as Product;
            Assert.AreEqual(actionResult.StatusCode, 201);
            Assert.AreEqual(newProduct, result);
        }

        [TestMethod]
        public void PostProduct_2()
        {
            // Arrange
            var products = GetTestProducts();
            var repository = new ProductRepository(products);
            var controller = new Products1Controller(repository);
            Product newProduct = new Product
            {
                Id = 6,
                Name = "Demo6",
                Price = 123,
            };

            // Act
            var response = controller.Post(newProduct) as ObjectResult;

            // Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(response.StatusCode, 201);

            Assert.AreEqual(newProduct, response.Value);
        }

        [TestMethod]
        public async Task GetAllProduct_Success()
        {
            //Arrange
            var products = GetTestProducts();
            var repository = new ProductRepository(products);
            var controller = new Products1Controller(repository);

            //Act
            var actionResult = await controller.GetAllProduct();

            //Assert
            var result = actionResult.Result as OkObjectResult;
        
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual(products.Count, (result.Value as List<Product>).Count);
        }
        private List<Product> GetTestProducts()
        {
            var testProducts = new List<Product>();
            testProducts.Add(new Product { Id = 1, Name = "Demo1", Price = 1 });
            testProducts.Add(new Product { Id = 2, Name = "Demo2", Price = 3.75M });
            testProducts.Add(new Product { Id = 3, Name = "Demo3", Price = 16.99M });
            testProducts.Add(new Product { Id = 4, Name = "Demo4", Price = 11.00M });

            return testProducts;
        }
    }
}
