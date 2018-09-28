using MedicalCorporateWebPortal.AppData;
using MedicalCorporateWebPortal.Models;
using Microsoft.EntityFrameworkCore;

namespace MedicalCorporateWebPortal.Repository
{
    public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(MedicCroporateContext context) : base(context)
        {
        }
    }
}
