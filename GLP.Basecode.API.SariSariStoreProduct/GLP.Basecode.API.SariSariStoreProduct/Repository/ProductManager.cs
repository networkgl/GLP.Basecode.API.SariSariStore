using GLP.Basecode.API.SariSariStoreProduct.Controllers;
using GLP.Basecode.API.SariSariStoreProduct.Models;
using GLP.Basecode.API.SariSariStoreProduct.Constant;
using GLP.Basecode.API.SariSariStoreProduct.Models.CustomModel;

namespace GLP.Basecode.API.SariSariStoreProduct.Repository
{
    public class ProductManager : BaseController
    {
        protected List<Product>? GetAll(out string? successMsg, out string? errorMsg)
        {
            successMsg = errorMsg = null;

            try
            {
                var retVal = _productRepo.GetAll();

                if (retVal is null)
                {
                    errorMsg = "No products available.";
                    return null;
                }
                else
                    successMsg = "Product list successfully retrieved.";


                return retVal;
            }
            catch (Exception e)
            {
                errorMsg = GetInnermostExceptionMessage(e);
                return null;
            }
        }

        protected Product? GetProductById(string code, out string? successMsg, out string? errorMsg)
        {
            successMsg = errorMsg = null;

            try
            {
                var result = _productRepo.Table.Where(m => m.Barcode == code).SingleOrDefault();

                if (result is null) 
                {
                    errorMsg = $"Product with barcode {code} does not exist.";
                    return null;
                }


                successMsg = "Product details successfully retrieved";
                return result;
            }
            catch (Exception e)
            {
                errorMsg = GetInnermostExceptionMessage(e);
                return null;
            }
        }

        private bool IsProductExisted(string barcode, string productName, out string? errorMsg)
        {
            errorMsg = null;

            try
            {
                var exists = _productRepo.GetAll().Any(c =>
                             c.Barcode == barcode ||
                             c.ProductName == productName         
                         );

                if (exists)
                    errorMsg = "The same product name or barcode already added.";

                return exists;
            }
            catch (Exception e)
            {
                errorMsg = GetInnermostExceptionMessage(e);
                return true; //to prevent inserting when error
            }
        }

        protected ErrorCode Add(ProductInputModel model, out string? successMsg, out string? errorMsg)
        {
            successMsg = errorMsg = null;

            try
            {
                //check if existed
                if (IsProductExisted(model.Barcode, model.ProductName, out errorMsg))
                    return ErrorCode.Error;

                var newProduct = new Product()
                {
                    Barcode = model.Barcode,
                    ProductName = model.ProductName,
                    Price = model.Price,
                    CategoryId = model.CategoryId
                };

                var result = _productRepo.Create(newProduct, out successMsg, out errorMsg);
                if (result == ErrorCode.Error)
                    return ErrorCode.Error;
                
                successMsg = "Product details successfully added.";
                return ErrorCode.Success;
            }
            catch (Exception e)
            {
                errorMsg = GetInnermostExceptionMessage(e);
                return ErrorCode.Error;
            }
        }

        protected ErrorCode Update(long id, ProductInputModel model, out string? successMsg, out string? errorMsg)
        {
            successMsg = errorMsg = null;

            try
            {
                var updatedEntity = _productRepo.Get(id, out errorMsg);

                if (updatedEntity is null)
                {
                    errorMsg = $"Product with ID {id} does not exist or has already been deleted.";
                    return ErrorCode.NotFound;
                }

                // Manually clone the original before modifying it to reduce twice calling of DB
                var oldEntity = new Product()
                {
                    Barcode = updatedEntity.Barcode,
                    ProductName = updatedEntity.ProductName,
                    Price = updatedEntity.Price,
                    CategoryId = updatedEntity.CategoryId
                };

                updatedEntity.Barcode = model.Barcode;
                updatedEntity.ProductName = model.ProductName;
                updatedEntity.Price = model.Price;
                updatedEntity.CategoryId = model.CategoryId;
               

                var result = _productRepo.Update(oldEntity, updatedEntity, out successMsg, out errorMsg);
                if (result == ErrorCode.Error)
                    return ErrorCode.Error;

                successMsg = "Product details successfully updated";
                return ErrorCode.Success;
            }
            catch (Exception e)
            {
                errorMsg = GetInnermostExceptionMessage(e);
                return ErrorCode.Error;
            }
        }

        protected ErrorCode Delete(long id, out string? successMsg, out string? errorMsg)
        {
            successMsg = errorMsg = null;

            try
            {
                var entity = _productRepo.Get(id, out errorMsg);

                if (entity is null)
                {
                    errorMsg = $"Product with ID {id} does not exist or has already been deleted.";
                    return ErrorCode.NotFound;
                }

                var result = _productRepo.Delete(id, out successMsg, out errorMsg);
                if (result == ErrorCode.Error)
                    return ErrorCode.Error;

                successMsg = "Product details successfully deleted.";
                return ErrorCode.Success;
            }
            catch (Exception e)
            {
                errorMsg = GetInnermostExceptionMessage(e);
                return ErrorCode.Error;
            }
        }

    }
}
