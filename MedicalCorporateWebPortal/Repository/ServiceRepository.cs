using MedicalCorporateWebPortal.AppData;
using MedicalCorporateWebPortal.Models;

namespace MedicalCorporateWebPortal.Repository
{
    public class ServiceRepository : Repository<Service>, IServiceRepository
    {
        public ServiceRepository(MedicCroporateContext context) : base(context)
        {

        }
    }
}
