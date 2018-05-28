using MedicalCorporateWebPortal.AppData;
using MedicalCorporateWebPortal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MedicalCorporateWebPortal.Controllers
{
    public class ScheduleController : Controller
    {
        private readonly MedicCroporateContext _context;

        public ScheduleController(MedicCroporateContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Schedules(int doctorId, int serviceId)
        {
            var doctor = _context.Doctors.SingleOrDefault(d => d.EmployeeID == doctorId);
            var employee = _context.Employees.SingleOrDefault(e => e.EmployeeID == doctor.EmployeeID);
            var user = _context.Users.SingleOrDefault(u => u.UserID == employee.UserID);
            var appointmentDate = _context.AppointmentDates.Where(date => date.DoctorID == doctor.EmployeeID);
            var ReservedTime = _context.AppointmentTimes.Where(time => appointmentDate.Any(date => date.DateOfAppointmentID == time.DateOfAppointmentID));
            var providedServices = _context.ProvideServices.Where(service => service.DoctorID == doctor.ID);
            List<Service> services = null;
            ServiceListViewModel mv = new ServiceListViewModel();
            mv.Services = _context.Services.Where(service => providedServices.Any(ps => ps.ServiceID == service.ServiceID)).ToList();
            mv.ServiceItems = new List<SelectListItem>();
            for (int i = 0; i < mv.Services.Count; i++)
            {
                mv.ServiceItems.Add(new SelectListItem { Text = mv.Services[i].Name, Value = mv.Services[i].ServiceID.ToString() });
            }
            if (providedServices.Any())
            {
                services = _context.Services.Where(service => providedServices.Any(ps => ps.ServiceID == service.ServiceID)).ToList();
            }
            List<SelectListItem> items = new List<SelectListItem>();
            if (services != null)
            {
                foreach (var service in services)
                {
                    items.Add(new SelectListItem { Text = service.Name, Value = service.ServiceID.ToString() });
                }
            }
            var tuple = new Tuple<Doctor, IEnumerable<DateOfAppointment>, IEnumerable<ReservedTime>, ServiceListViewModel, User>(doctor, appointmentDate, ReservedTime, mv, user);
            return View(tuple);
        }
    }
}