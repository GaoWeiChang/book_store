using book_store.Areas.Admin.Services.IServices;
using book_store.DataAccess.Repository.IRepository;
using book_store.Models;

namespace book_store.Areas.Admin.Services
{
    public class CategoryService: ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public bool CreateCategory(Category category)
        {
            try{
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
    }
}
