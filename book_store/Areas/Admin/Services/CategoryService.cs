using book_store.Areas.Admin.Services.IServices;
using book_store.DataAccess.Repository.IRepository;
using book_store.Models;

namespace book_store.Areas.Admin.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public bool CreateCategory(Category category)
        {
            try {
                _unitOfWork.Category.Add(category);
                _unitOfWork.Save();
                return true;
            }
            catch (Exception ex) {
                return false;
            }
        }

        public IEnumerable<Category> GetAllCategories()
        {
            return _unitOfWork.Category.GetAll().OrderBy(c => c.DisplayOrder);
        }

        public Category GetCategoryById(int id)
        {
            return _unitOfWork.Category.Get(c => c.Id == id);
        }

        public bool UpdateCategory(Category category)
        {
            Category ctgr = _unitOfWork.Category.Get(c => c.Id == category.Id, tracked: true);
            if (ctgr == null) return false;

            try
            {
                _unitOfWork.Category.Update(ctgr);
                _unitOfWork.Save();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool DeleteCategory(int id)
        {
            Category category = _unitOfWork.Category.Get(c => c.Id == id);
            if (category == null) return false;

            try
            {
                _unitOfWork.Category.Remove(category);
                _unitOfWork.Save();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        } 
    }
}
