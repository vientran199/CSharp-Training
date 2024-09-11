using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Collections.Generic;
using UnitTestLearning.Controllers;
using UnitTestLearning.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;

namespace TestProject1
{
    [TestClass]
    public class ProductControllerTest
    {
        [TestInitialize]
        public void Initialize ()
        {
            
        }


        [TestMethod]
        public void WeatherForecast_Get()
        {
            //Arrange: Set up any prerequisites for the test to run.
            var controller = new WeatherForecastController();

            //Act: Perform the test.
            var summaries = controller.Get();

            //Assert: Verify that the test succeeded.
            Assert.IsNotNull(summaries);
            Assert.AreEqual(5, summaries.Count());
        }

        [TestMethod]
        public void GetAllProducts_ShouldReturnAllProducts()
        {
            var testProducts = GetTestProducts();
            var controller = new ProductController(testProducts);

            var result = controller.GetAllProducts() as List<Product>;

            Assert.IsNotNull(result);
            Assert.AreEqual(testProducts.Count, result.Count);
        }

        [TestMethod]
        public async Task GetAllProductsAsync_ShouldReturnAllProducts()
        {
            var testProducts = GetTestProducts();
            var controller = new ProductController(testProducts);

            var result = await controller.GetAllProductsAsync() as List<Product>;

            Assert.IsNotNull(result);
            Assert.AreEqual(testProducts.Count, result.Count);
        }

        [TestMethod]
        public void GetProduct_ShouldReturnCorrectProduct()
        {
            var testProducts = GetTestProducts();
            var controller = new ProductController(testProducts);

            var result = controller.GetProduct(4) as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(testProducts[3], result.Value);
        }

        [TestMethod]
        public async Task GetProductAsync_ShouldReturnCorrectProduct()
        {
            var testProducts = GetTestProducts();
            var controller = new ProductController(testProducts);

            var result = await controller.GetProductAsync(4) as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(testProducts[3], result.Value);
        }

        [TestMethod]
        public void GetProduct_ShouldNotFindProduct()
        {
            var controller = new ProductController(GetTestProducts());

            var result = controller.GetProduct(999);

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
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

        [TestMethod]
        public void Test()
        {
            var testProducts = GetTestProducts();

            Assert.AreEqual(testProducts.Count, testProducts.Count);
        }
    }
}

//1. The action returns the correct type of response.
//2. Invalid parameters return the correct error response.
//3. The action calls the correct method on the repository or service layer.
//4. If the response includes a domain model, verify the model type.