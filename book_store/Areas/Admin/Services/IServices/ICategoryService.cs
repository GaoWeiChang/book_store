using book_store.Models;

namespace book_store.Areas.Admin.Services.IServices
{
    public interface ICategoryService
    {
        public bool CreateCategory(Category category);
        IEnumerable<Category> GetAllCategories();
        public Category GetCategoryById(int id);
        public bool UpdateCategory(Category category);
        public bool DeleteCategory(int id);

    }
}
