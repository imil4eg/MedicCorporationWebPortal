using MedicalCorporateWebPortal.AppData;
using MedicalCorporateWebPortal.Models;

namespace MedicalCorporateWebPortal.Repository
{
    public class SpecialtyRepository : Repository<Specialty>, ISpecialtyRepository
    {
        public SpecialtyRepository(MedicCroporateContext context) : base(context)
        {

        }
    }
}
