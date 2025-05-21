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
        public IActionResult GetAllCategory()
        {
            string? errorMsg, successMsg;

            var result = GetAllCategory(out successMsg, out errorMsg);
            if (result is not null)
            {
                return Ok(new { data = result, message = successMsg });
            }

            return NotFound(new { message = errorMsg });
        }

        [HttpPost("addCategory")]
        public IActionResult AddCategory([FromBody] CategoryInputModel input)
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
    }
}
