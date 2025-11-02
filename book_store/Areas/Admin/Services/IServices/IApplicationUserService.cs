using book_store.Models;
using book_store.Utility;

namespace book_store.Areas.Admin.Services.IServices
{
    public interface IApplicationUserService
    {
        public ServiceResult<ApplicationUser> getApplicationUser(string userId);
        public IEnumerable<ApplicationUser> getAllApplicationUser();
        public ServiceResult updateApplicationUser(ApplicationUser applicationUser);
    }
}
