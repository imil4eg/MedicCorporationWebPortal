using MedicalCorporateWebPortal.AppData;
using MedicalCorporateWebPortal.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalCorporateWebPortal.Controllers
{
    public class AppointmentController : Controller
    {
        protected MedicCroporateContext _context;

        protected UserManager<User> _userManager;

        protected RoleManager<ApplicationRole> _roleManager;

        public AppointmentController(MedicCroporateContext context, UserManager<User> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _context = context;
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
                    var employee = _context.Employees.FirstOrDefault(e => e.UserID == user.Id);
                    var doctor = _context.Doctors.FirstOrDefault(d => d.EmployeeID == employee.EmployeeID);
                    appointments = _context.Appointments.Where(a => a.DoctorId == doctor.ID).ToList();

                    foreach (var appointment in appointments)
                    {
                        var patient = await _context.Patients.FindAsync(appointment.PatientId);
                        var patientUser = await _context.Users.FindAsync(patient.UserID);
                        var service = await _context.Services.FindAsync(appointment.ServiceID);

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
                    var patient = await _context.Patients.FindAsync(user.Id);
                    appointments = _context.Appointments.Where(a => a.PatientId == patient.UserID).ToList();

                    foreach (var appointment in appointments)
                    {
                        var doctor = await _context.Doctors.FindAsync(appointment.DoctorId);
                        var employee = await _context.Employees.FindAsync(doctor.EmployeeID);
                        var doctorUser = await _context.Users.FindAsync(employee.UserID);
                        var service = await _context.Services.FindAsync(appointment.ServiceID);

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
        public async Task<IActionResult> AppointmentDetails(int appointmentId)
        {
            Appointment appoitment = await _context.Appointments.FindAsync(appointmentId);
            Patient patient = await _context.Patients.FindAsync(appoitment.PatientId);
            User patientUser = await _context.Users.FindAsync(patient.UserID);
            Service service = await _context.Services.FindAsync(appoitment.ServiceID);
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

            Appointment appointment = await _context.Appointments.FindAsync(model.AppointmentId);
            appointment.Information = model.Information;
            appointment.Result = model.Result;
            await _context.SaveChangesAsync();
            ViewBag.Message = "Данные успешно изменены";
            return View("Info");
        }
    }
}