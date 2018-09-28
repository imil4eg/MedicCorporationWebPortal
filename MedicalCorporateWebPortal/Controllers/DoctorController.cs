using Microsoft.AspNetCore.Mvc;
using MedicalCorporateWebPortal.AppData;
using System;
using System.Collections.Generic;
using System.Linq;
using MedicalCorporateWebPortal.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using MedicalCorporateWebPortal.Repository;

namespace MedicalCorporateWebPortal.Controllers
{
    public class DoctorController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        protected UserManager<ApplicationUser> _userManager;

        protected RoleManager<ApplicationRole> _roleManager;

        public DoctorController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            this._unitOfWork = unitOfWork;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult Doctors()
        {
            var doctors = this._unitOfWork.Doctors.GetAll(); //Where(d => !d.IsDeleted);

            var models = new List<DoctorViewModel>();

            DateTime beginingOfWeek = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday);
            DateTime endOfWeek = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Friday);
            foreach (var doctor in doctors)
            {
                var employee = this._unitOfWork.Employees.Get(doctor.EmployeeID); //.FindAsync(doctor.EmployeeID);
                var user = this._unitOfWork.Users.Get(employee.UserID);
                var dates = this._unitOfWork.DatesOfAppointments.Find(date => doctor.ID == date.DoctorID
                                                            && date.Date >= beginingOfWeek && date.Date <= endOfWeek);

                var specialty = this._unitOfWork.Specialtys.Get(doctor.SpecialtyID);
                models.Add(new DoctorViewModel
                {
                    ApplicationUser = user,
                    Doctor = doctor,
                    DatesOfAppointment = dates.OrderBy(d => d.Date),
                    SpecialtyName = specialty.Name
                });
                //List<string> AppointmentDates = new List<string>();
                //var dates = this._unitOfWork.AppointmentDates.Where(date => users.Any(u => date.DoctorID == doctor.ID));
                //if (dates.Count() > 0)
                //{
                //    foreach (var date in dates)
                //    {
                //        AppointmentDates.Add(dtfi.GetShortestDayName(date.Date.DayOfWeek));
                //    }
                //}

                //models.Add(new DoctorViewModel
                //{
                //    ApplicationUser = users.SingleOrDefault(u => doctors.Where())
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
                var employee = this._unitOfWork.Employees.Find(e => e.UserID == user.Id).SingleOrDefault();
                var doctor = this._unitOfWork.Doctors.Find(d => d.EmployeeID == employee.EmployeeID).SingleOrDefault();
                var appoitmentDate = this._unitOfWork.DatesOfAppointments.Find(date => date.DoctorID == doctor.ID);
                var provideServeces = this._unitOfWork.DoctorProvideServices.Find(ps => ps.DoctorID == doctor.ID);
                var speciality = this._unitOfWork.Specialtys.Get(doctor.SpecialtyID);
                var model = new DoctorViewModel
                {
                    ApplicationUser = user,
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
                var employee = this._unitOfWork.Employees.Find(e => e.UserID == user.Id).SingleOrDefault();
                var doctor = this._unitOfWork.Doctors.Find(d => d.EmployeeID == employee.EmployeeID).FirstOrDefault();
                var todayAppointment = this._unitOfWork.DatesOfAppointments.Find(date => date.DoctorID == doctor.ID && date.Date == DateTime.Today).SingleOrDefault();
                var recordedAppoitment = this._unitOfWork.Appointments.Find(date => DateTime.Compare(date.Date.Date, DateTime.Today) == 0);
                var reservedTimes = this._unitOfWork.ReservedTimes.Find(rt => 
                        rt.DateOfAppointmentID == todayAppointment.DateOfAppointmentID && !recordedAppoitment.Any(ra => ra.PatientId == rt.UserID));
                List<PatientRecordViewModel> models = new List<PatientRecordViewModel>();
                foreach (var reserved in reservedTimes)
                {
                    var service = this._unitOfWork.Services.Get(reserved.ServiceID);
                    var reservedUser = this._unitOfWork.Users.Get(reserved.UserID);

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
            var patient = this._unitOfWork.Patients.Get(patientUser.Id);
            var doctorUser = await _userManager.GetUserAsync(HttpContext.User);
            var employee = this._unitOfWork.Employees.Find(e => e.UserID == doctorUser.Id).FirstOrDefault();
            var doctor = this._unitOfWork.Doctors.Find(d => d.EmployeeID == employee.EmployeeID).FirstOrDefault();
            var service = this._unitOfWork.Services.Get(serviceId);
            var reservedTime = this._unitOfWork.ReservedTimes.Get(reservedTimeId);

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

            var doctor = this._unitOfWork.Doctors.Get(model.DoctorId);
            var patient = this._unitOfWork.Patients.Get(model.PatientId);
            var service = this._unitOfWork.Services.Get(model.ServiceID);
            var reservedTime = this._unitOfWork.ReservedTimes.Get(model.ReservedTimeID);

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

            this._unitOfWork.Appointments.Add(appointment);
            this._unitOfWork.Save();

            ViewBag.Message = "Сохранено успешно";
            return View("Info");
        }

        [HttpGet]
        public async Task<IActionResult> AppointmentsDates()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (User.IsInRole(UserRole.Врач.ToString()))
            {
                Employee employee = this._unitOfWork.Employees.Find(e => e.UserID == user.Id).FirstOrDefault();
                Doctor doctor = this._unitOfWork.Doctors.Find(d => d.EmployeeID == employee.EmployeeID).FirstOrDefault();
                var appointmentsDates = this._unitOfWork.DatesOfAppointments.Find(date =>
                        date.DoctorID == doctor.ID && DateTime.Compare(date.Date, DateTime.Today) >= 1).OrderBy(date => date.Date).ToList();
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
                DateOfAppointment date = this._unitOfWork.DatesOfAppointments.Get(appointmentDateId);
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
            var employee = this._unitOfWork.Employees.Find(e => e.UserID == user.Id).FirstOrDefault();
            var doctor = this._unitOfWork.Doctors.Find(d => d.EmployeeID == employee.EmployeeID).FirstOrDefault();
            if(model.AppointmentDateId == 0)
            {
                var existDate = this._unitOfWork.DatesOfAppointments
                    .Find(d => d.DoctorID == doctor.ID && DateTime.Compare(d.Date, model.Date) == 0)
                    .SingleOrDefault();

                if (existDate == null)
                {
                    DateOfAppointment date = new DateOfAppointment
                    {
                        DoctorID = doctor.ID,
                        Date = model.Date,
                        PeriodOfWorking = string.Format("{0}-{1}", model.StartOfWork, model.EndOfWork)
                    };
                    ViewBag.Message = "Дата приема создана";
                    this._unitOfWork.DatesOfAppointments.Add(date);
                }
                else
                {
                    ViewBag.Message = "В этот день уже есть прием";
                }
            }
            else
            {
                DateOfAppointment date = this._unitOfWork.DatesOfAppointments.Get(model.AppointmentDateId);
                date.Date = model.Date;
                date.PeriodOfWorking = string.Format("{0}-{1}", model.StartOfWork, model.EndOfWork);
                ViewBag.Message = "Дата приема успешно изменена";
            }

            this._unitOfWork.Save();
            return View("Info");
        }

        [HttpGet]
        public async Task<IActionResult> Patients(int doctorId, DateTime date, int serviceId)
        {
            var appointment = this._unitOfWork.DatesOfAppointments
                .Find(d => d.Date == DateTime.Today)
                .FirstOrDefault();

            if(appointment != null)
            {
                List<PatientViewModel> models = new List<PatientViewModel>();
                var reservedTimes = this._unitOfWork.ReservedTimes
                    .Find(rt => rt.DateOfAppointmentID == appointment.DateOfAppointmentID);

                foreach (var patient in this._unitOfWork.Patients.GetAll())
                {
                    var user = this._unitOfWork.Users.Get(patient.UserID);
                    models.Add(new PatientViewModel
                    {
                        PatientId = patient.UserID,
                        PatientFirstName = user.FirstName,
                        PatientLastName = user.LastName,
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
            var appointmentDate = this._unitOfWork.DatesOfAppointments
                .Find(d => d.DoctorID == doctorId && d.Date.Date == date.Date)
                .FirstOrDefault();
            var service = this._unitOfWork.Services.Get(serviceId);
            var reserve = new ReservedTime
            {
                DateOfAppointmentID = appointmentDate.DateOfAppointmentID,
                DateOfAppointment = appointmentDate,
                UserID = user.Id,
                ApplicationUser = user,
                ServiceID = service.ServiceID,
                Service = service,
                Time = date
            };

            this._unitOfWork.ReservedTimes.Add(reserve);
            this._unitOfWork.Save();
            ViewBag.Message = "Пациент успешно записан";
            return View("Info");
        }
    }
}