using book_store.Models;
using book_store.Utility;

namespace book_store.Areas.Admin.Services.IServices
{
    public interface ICategoryService
    {
        public ServiceResult CreateCategory(Category category);
        IEnumerable<Category> GetAllCategories();
        public ServiceResult<Category> GetCategoryById(int id);
        public ServiceResult UpdateCategory(Category category);
        public ServiceResult DeleteCategory(int id);

    }
}
