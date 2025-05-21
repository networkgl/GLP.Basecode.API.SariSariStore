using System;
using System.Collections.Generic;

namespace GLP.Basecode.API.SariSariStoreProduct.Models;

public partial class Product
{
    public long ProductId { get; set; }

    public string Barcode { get; set; } = null!;

    public string ProductName { get; set; } = null!;

    public decimal Price { get; set; }

    public long CategoryId { get; set; }

    public virtual Category Category { get; set; } = null!;
}
