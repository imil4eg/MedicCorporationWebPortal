using MedicalCorporateWebPortal.AppData;
using MedicalCorporateWebPortal.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalCorporateWebPortal.Controllers
{
    public class PatientController : Controller
    {
        protected MedicCroporateContext _contex;

        protected UserManager<User> _userManager;

        protected RoleManager<ApplicationRole> _roleManager;

        public PatientController(MedicCroporateContext contex, UserManager<User> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _contex = contex;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public async Task<IActionResult> Records()
        {
            User user = await _userManager.GetUserAsync(HttpContext.User);
            if(user != null)
            {
                if (User.IsInRole(UserRole.Пациент.ToString()))
                {
                    var recordsTime = _contex.AppoitmentReservedTime.Where(r => r.UserID == user.Id && DateTime.Compare(r.Time, DateTime.Now) >= 1);
                    List<RecordViewModel> models = new List<RecordViewModel>();
                    foreach (var time in recordsTime)
                    {
                        DateOfAppointment recordDate = await _contex.AppointmentDates.FindAsync(time.DateOfAppointmentID);
                        Doctor doctor = await _contex.Doctors.FindAsync(recordDate.DoctorID);
                        Employee employee = await _contex.Employees.FindAsync(doctor.EmployeeID);
                        User doctorsUser = await _contex.Users.FindAsync(employee.UserID);
                        Service service = await _contex.Services.FindAsync(time.ServiceID);

                        models.Add(new RecordViewModel
                        {
                            User = doctorsUser,
                            Doctor = doctor,
                            Date = time.Time,
                            Service = service,
                            ReservedTime = time
                        });
                    }

                    return View(models);
                }
            }

            ViewBag.Message = "Произошли некоторые ошибки";
            return View("Info");
        }

        [HttpGet]
        public async Task<IActionResult> CancelRecord(int reservedTimeID)
        {
            var recordTime = await _contex.AppoitmentReservedTime.FindAsync(reservedTimeID);
            _contex.Remove(recordTime);
            await _contex.SaveChangesAsync();

            ViewBag.Message = "Запись успешно отмнена";
            return View("Info");
        }
    }
}