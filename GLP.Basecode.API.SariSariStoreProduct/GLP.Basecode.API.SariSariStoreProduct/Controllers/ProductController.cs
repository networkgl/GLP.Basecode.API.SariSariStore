using GLP.Basecode.API.SariSariStoreProduct.Models.CustomModel;
using GLP.Basecode.API.SariSariStoreProduct.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GLP.Basecode.API.SariSariStoreProduct.Constant;


namespace GLP.Basecode.API.SariSariStoreProduct.Controllers
{
    [Route("api/product")]
    [ApiController]
    public class ProductController : ProductManager
    {

        [HttpGet("getAllProduct")]
        public IActionResult GetAllProduct()
        {
            string? errorMsg, successMsg;

            var result = GetAll(out successMsg, out errorMsg);
            if (result is not null)
            {
                return Ok(new { data = result, message = successMsg });
            }

            return NotFound(new { message = errorMsg });
        }

        [HttpGet("getProductByBarcode/{id}")]
        public IActionResult GetProductByBarcode(string code)
        {
            string? errorMsg, successMsg;
            var result = GetProductById(code, out successMsg, out errorMsg);
            if (result is not null)
            {
                return Ok(new { data = result, message = successMsg });
            }

            return NotFound(new { message = errorMsg });
        }

        [HttpPost("addProduct")]
        public IActionResult AddProduct([FromBody] ProductInputModel input)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string? errorMsg, successMsg;

            var result = Add(input, out successMsg, out errorMsg);
            if (result == ErrorCode.Success)
            {
                return Ok(new { message = successMsg });
            }

            return NotFound(new { message = errorMsg });
        }

        [HttpPut("updateProduct/{id:long}")]
        public IActionResult UpdateProduduct(long id,[FromBody] ProductInputModel input)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string? errorMsg, successMsg;

            var result = Update(id, input, out successMsg, out errorMsg);
            if (result == ErrorCode.Success)
            {
                return Ok(new { message = successMsg });
            }

            return NotFound(new { message = errorMsg });
        }

        [HttpDelete("deleteProduct{id:long}")]
        public IActionResult DeleteProduct(long id)
        {
            string? errorMsg, successMsg;

            var result = Delete(id, out successMsg, out errorMsg);
            if (result == ErrorCode.Success)
            {
                return Ok(new { message = successMsg });
            }

            return NotFound(new { message = errorMsg });
        }

    }
}
