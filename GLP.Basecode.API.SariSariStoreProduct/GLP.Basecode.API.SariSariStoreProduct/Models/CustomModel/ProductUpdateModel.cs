using System.ComponentModel.DataAnnotations;

namespace GLP.Basecode.API.SariSariStoreProduct.Models.CustomModel
{
    public class ProductUpdateModel
    {
        [Required]
        public string Barcode { get; set; } = null!;

        [Required]
        public string ProductName { get; set; } = null!;

        [Required]
        public decimal Price { get; set; }

    }
}
