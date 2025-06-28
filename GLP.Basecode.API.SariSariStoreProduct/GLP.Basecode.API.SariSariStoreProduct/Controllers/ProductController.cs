using GLP.Basecode.API.SariSariStoreProduct.Models.CustomModel;
using GLP.Basecode.API.SariSariStoreProduct.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GLP.Basecode.API.SariSariStoreProduct.Constant;
using Microsoft.AspNetCore.Authorization;


namespace GLP.Basecode.API.SariSariStoreProduct.Controllers
{
    [Route("api/product")]
    [ApiController]
    public class ProductController : ProductManager
    {
        [HttpGet("getAllProduct")]
        public async Task<IActionResult> GetAllProduct()
        {
            var result = await GetAll();

            switch (result.Status)
            {
                case ErrorCode.Success:
                    return Ok(new { data = result.Data, message = result.SuccessMessage });
                case ErrorCode.Error:
                    return BadRequest(new { message = result.ErrorMessage });
                case ErrorCode.NotFound:
                    return Ok(new { message = result.ErrorMessage });
                default:
                    return StatusCode(500, new { message = "Unhandled error state." });
            }
        }

        [HttpGet("getProductByBarcode/{code}")]
        public async Task<IActionResult> GetProductByBarcode(string code)
        {
            var result = await GetProductById(code);
            if (result.Status == ErrorCode.Success && result.Data is not null)
            {
                return Ok(new { data = result.Data, message = result.SuccessMessage });
            }

            return NotFound(new { message = result.ErrorMessage });
        }

        [HttpPost("addProduct")]
        public async Task<IActionResult> AddProduct([FromBody] ProductInputModel input)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await Add(input);

            switch (result.Status)
            {
                case ErrorCode.Success:
                    return Ok(new { message = result.SuccessMessage });
                case ErrorCode.Error:
                    return BadRequest(new { message = result.ErrorMessage });
                case ErrorCode.Duplicate:
                    return StatusCode(422, new { message = result.ErrorMessage }); //Unprocessable Entity
                default:
                    return StatusCode(500, new { message = "Unhandled error state." });
            }
        }

        [HttpPut("updateProduct/{id:long}")]
        public async Task<IActionResult> UpdateProduduct(long id,[FromBody] ProductUpdateModel input)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await Update(id, input);
            if (result.Status == ErrorCode.Success)
            {
                return Ok(new { message = result.SuccessMessage });
            }

            return NotFound(new { message = result.ErrorMessage });
        }

        [HttpDelete("deleteProduct/{id:long}")]
        public async Task<IActionResult> DeleteProduct(long id)
        {
            var result = await Delete(id);
            if (result.Status == ErrorCode.Success)
            {
                return Ok(new { message = result.SuccessMessage });
            }

            return NotFound(new { message = result.ErrorMessage });
        }

    }
}
