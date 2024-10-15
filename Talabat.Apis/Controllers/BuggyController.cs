using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Apis.Errors;
using Talabat.Repository.Data;

namespace Talabat.Apis.Controllers
{
   
    public class BuggyController : BaseController
    {
        private readonly StoreContext _dbContext;

        public BuggyController(StoreContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("notfound")] // Get: api/Buggy/notfound
        public ActionResult GetNotFound()
        {
            var product = _dbContext.Products.Find(100);
            if (product is null)
                return NotFound(new ApiResponse(404));

            return Ok(product);
        }

        [HttpGet("servererror")] // Get: api/Buggy/servererror
        public ActionResult GetServerError()
        {
            var product = _dbContext.Products.Find(100);
            var productToReturn = product.ToString();
            return Ok(product);
        }

        [HttpGet("badrrequest")] // Get: api/Buggy/badrrequest
        public ActionResult GetBadrRequest()
        {
            return BadRequest(new ApiResponse(400));
        }
        [HttpGet("badrrequest/{id}")] // Get: api/Buggy/badrrequest/five
        public ActionResult GetBadrRequest(int id)
        {
            return Ok();
        }

    }
}
