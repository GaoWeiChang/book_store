using book_store.Areas.Admin.Services.IServices;
using book_store.DataAccess.Repository.IRepository;
using book_store.Models;
using book_store.Utility;

namespace book_store.Areas.Admin.Services
{
    public class ApplicationUserService: IApplicationUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ApplicationUserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ServiceResult<ApplicationUser> getApplicationUser(string userId)
        {
            try
            {
                var user = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);
                if (user == null) return ServiceResult<ApplicationUser>.Fail("user not found");

                return ServiceResult<ApplicationUser>.Ok(user, "Success to get user");
            }
            catch (Exception ex)
            {
                return ServiceResult<ApplicationUser>.Fail($"Fail to retrive user: {ex.Message}");
            }
        }

        public ServiceResult updateApplicationUser(ApplicationUser applicationUser)
        {
            try
            {
                _unitOfWork.ApplicationUser.Update(applicationUser);
                _unitOfWork.Save();
                return ServiceResult.Ok("User updated successfully.");
            }
            catch (Exception ex)
            {
                return ServiceResult.Fail($"Fail to update user: {ex.Message}.");
            }
        }

        public IEnumerable<ApplicationUser> getAllApplicationUser()
        {
            return _unitOfWork.ApplicationUser.GetAll();
        }
    }
}
