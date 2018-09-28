using MedicalCorporateWebPortal.AppData;
using MedicalCorporateWebPortal.Models;
using Microsoft.EntityFrameworkCore;

namespace MedicalCorporateWebPortal.Repository
{
    public class PatientRepository : Repository<Patient>, IPatientRepository
    {
        public PatientRepository(MedicCroporateContext context) : base(context)
        {

        }
    }
}
