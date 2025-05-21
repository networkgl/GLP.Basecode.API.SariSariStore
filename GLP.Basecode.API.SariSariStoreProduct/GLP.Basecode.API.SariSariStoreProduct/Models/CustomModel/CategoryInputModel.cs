using System.ComponentModel.DataAnnotations;

namespace GLP.Basecode.API.SariSariStoreProduct.Models.CustomModel
{
    public class CategoryInputModel
    {
        [Required]
        public string CategoryName { get; set; } = null!;
    }
}
