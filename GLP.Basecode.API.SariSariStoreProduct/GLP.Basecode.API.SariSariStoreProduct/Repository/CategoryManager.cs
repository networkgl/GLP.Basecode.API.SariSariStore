using GLP.Basecode.API.SariSariStoreProduct.Controllers;
using GLP.Basecode.API.SariSariStoreProduct.Models;
using GLP.Basecode.API.SariSariStoreProduct.Constant;
using GLP.Basecode.API.SariSariStoreProduct.Models.CustomModel;

namespace GLP.Basecode.API.SariSariStoreProduct.Repository
{
    public class CategoryManager : BaseController
    {
        private bool IsCategoryExisted(string categoryName, out string? errorMsg)
        {
            errorMsg = null;

            try
            {
                var exists = _categoryRepo.GetAll().Any(c =>
                             c.CategoryName == categoryName
                         );

                if (exists)
                    errorMsg = "The same category name already added.";

                return exists;
            }
            catch (Exception e)
            {
                errorMsg = GetInnermostExceptionMessage(e);
                return true; //to prevent inserting when error
            }
        }

        protected List<Category>? GetAllCategory(out string? successMsg, out string? errorMsg)
        {
            successMsg = errorMsg = null;

            try
            {
                var retVal = _categoryRepo.GetAll();

                if (retVal is null)
                {
                    errorMsg = "No categories available.";
                    return null;
                }
                else
                    successMsg = "Category list successfully retrieved.";


                return retVal;
            }
            catch (Exception e)
            {
                errorMsg = GetInnermostExceptionMessage(e);
                return null;
            }
        }

        protected ErrorCode Add(CategoryInputModel model, out string? successMsg, out string? errorMsg)
        {
            successMsg = errorMsg = null;

            try
            {
                //check if existed
                if (IsCategoryExisted(model.CategoryName, out errorMsg))
                    return ErrorCode.Error;

                var newCategory = new Category()
                {
                    CategoryName = model.CategoryName,
                };

                var result = _categoryRepo.Create(newCategory, out successMsg, out errorMsg);
                if (result == ErrorCode.Error)
                    return ErrorCode.Error;

                successMsg = "Category details successfully added.";
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
