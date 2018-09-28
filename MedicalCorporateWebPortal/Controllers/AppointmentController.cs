using MedicalCorporateWebPortal.AppData;
using MedicalCorporateWebPortal.Models;
using MedicalCorporateWebPortal.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalCorporateWebPortal.Controllers
{
    public class AppointmentController : Controller
    {
        protected IUnitOfWork _unitOfWork;

        protected UserManager<ApplicationUser> _userManager;

        protected RoleManager<ApplicationRole> _roleManager;

        public AppointmentController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            this._unitOfWork = unitOfWork;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Appointments()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if(user != null)
            {
                List<Appointment> appointments = new List<Appointment>();
                List<AppoitmentViewModel> models = new List<AppoitmentViewModel>();
                if (user.Role == UserRole.Врач)
                {
                    var employee = this._unitOfWork.Employees.Find(e => e.UserID == user.Id).FirstOrDefault();
                    var doctor = this._unitOfWork.Doctors.Find(d => d.EmployeeID == employee.EmployeeID).FirstOrDefault();
                    appointments = this._unitOfWork.Appointments.Find(a => a.DoctorId == doctor.ID).ToList();

                    foreach (var appointment in appointments)
                    {
                        var patient = this._unitOfWork.Patients.Get(appointment.PatientId);
                        var patientUser = this._unitOfWork.Users.Get(patient.UserID);
                        var service = this._unitOfWork.Services.Get(appointment.ServiceID);

                        models.Add(new AppoitmentViewModel
                        {
                            AppointmentId = appointment.Id,    
                            DoctorId = appointment.DoctorId,
                            PatientId = appointment.PatientId,
                            PatientLastName = patientUser.LastName,
                            PatientFirstName = patientUser.FirstName,
                            Date = appointment.Date,
                            ServiceID = appointment.ServiceID,
                            ServiceName = service.Name,
                            Information = appointment.Information,
                            Result = appointment.Result
                        });
                    }
                }
                else if(user.Role == UserRole.Пациент)
                {
                    var patient = this._unitOfWork.Patients.Get(user.Id);
                    appointments = this._unitOfWork.Appointments.Find(a => a.PatientId == patient.UserID).ToList();

                    foreach (var appointment in appointments)
                    {
                        var doctor = this._unitOfWork.Doctors.Get(appointment.DoctorId);
                        var employee = this._unitOfWork.Employees.Get(doctor.EmployeeID);
                        var doctorUser = this._unitOfWork.Users.Get(employee.UserID);
                        var service = this._unitOfWork.Services.Get(appointment.ServiceID);

                        models.Add(new AppoitmentViewModel
                        {
                            AppointmentId = appointment.Id,
                            DoctorId = appointment.DoctorId,
                            DoctorLastName = doctorUser.LastName,
                            DoctorFirstName = doctorUser.FirstName,
                            PatientId = appointment.PatientId,
                            Date = appointment.Date,
                            ServiceID = appointment.ServiceID,
                            ServiceName = service.Name,
                            Information = appointment.Information,
                            Result = appointment.Result
                        });
                    }
                }

                return View(models);
            }

            return NotFound();
        }

        [HttpGet]
        public IActionResult AppointmentDetails(int appointmentId)
        {
            Appointment appoitment = this._unitOfWork.Appointments.Get(appointmentId);
            Patient patient = this._unitOfWork.Patients.Get(appoitment.PatientId);
            ApplicationUser patientUser = this._unitOfWork.Users.Get(patient.UserID);
            Service service = this._unitOfWork.Services.Get(appoitment.ServiceID);
            AppoitmentViewModel model = new AppoitmentViewModel
            {
                AppointmentId = appoitment.Id,
                Information = appoitment.Information,
                Result = appoitment.Result
            };

            ViewBag.Message = string.Format("Прием пациента {0} {1}, Услуга: {2}", patientUser.LastName, patientUser.FirstName, service.Name);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditAppointment(AppoitmentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            Appointment appointment = this._unitOfWork.Appointments.Get(model.AppointmentId);
            appointment.Information = model.Information;
            appointment.Result = model.Result;
            this._unitOfWork.Save();
            ViewBag.Message = "Данные успешно изменены";
            return View("Info");
        }
    }
}