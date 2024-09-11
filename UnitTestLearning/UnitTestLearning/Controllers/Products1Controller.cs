using Microsoft.AspNetCore.Mvc;
using System.Net;
using UnitTestLearning.Models;
using UnitTestLearning.Repository;

namespace UnitTestLearning.Controllers
{
    public class Products1Controller : ControllerBase
    {
        IProductRepository _repository;

        public Products1Controller(IProductRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Get(int id)
        {
            Product product = _repository.GetById(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        public IActionResult Post(Product product)
        {
            _repository.AddProduct(product);

            //var response = Request.CreateResponse(HttpStatusCode.Created, product);
            //string uri = Url.Link("DefaultApi", new { id = product.Id });
            //response.Headers.Location = new Uri(uri);

            //var response1 = new HttpResponseMessage(HttpStatusCode.Created);
            //response1.RequestMessage(product);

            return StatusCode(StatusCodes.Status201Created,product);
        }

        public async Task<ActionResult<List<Product>>> GetAllProduct()
        {
            var products = _repository.GetAllProducts();

            return Ok(products);
        }
    }
}
