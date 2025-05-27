using GLP.Basecode.API.SariSariStoreProduct.Controllers;
using GLP.Basecode.API.SariSariStoreProduct.Models;
using GLP.Basecode.API.SariSariStoreProduct.Constant;
using GLP.Basecode.API.SariSariStoreProduct.Models.CustomModel;
using GLP.Basecode.API.SariSariStoreProduct.Contracts;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GLP.Basecode.API.SariSariStoreProduct.Repository
{
    public class ProductManager : BaseController
    {
        protected async Task<OperationResult<List<VwGetProductBy>>> GetAll()
        {
            var opRes = new OperationResult<List<VwGetProductBy>>();

            try
            {
                //var retVal = await _productRepo.GetAll();

                using (var db = new Db20127Context())
                {
                    var retVal = await db.VwGetProductBies.ToListAsync();
                    if (retVal is null || retVal.Count == 0)
                    {
                        opRes.Status = ErrorCode.NotFound;
                        opRes.ErrorMessage = "No products available.";
                        opRes.Data = null;
                    }
                    else
                    {
                        opRes.Status = ErrorCode.Success;
                        opRes.SuccessMessage = "Product list successfully retrieved.";
                        opRes.Data = retVal;
                    }

                }

                return opRes;
            }
            catch (Exception e)
            {
                opRes.Status = ErrorCode.Error;
                opRes.ErrorMessage = GetInnermostExceptionMessage(e);
                opRes.Data = null;
                return opRes;
            }
        }

        protected async Task<OperationResult<VwGetProductBy?>> GetProductById(string code)
        {
            var opRes = new OperationResult<VwGetProductBy?>();

            try
            {
                using (var db = new Db20127Context())
                {
                    var result = await db.VwGetProductBies.Where(m => m.Barcode == code).SingleOrDefaultAsync();
                    if (result is null)
                    {
                        opRes.Status = ErrorCode.NotFound;
                        opRes.ErrorMessage = $"Product with barcode {code} does not exist.";
                        opRes.Data = null;
                    }

                    opRes.Status = ErrorCode.Success;
                    opRes.SuccessMessage = "Product details successfully retrieved";
                    opRes.Data = result;
                    return opRes;
                }    
            }
            catch (Exception e)
            {
                opRes.Status = ErrorCode.Error;
                opRes.ErrorMessage = GetInnermostExceptionMessage(e);
                opRes.Data = null;
                return opRes;
            }
        }

        private async Task<OperationResult<bool>> IsProductExisted(string barcode, string productName)
        {
            var opRes = new OperationResult<bool>();

            try
            {
                using (var db = new Db20127Context())
                {
                    var exists = await db.Products.AnyAsync(c => c.Barcode == barcode || c.ProductName == productName);
                    
                    if (exists)
                    {
                        opRes.Status = ErrorCode.Duplicate;
                        opRes.ErrorMessage = "The same product name or barcode already added.";
                        opRes.Data = true;
                        return opRes;
                    }

                    opRes.Data = false;
                    return opRes;
                }
            }
            catch (Exception e)
            {
                opRes.Status= ErrorCode.Error;
                opRes.ErrorMessage = GetInnermostExceptionMessage(e);
                opRes.Data = true;
                return opRes;
            }
        }

        protected async Task<OperationResult<ErrorCode>> Add(ProductInputModel model)
        {
            var opRes = new OperationResult<ErrorCode>();

            try
            {
                var objReturn = await IsProductExisted(model.Barcode, model.ProductName);
                //check if existed
                if (objReturn.Data)
                {
                    opRes.Status = objReturn.Status;
                    opRes.ErrorMessage = objReturn.ErrorMessage;
                    return opRes;
                }

                var newProduct = new Product()
                {
                    Barcode = model.Barcode,
                    ProductName = model.ProductName,
                    Price = model.Price,
                    CategoryId = model.CategoryId
                };

                var result = await _productRepo.Create(newProduct);
                if (result.Status == ErrorCode.Error)
                {
                    opRes.Status = result.Status;
                    opRes.ErrorMessage = result.ErrorMessage;
                    return opRes;
                }

                opRes.Status = result.Status;
                opRes.SuccessMessage = "Product details successfully added.";
                return opRes;
            }
            catch (Exception e)
            {
                opRes.Status = ErrorCode.Error;
                opRes.ErrorMessage = GetInnermostExceptionMessage(e);
                return opRes;
            }
        }

        protected async Task<OperationResult<ErrorCode>> Update(long id, ProductInputModel model)
        {
            var opRes = new OperationResult<ErrorCode>();

            try
            {
                var updatedEntity = await _productRepo.Get(id);

                if (updatedEntity.Data is null)
                {
                    opRes.Status = updatedEntity.Status;
                    opRes.ErrorMessage = $"Product with ID {id} does not exist or has already been deleted.";
                    return opRes;
                }

                // Manually clone the original before modifying it to reduce twice calling of DB
                var oldEntity = new Product()
                {
                    Barcode = updatedEntity.Data.Barcode,
                    ProductName = updatedEntity.Data.ProductName,
                    Price = updatedEntity.Data.Price,
                    CategoryId = updatedEntity.Data.CategoryId
                };

                updatedEntity.Data.Barcode = model.Barcode;
                updatedEntity.Data.ProductName = model.ProductName;
                updatedEntity.Data.Price = model.Price;
                updatedEntity.Data.CategoryId = model.CategoryId;
               

                var result = await _productRepo.Update(oldEntity, updatedEntity.Data);
                if (result.Status == ErrorCode.Error)
                {
                    opRes.Status = result.Status;
                }

                opRes.Status = result.Status;
                opRes.SuccessMessage = "Product details successfully updated";
                return opRes;
            }
            catch (Exception e)
            {
                opRes.Status = ErrorCode.Error;
                opRes.ErrorMessage = GetInnermostExceptionMessage(e);
                return opRes;
            }
        }

        protected async Task<OperationResult<ErrorCode>> Delete(long id)
        {
            var opRes = new OperationResult<ErrorCode>();

            try
            {
                var entity = await _productRepo.Get(id);

                if (entity.Data is null)
                {
                    opRes.Status = entity.Status;
                    opRes.ErrorMessage = $"Product with ID {id} does not exist or has already been deleted.";
                    return opRes;
                }

                var result = await _productRepo.Delete(id);
                if (result.Status == ErrorCode.Error)
                {
                    opRes.Status = result.Status;
                    opRes.ErrorMessage = result.ErrorMessage;
                    return opRes;
                }

                opRes.Status = result.Status;
                opRes.SuccessMessage = "Product details successfully deleted.";
                return opRes;
            }
            catch (Exception e)
            {
                opRes.Status = ErrorCode.Error;
                opRes.ErrorMessage = GetInnermostExceptionMessage(e);
                return opRes;
            }
        }

    }
}
