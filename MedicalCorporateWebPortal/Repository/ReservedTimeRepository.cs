using MedicalCorporateWebPortal.AppData;
using MedicalCorporateWebPortal.Models;

namespace MedicalCorporateWebPortal.Repository
{
    public class ReservedTimeRepository : Repository<ReservedTime>, IReservedTimeRepository
    {
        public ReservedTimeRepository(MedicCroporateContext context) : base(context)
        {

        }
    }
}
