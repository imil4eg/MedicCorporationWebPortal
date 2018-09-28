using MedicalCorporateWebPortal.AppData;
using MedicalCorporateWebPortal.Models;

namespace MedicalCorporateWebPortal.Repository
{
    public class UserRepository : Repository<ApplicationUser>, IUserRepository
    {
        public UserRepository(MedicCroporateContext context) : base(context)
        {
        }
    }
}
