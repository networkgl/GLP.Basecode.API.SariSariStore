using System;
using System.Collections.Generic;

namespace GLP.Basecode.API.SariSariStoreProduct.Models;

public partial class VwGetProductBy
{
    public long CategoryId { get; set; }

    public string Barcode { get; set; } = null!;

    public string CategoryName { get; set; } = null!;

    public string ProductName { get; set; } = null!;

    public decimal Price { get; set; }
}
