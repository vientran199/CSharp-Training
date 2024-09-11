using System.Web.Http.Description;
using System.Web.Http.ModelBinding;
using System.Web.Http;
using UnitTestLearning.Models;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace UnitTestLearning.Controllers
{
    public class ProductsMockEFController : ApiController
    {
        // modify the type of the db field
        private IStoreAppContext db = new StoreAppContext();

        // add these constructors
        public ProductsMockEFController() { }

        public ProductsMockEFController(IStoreAppContext context)
        {
            db = context;
        }

        // POST api/Product
        [ResponseType(typeof(Product))]
        public IHttpActionResult PostProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Products.Add(product);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = product.Id }, product);
        }

        // PUT api/Product/5
        public IHttpActionResult PutProduct(int id, Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != product.Id)
            {
                return BadRequest();
            }

            //db.Entry(product).State = EntityState.Modified;
            db.MarkAsModified(product);

            // rest of method not shown
            return Ok(product);
        }

        public IHttpActionResult GetProduct(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = db.Products.SingleOrDefault(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            // rest of method not shown
            return Ok(product);
        }

        public IHttpActionResult GetProducts()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

 
            // rest of method not shown
            return Ok(db);
        }

        public IHttpActionResult DeleteProduct(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = db.Products.SingleOrDefault(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            db.Products.Remove(product);

            // rest of method not shown
            return Ok(product);
        }
    }
}
