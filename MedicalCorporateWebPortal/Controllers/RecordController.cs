using MedicalCorporateWebPortal.AppData;
using MedicalCorporateWebPortal.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace MedicalCorporateWebPortal.Controllers
{
    public class RecordController : Controller
    {
        private readonly MedicCroporateContext _context;

        public RecordController(MedicCroporateContext context)
        {
            _context = context;
        }
        
        [HttpPost]
        public IActionResult RecordConfirmation(int doctorId, DateTime date, ServiceListViewModel vm)
        {
            int serviceId = vm.SelectedServiceID;
            var doctor = _context.Doctors.SingleOrDefault(d => d.ID == doctorId);
            var employee = _context.Employees.SingleOrDefault(e => e.EmployeeID == doctor.EmployeeID);
            var user = _context.Users.SingleOrDefault(u => u.UserID == employee.UserID);
            var service = _context.Services.SingleOrDefault(s => s.ServiceID == serviceId);
            var appointmentDate = _context.AppointmentDates.SingleOrDefault(d => d.Date.Equals(date));

            return View(new Tuple<Doctor, Service, DateOfAppointment, string, User>(doctor, service, appointmentDate, date.Date.TimeOfDay.ToString("HH:mm"), user));
        }
    }
}