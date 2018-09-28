using MedicalCorporateWebPortal.AppData;
using MedicalCorporateWebPortal.Models;
using Microsoft.EntityFrameworkCore;

namespace MedicalCorporateWebPortal.Repository
{
    public class DoctorProvideServiceRepository : Repository<DoctorProvideService>, IDoctorProvideServiceRepository
    {
        public DoctorProvideServiceRepository(MedicCroporateContext context) : base(context)
        {

        }
    }
}
