using MedicalCorporateWebPortal.AppData;
using MedicalCorporateWebPortal.Models;
using Microsoft.EntityFrameworkCore;

namespace MedicalCorporateWebPortal.Repository
{
    public class AppointmentRepository : Repository<Appointment>, IAppointmentRepository
    {
        public AppointmentRepository(MedicCroporateContext context) : base(context)
        {

        }
    }
}
