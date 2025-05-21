using GLP.Basecode.API.SariSariStoreProduct.Constant;

namespace GLP.Basecode.API.SariSariStoreProduct.Contracts
{
    public interface IBaseRepository<T>
    {
        T? Get(object id, out String? returnMsg);
        List<T> GetAll();
        ErrorCode Create(T entity, out String successMsg, out String? errorMsg);
        ErrorCode Update(T oldEntity, T newEntity, out String successMsg, out String? errorMsg);
        ErrorCode Delete(object id, out String successMsg, out String? errorMsg);

    }
}
