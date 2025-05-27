using GLP.Basecode.API.SariSariStoreProduct.Controllers;
using GLP.Basecode.API.SariSariStoreProduct.Models;
using GLP.Basecode.API.SariSariStoreProduct.Constant;
using GLP.Basecode.API.SariSariStoreProduct.Models.CustomModel;
using GLP.Basecode.API.SariSariStoreProduct.Contracts;
using Microsoft.EntityFrameworkCore;

namespace GLP.Basecode.API.SariSariStoreProduct.Repository
{
    public class CategoryManager : BaseController
    {
        private async Task<OperationResult<bool>> IsCategoryExisted(string categoryName)
        {
            var opRes = new OperationResult<bool>();

            try
            {
                using (var db = new Db20515Context())
                {
                    var exists = await db.Categories.AnyAsync(c => c.CategoryName == categoryName);

                    if (exists)
                    {
                        opRes.Status = ErrorCode.Duplicate;
                        opRes.ErrorMessage = "The same category name or barcode already added.";
                        opRes.Data = true;
                        return opRes;
                    }

                    opRes.Data = false;
                    return opRes;
                }
            }
            catch (Exception e)
            {
                opRes.Status = ErrorCode.Error;
                opRes.ErrorMessage = GetInnermostExceptionMessage(e);
                opRes.Data = true;
                return opRes;
            }
        }

        protected async Task<OperationResult<List<Category>?>> GetAll()
        {
            var opRes = new OperationResult<List<Category>?>();

            try
            {
                var retVal = await _categoryRepo.GetAll();

                if (retVal is null || retVal.Count == 0)
                {
                    opRes.Status = ErrorCode.NotFound;
                    opRes.ErrorMessage = "No categories available.";
                    opRes.Data = null;
                    return opRes;
                }
                else
                {
                    opRes.Status = ErrorCode.Success;
                    opRes.SuccessMessage = "Category list successfully retrieved.";
                    opRes.Data = retVal;
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

        protected async Task<OperationResult<ErrorCode>> Add(CategoryInputModel model)
        {
            var opRes = new OperationResult<ErrorCode>();

            try
            {
                //check if existed
                var objReturn = await IsCategoryExisted(model.CategoryName);
                if (objReturn.Data)
                {
                    opRes.Status = objReturn.Status;
                    opRes.ErrorMessage = objReturn.ErrorMessage;
                    return opRes;
                }

                var newCategory = new Category()
                {
                    CategoryName = model.CategoryName,
                };

                var result = await _categoryRepo.Create(newCategory);
                if (result.Status == ErrorCode.Error)
                {
                    opRes.Status = objReturn.Status;
                    opRes.ErrorMessage = objReturn.ErrorMessage;
                    return opRes;
                }

                opRes.Status = result.Status;
                opRes.SuccessMessage = "Category details successfully added.";
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
