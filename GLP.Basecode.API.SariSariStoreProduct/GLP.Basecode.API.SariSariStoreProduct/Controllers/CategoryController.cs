using GLP.Basecode.API.SariSariStoreProduct.Constant;
using GLP.Basecode.API.SariSariStoreProduct.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GLP.Basecode.API.SariSariStoreProduct.Models.CustomModel;

namespace GLP.Basecode.API.SariSariStoreProduct.Controllers
{
    [Route("api/category")]
    [ApiController]
    public class CategoryController : CategoryManager
    {
        [HttpGet("getAllCategory")]
        public async Task<IActionResult> GetAllCategory()
        {
            var result = await GetAll();

            switch (result.Status)
            {
                case ErrorCode.Success:
                    return Ok(new { data = result.Data, message = result.SuccessMessage });
                case ErrorCode.Error:
                    return BadRequest(new { message = result.ErrorMessage });
                case ErrorCode.NotFound:
                    return NotFound(new { message = result.ErrorMessage });
                default:
                    return StatusCode(500, new { message = "Unhandled error state." });
            }
        }

        [HttpPost("addCategory")]
        public async Task<IActionResult> AddCategory([FromBody] CategoryInputModel input)
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
    }
}
