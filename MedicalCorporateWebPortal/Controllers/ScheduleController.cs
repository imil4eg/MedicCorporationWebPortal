using MedicalCorporateWebPortal.AppData;
using MedicalCorporateWebPortal.Models;
using MedicalCorporateWebPortal.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalCorporateWebPortal.Controllers
{
    public class ScheduleController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        protected UserManager<ApplicationUser> _userManager;

        protected RoleManager<ApplicationRole> _roleManager;

        public ScheduleController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            this._unitOfWork = unitOfWork;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult Schedules(int doctorId, int serviceId)
        {
            var doctor = this._unitOfWork.Doctors.Get(doctorId);
            var employee = this._unitOfWork.Employees.Get(doctor.EmployeeID);
            var user = this._unitOfWork.Users.Get(employee.UserID);
            var appointmentDate = this._unitOfWork.DatesOfAppointments.Find(date => date.DoctorID == doctor.ID);
            var ReservedTime = this._unitOfWork.ReservedTimes
                .Find(time => appointmentDate.Any(date => date.DateOfAppointmentID == time.DateOfAppointmentID));
            var providedServices = this._unitOfWork
                .DoctorProvideServices
                .Find(service => service.DoctorID == doctor.ID);
            var services = this._unitOfWork.Services
                .Find(s => providedServices.Any(ps => ps.ServiceID == s.ServiceID));
            var specialty = this._unitOfWork.Specialtys
                .Get(doctor.SpecialtyID);

            var model = new DoctorViewModel
            {
                ApplicationUser = user,
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
            var user = await _userManager.FindByNameAsync(model.ApplicationUser.UserName);
            var service = this._unitOfWork.Services.Get(int.Parse(model.SelectedService));
            var doctor = this._unitOfWork.Doctors.Get(model.Doctor.ID);
            var specialty = this._unitOfWork.Specialtys.Get(doctor.SpecialtyID);
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