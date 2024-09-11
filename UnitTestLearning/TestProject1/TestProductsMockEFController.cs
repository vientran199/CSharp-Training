using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Results;
using UnitTestLearning.Controllers;
using UnitTestLearning.Models;
using UnitTestLearning.Tests;

namespace TestProject1
{
    [TestClass]
    public class TestProductsMockEFController
    {
        [TestMethod]
        public void PostProduct_ShouldReturnSameProduct()
        {
            var controller = new ProductsMockEFController(new TestStoreAppContext());

            var item = GetDemoProduct();

            var result =
                controller.PostProduct(item) as CreatedAtRouteNegotiatedContentResult<Product>;

            Assert.IsNotNull(result);
            Assert.AreEqual(result.RouteName, "DefaultApi");
            Assert.AreEqual(result.RouteValues["id"], result.Content.Id);
            Assert.AreEqual(result.Content.Name, item.Name);
        }

        [TestMethod]
        public void PutProduct_ShouldReturnStatusCode()
        {
            var controller = new ProductsMockEFController(new TestStoreAppContext());

            var item = GetDemoProduct();

            var result = controller.PutProduct(item.Id, item) as StatusCodeResult;

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(StatusCodeResult));
            //Assert.AreEqual(HttpStatusCode.NoContent, result.StatusCode);
        }

        [TestMethod]
        public void PutProduct_ShouldFail_WhenDifferentID()
        {
            var controller = new ProductsMockEFController(new TestStoreAppContext());

            var badresult = controller.PutProduct(999, GetDemoProduct());
            Assert.IsInstanceOfType(badresult, typeof(BadRequestResult));
        }

        [TestMethod]
        public void GetProduct_ShouldReturnProductWithSameID()
        {
            var context = new TestStoreAppContext();
            context.Products.Add(GetDemoProduct());

            var controller = new ProductsMockEFController(context);
            var result = controller.GetProduct(3) as OkNegotiatedContentResult<Product>;

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Content.Id);
        }

        [TestMethod]
        public void GetProducts_ShouldReturnAllProducts()
        {
            var context = new TestStoreAppContext();
            context.Products.Add(new Product { Id = 1, Name = "Demo1", Price = 20 });
            context.Products.Add(new Product { Id = 2, Name = "Demo2", Price = 30 });
            context.Products.Add(new Product { Id = 3, Name = "Demo3", Price = 40 });

            var controller = new ProductsMockEFController(context);
            var result = controller.GetProducts() as TestProductDbSet;

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Local.Count);
        }

        [TestMethod]
        public void DeleteProduct_ShouldReturnOK()
        {
            var context = new TestStoreAppContext();
            var item = GetDemoProduct();
            context.Products.Add(item);

            var controller = new ProductsMockEFController(context);
            var result = controller.DeleteProduct(3) as OkNegotiatedContentResult<Product>;

            Assert.IsNotNull(result);
            Assert.AreEqual(item.Id, result.Content.Id);
        }

        Product GetDemoProduct()
        {
            return new Product() { Id = 3, Name = "Demo name", Price = 5 };
        }
    }
}
