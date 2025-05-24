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

        public async Task<OperationResult<T?>> Get(object id)
        {
            var opRes = new OperationResult<T?>();
            try
            {
                var entity = await _table.FindAsync(id);

                if (entity == null)
                {
                    opRes.Status = ErrorCode.NotFound;
                    opRes.ErrorMessage = $"No entity found for ID: {id}";
                    opRes.Data = null;
                    return opRes;
                }

                opRes.Status = ErrorCode.Success;
                opRes.SuccessMessage = "Entity successfully found.";
                opRes.Data = entity;
                return opRes;
            }
            catch (Exception ex)
            {
                opRes.ErrorMessage = GetInnermostExceptionMessage(ex);
                opRes.Data = null;
                return opRes;
            }
        }

        public async Task<List<T>> GetAll()
        {
            return await _table.ToListAsync();
        }

        public async Task<OperationResult<ErrorCode>> Create(T t)
        {
            var opRes = new OperationResult<ErrorCode>();
            try
            {
                await _table.AddAsync(t);
                await _db.SaveChangesAsync();
                opRes.Status = ErrorCode.Success;
                opRes.SuccessMessage = "Success";

                return opRes;
            }
            catch (Exception e)
            {
                opRes.Status = ErrorCode.Error;
                opRes.ErrorMessage = GetInnermostExceptionMessage(e);
                return opRes;
            }

        }

        public async Task<OperationResult<ErrorCode>> Update(T oldEntity, T updatedEntity)
        {
            var opRes = new OperationResult<ErrorCode>();
            try
            {
                _db.Entry(oldEntity).CurrentValues.SetValues(updatedEntity);
                await _db.SaveChangesAsync();
                opRes.Status = ErrorCode.Success;
                opRes.SuccessMessage = "Updated";

                return opRes;
            }
            catch (Exception e)
            {
                opRes.Status = ErrorCode.Error;
                opRes.ErrorMessage = GetInnermostExceptionMessage(e);
                return opRes;
            }
        }

        public async Task<OperationResult<ErrorCode>> Delete(object id)
        {
            var opRes = new OperationResult<ErrorCode>();
            try
            {
                var obj = await _table.FindAsync(id);
                if (obj is null)
                {
                    opRes.Status = ErrorCode.NotFound;
                    opRes.ErrorMessage = $"Entity with ID {id} does not exist or has already been deleted.";
                    return opRes;
                }

                _table.Remove(obj);
                await _db.SaveChangesAsync();
                opRes.Status = ErrorCode.Success;
                opRes.SuccessMessage = "Deleted";

                return opRes;
            }
            catch (Exception e)
            {
                opRes.Status= ErrorCode.Error;
                opRes.ErrorMessage = GetInnermostExceptionMessage(e);
                return opRes;
            }
        }

        private string? GetInnermostExceptionMessage(Exception ex)
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
