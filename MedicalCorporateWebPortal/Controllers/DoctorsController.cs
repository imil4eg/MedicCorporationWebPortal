using Microsoft.AspNetCore.Mvc;
using MedicalCorporateWebPortal.AppData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using Microsoft.AspNetCore.Mvc.Rendering;
using MedicalCorporateWebPortal.Models;

namespace MedicalCorporateWebPortal.Controllers
{
    public class DoctorsController : Controller
    {
        private readonly MedicCroporateContext _context;

        public DoctorsController(MedicCroporateContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            CultureInfo ci = new CultureInfo("ru-Ru");
            DateTimeFormatInfo dtfi = ci.DateTimeFormat;
            var users = _context.Users.Where(u => _context.Doctors.Any(d => d.EmployeeID == u.UserID));

            var tupleList = new List<Tuple<User, Doctor, IEnumerable<string>>>();
            foreach (var doctor in _context.Doctors)
            {
                List<string> AppointmentDates = new List<string>();
                var dates = _context.AppointmentDates.Where(date => users.Any(u => date.DoctorID == doctor.ID));
                if (dates.Count() > 0)
                {
                    foreach (var date in dates)
                    {
                        AppointmentDates.Add(dtfi.GetShortestDayName(date.Date.DayOfWeek));
                    }
                }

                tupleList.Add(new Tuple<User, Doctor, IEnumerable<string>>
                    (users.Single(u => u.UserID == doctor.EmployeeID), doctor, AppointmentDates));
            }
            return View(tupleList);
        }

        public IActionResult DoctorProfile(int id)
        {
            var doctor = _context.Doctors.SingleOrDefault(d => d.EmployeeID == id);
            var employeee = _context.Employees.SingleOrDefault(e => e.UserID == doctor.EmployeeID);
            var user = _context.Users.SingleOrDefault(u => u.UserID == employeee.UserID);
            var appoitmentDate = _context.AppointmentDates.Where(date => date.DoctorID == doctor.EmployeeID);
            var provideServeces = _context.ProvideServices.Where(ps => ps.DoctorID == doctor.EmployeeID);
            var tuple = new Tuple<User, Doctor, IEnumerable<DateOfAppointment>, IEnumerable<DoctorProvideService>>(user, doctor, appoitmentDate, provideServeces);

            return View(tuple);
        }

        public IActionResult Schedules(int doctorId, int serviceId)
        {
            var doctor = _context.Doctors.SingleOrDefault(d => d.EmployeeID == doctorId);
            var employee = _context.Employees.SingleOrDefault(e => e.EmployeeID == doctor.EmployeeID);
            var user = _context.Users.SingleOrDefault(u => u.UserID == employee.UserID);
            var appointmentDate = _context.AppointmentDates.Where(date => date.DoctorID == doctor.EmployeeID);
            var appointmentTime = _context.AppointmentTimes.Where(time => appointmentDate.Any(date => date.DateOfAppointmentID == time.DateOfAppointmentID));
            var providedServices = _context.ProvideServices.Where(service => service.DoctorID == doctor.ID);
            List<Service> services = null;
            ServiceListViewModel mv = new ServiceListViewModel();
            mv.Services = _context.Services.Where(service => providedServices.Any(ps => ps.ServiceID == service.ServiceID)).ToList();
            mv.ServiceItems = new List<SelectListItem>();
            for (int i = 0;i < mv.Services.Count; i++)
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
            var tuple = new Tuple<Doctor, IEnumerable<DateOfAppointment>, IEnumerable<AppointmentTime>, ServiceListViewModel, User>(doctor, appointmentDate, appointmentTime, mv, user);
            return View(tuple);
        }

        public IActionResult Services()
        {
            var services = _context.Services.Select(s => s);

            return View(services);
        }

        [HttpPost]
        public IActionResult RecordConfirmation(int doctorId, DateTime date)
        {
            int serviceId = 0;
            var doctor = _context.Doctors.SingleOrDefault(d => d.ID == doctorId);
            var employee = _context.Employees.SingleOrDefault(e => e.EmployeeID == doctor.EmployeeID);
            var user = _context.Users.SingleOrDefault(u => u.UserID == employee.UserID);
            var service = _context.Services.SingleOrDefault(s => s.ServiceID == serviceId);
            var appointmentDate = _context.AppointmentDates.SingleOrDefault(d => d.Date.Equals(date));

            return View(new Tuple<Doctor, Service, DateOfAppointment, string, User>(doctor, service, appointmentDate, date.Date.TimeOfDay.ToString("HH:mm"), user));
        }
    }
}