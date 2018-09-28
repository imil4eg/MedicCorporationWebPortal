using MedicalCorporateWebPortal.AppData;
using MedicalCorporateWebPortal.Models;
using Microsoft.EntityFrameworkCore;

namespace MedicalCorporateWebPortal.Repository
{
    public class DateOfAppointmentRepository : Repository<DateOfAppointment>, IDateOfAppointmentRepository
    {
        public DateOfAppointmentRepository(MedicCroporateContext context) : base(context)
        {

        }
    }
}
