using GLP.Basecode.API.SariSariStoreProduct.Constant;

namespace GLP.Basecode.API.SariSariStoreProduct.Contracts
{
    public interface IBaseRepository<T> 
    {
        Task<OperationResult<T?>> Get(object id);
        Task<List<T>> GetAll();
        Task<OperationResult<ErrorCode>> Create(T entity);
        Task<OperationResult<ErrorCode>> Update(T oldEntity, T newEntity);
        Task<OperationResult<ErrorCode>> Delete(object id);
    }

}
