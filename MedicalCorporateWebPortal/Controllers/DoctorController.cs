using Microsoft.AspNetCore.Mvc;
using MedicalCorporateWebPortal.AppData;
using System;
using System.Collections.Generic;
using System.Linq;
using MedicalCorporateWebPortal.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace MedicalCorporateWebPortal.Controllers
{
    public class DoctorController : Controller
    {
        private readonly MedicCroporateContext _context;

        protected UserManager<User> _userManager;

        protected RoleManager<ApplicationRole> _roleManager;

        public DoctorController(MedicCroporateContext context, UserManager<User> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public async Task<IActionResult> Doctors()
        {
            var doctors = _context.Doctors.Where(d => !d.IsDeleted);

            var models = new List<DoctorViewModel>();

            DateTime beginingOfWeek = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday);
            DateTime endOfWeek = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Friday);
            foreach (var doctor in doctors)
            {
                var employee = await _context.Employees.FindAsync(doctor.EmployeeID);
                var user = await _context.Users.FindAsync(employee.UserID);
                var dates = _context.AppointmentDates.Where(date => doctor.ID == date.DoctorID
                                                            && date.Date >= beginingOfWeek && date.Date <= endOfWeek);

                var specialty = await _context.Specialties.FindAsync(doctor.SpecialtyID);
                models.Add(new DoctorViewModel
                {
                    User = user,
                    Doctor = doctor,
                    DatesOfAppointment = dates.OrderBy(d => d.Date),
                    SpecialtyName = specialty.Name
                });
                //List<string> AppointmentDates = new List<string>();
                //var dates = _context.AppointmentDates.Where(date => users.Any(u => date.DoctorID == doctor.ID));
                //if (dates.Count() > 0)
                //{
                //    foreach (var date in dates)
                //    {
                //        AppointmentDates.Add(dtfi.GetShortestDayName(date.Date.DayOfWeek));
                //    }
                //}

                //models.Add(new DoctorViewModel
                //{
                //    User = users.SingleOrDefault(u => doctors.Where())
                //}
            }
            return View(models);
        }

        [HttpGet]
        public async Task<IActionResult> DoctorProfile(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user != null)
            {
                var employee = _context.Employees.SingleOrDefault(e => e.UserID == user.Id);
                var doctor = _context.Doctors.SingleOrDefault(d => d.EmployeeID == employee.EmployeeID);
                var appoitmentDate = _context.AppointmentDates.Where(date => date.DoctorID == doctor.ID);
                var provideServeces = _context.ProvideServices.Where(ps => ps.DoctorID == doctor.ID);
                var speciality = await _context.Specialties.FindAsync(doctor.SpecialtyID);
                var model = new DoctorViewModel
                {
                    User = user,
                    Doctor = doctor,
                    ProvideServices = provideServeces,
                    DatesOfAppointment = appoitmentDate,
                    SpecialtyName = speciality.Name
                };

                return View(model);
            }

            return View();
        }

        public async Task<IActionResult> PatientsRecords()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if(user.Role == UserRole.Врач)
            {
                var employee = _context.Employees.SingleOrDefault(e => e.UserID == user.Id);
                var doctor = _context.Doctors.FirstOrDefault(d => d.EmployeeID == employee.EmployeeID);
                var todayAppointment = _context.AppointmentDates.SingleOrDefault(date => date.DoctorID == doctor.ID && date.Date == DateTime.Today);
                var recordedAppoitment = _context.Appointments.Where(date => DateTime.Compare(date.Date.Date, DateTime.Today) == 0);
                var reservedTimes = _context.AppoitmentReservedTime.Where(rt => rt.DateOfAppointmentID == todayAppointment.DateOfAppointmentID && !recordedAppoitment.Any(ra => ra.PatientId == rt.UserID));
                List<PatientRecordViewModel> models = new List<PatientRecordViewModel>();
                foreach (var reserved in reservedTimes)
                {
                    var service = await _context.Services.FindAsync(reserved.ServiceID);
                    var reservedUser = await _context.Users.FindAsync(reserved.UserID);

                    models.Add(new PatientRecordViewModel
                    {
                        PatientID = reservedUser.Id,
                        PatientLastName = reservedUser.LastName,
                        PatientFirstName = reservedUser.FirstName,
                        ServiceId = service.ServiceID,
                        ServiceName = service.Name,
                        ReservedTimeId = reserved.ID,
                        Time = reserved.Time
                    });
                }
                return View("~/Views/Doctor/Patients.cshtml", models);
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Appointment(string userId, int serviceId, int reservedTimeId)
        {
            var patientUser = await _userManager.FindByIdAsync(userId);
            var patient = await _context.Patients.FindAsync(patientUser.Id);
            var doctorUser = await _userManager.GetUserAsync(HttpContext.User);
            var employee = _context.Employees.FirstOrDefault(e => e.UserID == doctorUser.Id);
            var doctor = _context.Doctors.FirstOrDefault(d => d.EmployeeID == employee.EmployeeID);
            var service = await _context.Services.FindAsync(serviceId);
            var reservedTime = await _context.AppoitmentReservedTime.FindAsync(reservedTimeId);

            AppoitmentViewModel model = new AppoitmentViewModel
            {
                DoctorId = doctor.ID,
                PatientId = patient.UserID,
                ServiceID = service.ServiceID,
                ReservedTimeID = reservedTime.ID
            };

            ViewBag.Message = string.Format("Прием пациента {0} {1}, Услуга: {2}", patientUser.LastName, patientUser.FirstName, service.Name);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Appointment(AppoitmentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var doctor = await _context.Doctors.FindAsync(model.DoctorId);
            var patient = await _context.Patients.FindAsync(model.PatientId);
            var service = await _context.Services.FindAsync(model.ServiceID);
            var reservedTime = await _context.AppoitmentReservedTime.FindAsync(model.ReservedTimeID);

            Appointment appointment = new Appointment
            {
                DoctorId = doctor.ID,
                Doctor = doctor,
                PatientId = patient.UserID,
                Patient = patient,
                ServiceID = service.ServiceID,
                Service = service,
                Date = reservedTime.Time,
                Information = model.Information,
                Result = model.Result
            };

            await _context.Appointments.AddAsync(appointment);
            await _context.SaveChangesAsync();

            ViewBag.Message = "Сохранено успешно";
            return View("Info");
        }

        [HttpGet]
        public async Task<IActionResult> AppointmentsDates()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (User.IsInRole(UserRole.Врач.ToString()))
            {
                Employee employee = _context.Employees.FirstOrDefault(e => e.UserID == user.Id);
                Doctor doctor = _context.Doctors.FirstOrDefault(d => d.EmployeeID == employee.EmployeeID);
                var appointmentsDates = _context.AppointmentDates.Where(date => date.DoctorID == doctor.ID && DateTime.Compare(date.Date, DateTime.Today) >= 1).OrderBy(date => date.Date).ToList();
                return View(appointmentsDates);
            }

            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> AppointmentDate(int appointmentDateId)
        {
            AppointmentDateViewModel model = new AppointmentDateViewModel();
            ViewBag.Message = "Создание даты приема";
            if (appointmentDateId != 0)
            {
                DateOfAppointment date = await _context.AppointmentDates.FindAsync(appointmentDateId);
                string[] values = date.PeriodOfWorking.Split('-');
                model = new AppointmentDateViewModel
                {
                    AppointmentDateId = date.DateOfAppointmentID,
                    DoctorId = date.DoctorID,
                    Date = date.Date.Date,
                    PeriodOfWorking = date.PeriodOfWorking,
                    StartOfWork = int.Parse(values[0]),
                    EndOfWork = int.Parse(values[1])
                };
                ViewBag.Message = "Изменение даты приема";
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AppointmentDate(AppointmentDateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if(model.EndOfWork < model.StartOfWork)
            {
                ViewBag.Message = "Дата окончания рабочего дня не может быть меньше начала";
                return View(model);
            }

            var user = await _userManager.GetUserAsync(HttpContext.User);
            var employee = _context.Employees.FirstOrDefault(e => e.UserID == user.Id);
            var doctor = _context.Doctors.FirstOrDefault(d => d.EmployeeID == employee.EmployeeID);
            if(model.AppointmentDateId == 0)
            {
                var existDate = _context.AppointmentDates.SingleOrDefault(d => d.DoctorID == doctor.ID && DateTime.Compare(d.Date, model.Date) == 0);

                if (existDate == null)
                {
                    DateOfAppointment date = new DateOfAppointment
                    {
                        DoctorID = doctor.ID,
                        Date = model.Date,
                        PeriodOfWorking = string.Format("{0}-{1}", model.StartOfWork, model.EndOfWork)
                    };
                    ViewBag.Message = "Дата приема создана";
                    await _context.AppointmentDates.AddAsync(date);
                }
                else
                {
                    ViewBag.Message = "В этот день уже есть прием";
                }
            }
            else
            {
                DateOfAppointment date = await _context.AppointmentDates.FindAsync(model.AppointmentDateId);
                date.Date = model.Date;
                date.PeriodOfWorking = string.Format("{0}-{1}", model.StartOfWork, model.EndOfWork);
                ViewBag.Message = "Дата приема успешно изменена";
            }

            await _context.SaveChangesAsync();
            return View("Info");
        }

        [HttpGet]
        public async Task<IActionResult> Patients(int doctorId, DateTime date, int serviceId)
        {
            var appointment = _context.AppointmentDates.FirstOrDefault(d => d.Date == DateTime.Today);
            if(appointment != null)
            {
                List<PatientViewModel> models = new List<PatientViewModel>();
                var reservedTimes = _context.AppoitmentReservedTime.Where(rt => rt.DateOfAppointmentID == appointment.DateOfAppointmentID);
                foreach(var time in reservedTimes)
                {
                    var user = await _context.Users.FindAsync(time.UserID);
                    var patient = await _context.Patients.FindAsync(user.Id);
                    models.Add(new PatientViewModel
                    {
                        PatientLastName = user.LastName,
                        PatientFirstName = user.FirstName,
                        PatientId = patient.UserID,
                        DoctorId = doctorId,
                        Date = date,
                        ServiceId = serviceId
                    });
                }

                return View("~/Views/Record/DoctorPatientRecordConform.cshtml", models);
            }

            return NotFound();
        }

        public async Task<IActionResult> DoctorPatientRecordConform(Guid patientId, int doctorId, DateTime date, int serviceId)
        {
            var user = await _userManager.FindByIdAsync(patientId.ToString());
            var appointmentDate = _context.AppointmentDates.FirstOrDefault(d => d.DoctorID == doctorId && d.Date.Date == date.Date);
            var service = await _context.Services.FindAsync(serviceId);
            var reserve = new ReservedTime
            {
                DateOfAppointmentID = appointmentDate.DateOfAppointmentID,
                DateOfAppointment = appointmentDate,
                UserID = user.Id,
                User = user,
                ServiceID = service.ServiceID,
                Service = service,
                Time = date
            };

            await _context.AppoitmentReservedTime.AddAsync(reserve);
            await _context.SaveChangesAsync();
            ViewBag.Message = "Пациент успешно записан";
            return View("Info");
        }
    }
}