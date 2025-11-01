using book_store.Models;

namespace book_store.Areas.Admin.Services.IServices
{
    public interface IApplicationUserService
    {
        public IEnumerable<ApplicationUser> getAllApplicationUser();
    }
}
