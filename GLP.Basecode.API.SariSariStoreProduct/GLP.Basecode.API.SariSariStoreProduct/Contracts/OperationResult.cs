using GLP.Basecode.API.SariSariStoreProduct.Constant;

namespace GLP.Basecode.API.SariSariStoreProduct.Contracts
{
    public class OperationResult<T>
    {
        public T? Data { get; set; }
        public string? SuccessMessage { get; set; }
        public string? ErrorMessage { get; set; }
        public ErrorCode Status { get; set; } // Optional but recommended
    }


}
