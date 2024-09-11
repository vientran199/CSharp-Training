using System.Net;
using System.Web.Http;
using UnitTestLearning.Models;
using UnitTestLearning.Repository;

namespace UnitTestLearning.Controllers
{
    public class Products2Controller : ApiController
    {
        IProductRepository _repository;

        public Products2Controller(IProductRepository repository)
        {
            _repository = repository;
        }

        public IHttpActionResult Get(int id)
        {
            Product product = _repository.GetById(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        public IHttpActionResult Post(Product product)
        {
            _repository.AddProduct(product);
            return CreatedAtRoute("DefaultApi", new { id = product.Id }, product);
        }

        public IHttpActionResult Delete(int id)
        {
            _repository.Delete(id);
            return Ok();
        }

        public IHttpActionResult Put(Product product)
        {
            // Do some work (not shown).
            return Content(HttpStatusCode.Accepted, product);
        }
    }
}
