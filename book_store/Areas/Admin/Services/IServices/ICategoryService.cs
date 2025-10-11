using book_store.Models;

namespace book_store.Areas.Admin.Services.IServices
{
    public interface ICategoryService
    {
        public bool CreateCategory(Category category);
        IEnumerable<Category> GetAllCategories();
    }
}
