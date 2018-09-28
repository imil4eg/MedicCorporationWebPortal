using MedicalCorporateWebPortal.Models;
using MedicalCorporateWebPortal.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalCorporateWebPortal.Controllers
{
    public class RecordController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        protected UserManager<ApplicationUser> _userManager;

        protected RoleManager<ApplicationRole> _roleManager;

        public RecordController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            this._unitOfWork = unitOfWork;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        
        //[HttpPost]
        //public IActionResult RecordConfirmation(int doctorId, DateTime date, ServiceListViewModel vm)
        //{
        //    int serviceId = vm.SelectedServiceID;
        //    var doctor = this._unitOfWork.Doctors.SingleOrDefault(d => d.ID == doctorId);
        //    var employee = this._unitOfWork.Employees.SingleOrDefault(e => e.EmployeeID == doctor.EmployeeID);
        //    var user = this._unitOfWork.Users.SingleOrDefault(u => u.Id == employee.UserID);
        //    var service = this._unitOfWork.Services.SingleOrDefault(s => s.ServiceID == serviceId);
        //    var appointmentDate = this._unitOfWork.AppointmentDates.SingleOrDefault(d => d.Date.Equals(date));

        //    return View(new Tuple<Doctor, Service, DateOfAppointment, string, ApplicationUser>(doctor, service, appointmentDate, date.Date.TimeOfDay.ToString("HH:mm"), user));
        //}

        [HttpGet]
        public async Task<IActionResult> RecordConfirmation(DoctorViewModel model, DateTime selectedTime)
        {
            var doctorUser = await _userManager.FindByNameAsync(model.ApplicationUser.UserName);
            var service = this._unitOfWork.Services.Get(int.Parse(model.SelectedService));
            var doctor = this._unitOfWork.Doctors.Get(model.Doctor.ID);
            var viewmodel = new RecordConfirmViewModel
            {
                DoctorID = doctor.ID,
                DoctorLastName = doctorUser.LastName,
                DoctorFirstName = doctorUser.FirstName,
                DoctorSpeciality = this._unitOfWork.Specialtys.Find(s => s.ID == doctor.SpecialtyID).FirstOrDefault().Name,
                ServiceID = service.ServiceID,
                ServiceName = service.Name,
                ServiceCost = service.Price,
                Date = selectedTime
            };

            return View(viewmodel);
        }

        [HttpPost]
        public async Task<IActionResult> RecordConfirmation(RecordConfirmViewModel model)
        {
            if (!User.Identity.IsAuthenticated)
            {
                if (!ModelState.IsValid)
                {
                    var modelDoctor = this._unitOfWork.Doctors.Get(model.DoctorID);
                    var modelEmployee = this._unitOfWork.Employees.Get(modelDoctor.EmployeeID);
                    var modelUser = this._unitOfWork.Users.Get(modelEmployee.UserID);
                    var modelService = this._unitOfWork.Services.Get(model.ServiceID);
                    model.DoctorID = modelDoctor.ID;
                    model.DoctorLastName = modelUser.LastName;
                    model.DoctorFirstName = modelUser.FirstName;
                    model.DoctorSpeciality = this._unitOfWork.Specialtys.Find(s => s.ID == modelDoctor.SpecialtyID).FirstOrDefault().Name;
                    model.ServiceID = modelService.ServiceID;
                    model.ServiceName = modelService.Name;
                    model.ServiceCost = modelService.Price;
                    return View(model);
                }
            }

            Patient patient = null;
            var date = this._unitOfWork.DatesOfAppointments.
                Find(d => d.DoctorID == model.DoctorID && d.Date.Date == model.Date.Date).FirstOrDefault();
            ApplicationUser user = null;
            if (User.Identity.IsAuthenticated)
            {
                user = await _userManager.GetUserAsync(HttpContext.User);
                if (User.IsInRole(UserRole.Пациент.ToString()))
                {
                    patient = this._unitOfWork.Patients.Get(user.Id);

                }
                else
                {
                    ViewBag.Message = "Только пациенты могут записываться на прием";
                }
            }
            else
            {
                var userName = Guid.NewGuid().ToString();
                user = new ApplicationUser
                {
                    UserName = userName,
                    Password = "",
                    LastName = model.LastName,
                    FirstName = model.FirstName,
                    PhoneNumber = model.Phone,
                    Email = model.Email,
                    Gender = Gender.Мужской,
                    Role = UserRole.Пациент,
                };

                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    user = await _userManager.FindByNameAsync(userName);
                    patient = new Patient
                    {
                        UserID = user.Id,
                        ApplicationUser = user
                    };

                    this._unitOfWork.Patients.Add(patient);
                    this._unitOfWork.Save();
                    patient = this._unitOfWork.Patients.Get(user.Id);
                }
            }


            var reserve = new ReservedTime
            {
                DateOfAppointmentID = date.DateOfAppointmentID,
                DateOfAppointment = date,
                ApplicationUser = user,
                UserID = user.Id,
                Time = model.Date,
                ServiceID = model.ServiceID
            };

            this._unitOfWork.ReservedTimes.Add(reserve);
            this._unitOfWork.Save();

            ViewBag.Message = "Пользователь успешно записан";
            return View("Info");
        }
    }
}