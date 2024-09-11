using System.Web.Http.Results;
using System.Web.Http;
using UnitTestLearning.Controllers;
using UnitTestLearning.Models;
using UnitTestLearning.Repository;
using System.Net;

namespace TestProject1
{
    [TestClass]
    public class Products2ControllerTest
    {

        [TestMethod]
        public void GetReturnsProductWithSameId()
        {
            //Arrange
            var products = GetTestProducts();
            var repository = new ProductRepository(products);
            var controller = new Products2Controller(repository);

            // Act
            IHttpActionResult actionResult = controller.Get(4);
            var contentResult = actionResult as OkNegotiatedContentResult<Product>;

            // Assert
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(4, contentResult.Content.Id);
        }

        [TestMethod]
        public void GetReturnsNotFound()
        {
            // Arrange
            var products = GetTestProducts();
            var repository = new ProductRepository(products);
            var controller = new Products2Controller(repository);

            // Act
            IHttpActionResult actionResult = controller.Get(10);

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult, typeof(NotFoundResult));
        }

        [TestMethod]
        public void DeleteReturnsOk()
        {
            // Arrange
            var products = GetTestProducts();
            var repository = new ProductRepository(products);
            var controller = new Products2Controller(repository);

            // Act
            IHttpActionResult actionResult = controller.Delete(1);

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(OkResult));
        }

        [TestMethod]
        public void PostMethodSetsLocationHeader()
        {
            // Arrange
            var products = GetTestProducts();
            var repository = new ProductRepository(products);
            var controller = new Products2Controller(repository);

            // Act
            IHttpActionResult actionResult = controller.Post(new Product { Id = 10, Name = "Product1" });
            var createdResult = actionResult as CreatedAtRouteNegotiatedContentResult<Product>;

            // Assert
            Assert.IsNotNull(createdResult);
            Assert.AreEqual("DefaultApi", createdResult.RouteName);
            Assert.AreEqual(10, createdResult.RouteValues["id"]);
        }

        [TestMethod]
        public void PutReturnsContentResult()
        {
            // Arrange
            var products = GetTestProducts();
            var repository = new ProductRepository(products);
            var controller = new Products2Controller(repository);

            // Act
            IHttpActionResult actionResult = controller.Put(new Product { Id = 10, Name = "Product" });
            var contentResult = actionResult as NegotiatedContentResult<Product>;

            // Assert
            Assert.IsNotNull(contentResult);
            Assert.AreEqual(HttpStatusCode.Accepted, contentResult.StatusCode);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(10, contentResult.Content.Id);
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
