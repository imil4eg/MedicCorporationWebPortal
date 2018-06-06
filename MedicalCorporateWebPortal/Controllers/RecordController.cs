using MedicalCorporateWebPortal.AppData;
using MedicalCorporateWebPortal.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalCorporateWebPortal.Controllers
{
    public class RecordController : Controller
    {
        private readonly MedicCroporateContext _context;

        protected UserManager<User> _userManager;

        protected RoleManager<ApplicationRole> _roleManager;

        public RecordController(MedicCroporateContext context, UserManager<User> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        
        //[HttpPost]
        //public IActionResult RecordConfirmation(int doctorId, DateTime date, ServiceListViewModel vm)
        //{
        //    int serviceId = vm.SelectedServiceID;
        //    var doctor = _context.Doctors.SingleOrDefault(d => d.ID == doctorId);
        //    var employee = _context.Employees.SingleOrDefault(e => e.EmployeeID == doctor.EmployeeID);
        //    var user = _context.Users.SingleOrDefault(u => u.Id == employee.UserID);
        //    var service = _context.Services.SingleOrDefault(s => s.ServiceID == serviceId);
        //    var appointmentDate = _context.AppointmentDates.SingleOrDefault(d => d.Date.Equals(date));

        //    return View(new Tuple<Doctor, Service, DateOfAppointment, string, User>(doctor, service, appointmentDate, date.Date.TimeOfDay.ToString("HH:mm"), user));
        //}

        [HttpGet]
        public async Task<IActionResult> RecordConfirmation(DoctorViewModel model, DateTime selectedTime)
        {
            var doctorUser = await _userManager.FindByNameAsync(model.User.UserName);
            var service = await _context.Services.FindAsync(int.Parse(model.SelectedService));
            var doctor = await _context.Doctors.FindAsync(model.Doctor.ID);
            var viewmodel = new RecordConfirmViewModel
            {
                DoctorID = doctor.ID,
                DoctorLastName = doctorUser.LastName,
                DoctorFirstName = doctorUser.FirstName,
                DoctorSpeciality = _context.Specialties.FirstOrDefault(s => s.ID == doctor.SpecialtyID).Name,
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
                    var modelDoctor = await _context.Doctors.FindAsync(model.DoctorID);
                    var modelEmployee = await _context.Employees.FindAsync(modelDoctor.EmployeeID);
                    var modelUser = await _context.Users.FindAsync(modelEmployee.UserID);
                    var modelService = await _context.Services.FindAsync(model.ServiceID);
                    model.DoctorID = modelDoctor.ID;
                    model.DoctorLastName = modelUser.LastName;
                    model.DoctorFirstName = modelUser.FirstName;
                    model.DoctorSpeciality = _context.Specialties.FirstOrDefault(s => s.ID == modelDoctor.SpecialtyID).Name;
                    model.ServiceID = modelService.ServiceID;
                    model.ServiceName = modelService.Name;
                    model.ServiceCost = modelService.Price;
                    return View(model);
                }
            }

            Patient patient = null;
            var date = _context.AppointmentDates.SingleOrDefault(d => d.DoctorID == model.DoctorID && d.Date.Date == model.Date.Date);
            User user = null;
            if (User.Identity.IsAuthenticated)
            {
                user = await _userManager.GetUserAsync(HttpContext.User);
                if (User.IsInRole(UserRole.Пациент.ToString()))
                {
                    patient = await _context.Patients.FindAsync(user.Id);

                }
                else
                {
                    ViewBag.Message = "Только пациенты могут записываться на прием";
                }
            }
            else
            {
                var userName = Guid.NewGuid().ToString();
                user = new User
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
                        User = user
                    };

                    await _context.AddAsync(patient);
                    await _context.SaveChangesAsync();
                    patient = await _context.Patients.FindAsync(user.Id);
                }
            }


            var reserve = new ReservedTime
            {
                DateOfAppointmentID = date.DateOfAppointmentID,
                DateOfAppointment = date,
                User = user,
                UserID = user.Id,
                Time = model.Date,
                ServiceID = model.ServiceID
            };

            await _context.AppoitmentReservedTime.AddAsync(reserve);
            await _context.SaveChangesAsync();

            ViewBag.Message = "Пользователь успешно записан";
            return View("Info");
        }
    }
}