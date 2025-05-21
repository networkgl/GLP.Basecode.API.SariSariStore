using GLP.Basecode.API.SariSariStoreProduct.Constant;
using GLP.Basecode.API.SariSariStoreProduct.Contracts;
using Microsoft.EntityFrameworkCore;

namespace GLP.Basecode.API.SariSariStoreProduct.Repository
{
    public class BaseRepository<T> : IBaseRepository<T>
         where T : class
    {
        private DbContext _db;
        private DbSet<T> _table;

        public DbSet<T> Table => _table;

        public BaseRepository()
        {
            _db = new MaribethStoreDbContext();
            _table = _db.Set<T>();
        }

        public T? Get(object id, out string? returnMsg)
        {
            returnMsg = string.Empty;

            try
            {
                var entity = _table.Find(id);

                if (entity == null)
                {
                    returnMsg = $"No entity found for ID: {id}";
                    return null;
                }

                return entity;
            }
            catch (Exception ex)
            {
                returnMsg = GetInnermostExceptionMessage(ex);
                return null;
            }
        }


        public List<T> GetAll()
        {
            return _table.ToList();
        }

        public ErrorCode Create(T t, out string successMsg, out string? errorMsg)
        {
            errorMsg = successMsg = string.Empty;

            try
            {
                _table.Add(t);
                _db.SaveChanges();
                successMsg = "Success";

                return ErrorCode.Success;
            }
            catch (Exception e)
            {
                errorMsg = GetInnermostExceptionMessage(e);
                return ErrorCode.Error;
            }

        }

        public ErrorCode Update(T oldEntity, T updatedEntity, out string successMsg, out string? errorMsg)
        {
            errorMsg = successMsg = string.Empty;
            try
            {
                _db.Entry(oldEntity).CurrentValues.SetValues(updatedEntity);
                _db.SaveChanges();
                successMsg = "Updated";

                return ErrorCode.Success;
            }
            catch (Exception e)
            {
                errorMsg = GetInnermostExceptionMessage(e);
                return ErrorCode.Error;
            }
        }

        public ErrorCode Delete(object id, out string successMsg, out string? errorMsg)
        {
            errorMsg = successMsg = string.Empty;

            try
            {
                var obj = _table.Find(id);
                if (obj is null)
                    return ErrorCode.NotFound;

                _table.Remove(obj);
                _db.SaveChanges();

                successMsg = "Deleted";

                return ErrorCode.Success;
            }
            catch (Exception e)
            {
                errorMsg = GetInnermostExceptionMessage(e);
                return ErrorCode.Error;
            }
        }

        protected string? GetInnermostExceptionMessage(Exception ex)
        {
            if (ex == null) return null;

            while (ex.InnerException != null)
            {
                ex = ex.InnerException;
            }
            return ex.Message;
        }


    }

}
