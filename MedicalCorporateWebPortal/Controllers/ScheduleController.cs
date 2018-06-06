using MedicalCorporateWebPortal.AppData;
using MedicalCorporateWebPortal.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalCorporateWebPortal.Controllers
{
    public class ScheduleController : Controller
    {
        private readonly MedicCroporateContext _context;

        protected UserManager<User> _userManager;

        protected RoleManager<ApplicationRole> _roleManager;

        public ScheduleController(MedicCroporateContext context, UserManager<User> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public async Task<IActionResult> Schedules(int doctorId, int serviceId)
        {
            var doctor = await _context.Doctors.FindAsync(doctorId);
            var employee = await _context.Employees.FindAsync(doctor.EmployeeID);
            var user = await _context.Users.FindAsync(employee.UserID);
            var appointmentDate = _context.AppointmentDates.Where(date => date.DoctorID == doctor.ID);
            var ReservedTime = _context.AppoitmentReservedTime.Where(time => appointmentDate.Any(date => date.DateOfAppointmentID == time.DateOfAppointmentID));
            var providedServices = _context.ProvideServices.Where(service => service.DoctorID == doctor.ID);
            var services = _context.Services.Where(s => providedServices.Any(ps => ps.ServiceID == s.ServiceID));
            var specialty = await _context.Specialties.FindAsync(doctor.SpecialtyID);

            var model = new DoctorViewModel
            {
                User = user,
                Doctor = doctor,
                DatesOfAppointment = appointmentDate,
                ReservedTimes = ReservedTime,
                ProvideServices = providedServices,
                Services = services,
                SpecialtyName = specialty.Name
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> RecordConfirmation(DoctorViewModel model, DateTime selectedTime)
        {
            var user = await _userManager.FindByNameAsync(model.User.UserName);
            var service = await _context.Services.FindAsync(int.Parse(model.SelectedService));
            var doctor = await _context.Doctors.FindAsync(model.Doctor.ID);
            var specialty = await _context.Specialties.FindAsync(doctor.SpecialtyID);
            var viewmodel = new RecordConfirmViewModel
            {
                DoctorID = doctor.ID,
                DoctorLastName = user.LastName,
                DoctorFirstName = user.FirstName,
                DoctorSpeciality = specialty.Name,
                ServiceID = service.ServiceID,
                ServiceName = service.Name,
                ServiceCost = service.Price,
                Date = selectedTime,
            };

            return View("~/Views/Record/RecordConfirmation.cshtml", viewmodel);
        }
    }
}