using GLP.Basecode.API.SariSariStoreProduct.Repository;
using Microsoft.AspNetCore.Mvc;
using GLP.Basecode.API.SariSariStoreProduct.Models;

namespace GLP.Basecode.API.SariSariStoreProduct.Controllers
{
    public class BaseController : ControllerBase
    {
        public MaribethStoreDbContext _db = new MaribethStoreDbContext();
        public Db20127Context _db1 = new Db20127Context();
        
        public BaseRepository<Product> _productRepo;
        public BaseRepository<Category> _categoryRepo;

        public BaseController()
        {
            _productRepo = new BaseRepository<Product>();
            _categoryRepo = new BaseRepository<Category>();
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
