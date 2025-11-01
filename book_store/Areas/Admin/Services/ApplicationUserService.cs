using book_store.Areas.Admin.Services.IServices;
using book_store.DataAccess.Repository.IRepository;
using book_store.Models;

namespace book_store.Areas.Admin.Services
{
    public class ApplicationUserService: IApplicationUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ApplicationUserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<ApplicationUser> getAllApplicationUser()
        {
            return _unitOfWork.ApplicationUser.GetAll();
        }
    }
}
