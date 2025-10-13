using book_store.Areas.Admin.Services.IServices;
using book_store.DataAccess.Repository.IRepository;
using book_store.Models;
using book_store.Utility;

namespace book_store.Areas.Admin.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ServiceResult CreateCategory(Category category)
        {
            // check duplicated name
            var existingCategory = _unitOfWork.Category.Get(c => c.Name.ToLower() == category.Name.ToLower());
            if (existingCategory != null)
            {
                return ServiceResult.Fail("This category name already exist.");
            }

            // check duplicated display order
            var existingDisplayOrder = _unitOfWork.Category.Get(c => c.DisplayOrder == category.DisplayOrder);
            if (existingDisplayOrder != null)
            {
                return ServiceResult.Fail("This display order already exist.");
            }

            if (category.DisplayOrder < 0)
            {
                return ServiceResult.Fail("Display order must be positive number.");
            }

            try
            {
                _unitOfWork.Category.Add(category);
                _unitOfWork.Save();
                return ServiceResult.Ok("Created category successfully.");
            }
            catch (Exception ex) {
                return ServiceResult.Fail($"Fail to create category: {ex.Message}");
            }
        }

        public IEnumerable<Category> GetAllCategories()
        {
            return _unitOfWork.Category.GetAll().OrderBy(c => c.DisplayOrder);
        }

        public ServiceResult<Category> GetCategoryById(int id)
        {
            if (id < 0) return ServiceResult<Category>.Fail("id must be positive number");

            try
            {
                var category = _unitOfWork.Category.Get(c => c.Id == id);
                if (category == null) return ServiceResult<Category>.Fail("id not found");

                return ServiceResult<Category>.Ok(category, "Success to get category");
            }
            catch (Exception ex)
            {
                return ServiceResult<Category>.Fail($"Fail to retrive category: {ex.Message}");
            }
        }

        public ServiceResult UpdateCategory(Category category)
        {
            if (category.DisplayOrder < 0)
            {
                return ServiceResult.Fail("Display order must be positive number.");
            }

            // check duplicated name
            var existingCategory = _unitOfWork.Category.Get(c => c.Name.ToLower() == category.Name.ToLower());
            if (existingCategory != null)
            {
                return ServiceResult.Fail("This category already exist.");
            }

            // check duplicated display order
            var existingDisplayOrder = _unitOfWork.Category.Get(c => c.DisplayOrder == category.DisplayOrder);
            if (existingDisplayOrder != null)
            {
                return ServiceResult.Fail("This display order already exist.");
            }

            try
            {
                var ctgr = _unitOfWork.Category.Get(c => c.Id == category.Id, tracked: true);
                if (ctgr == null) return ServiceResult.Fail($"Category Id {category.Id} not found.");

                ctgr.Name = category.Name;
                ctgr.DisplayOrder = category.DisplayOrder;

                _unitOfWork.Category.Update(ctgr);
                _unitOfWork.Save();
                return ServiceResult.Ok("Update category successfully.");
            }
            catch (Exception ex)
            {
                return ServiceResult.Fail($"Fail to update category: {ex.Message}");
            }
        }

        public ServiceResult DeleteCategory(int id)
        {
            try
            {
                var category = _unitOfWork.Category.Get(c => c.Id == id, tracked: true);
                if (category == null) return ServiceResult.Fail($"Category Id {id} not found.");

                _unitOfWork.Category.Remove(category);
                _unitOfWork.Save();
                return ServiceResult.Ok("Delete category successfully.");
            }
            catch (Exception ex)
            {
                return ServiceResult.Fail($"Fail to delete category: {ex.Message}");
            }
        } 
    }
}
