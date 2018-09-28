using MedicalCorporateWebPortal.AppData;
using MedicalCorporateWebPortal.Models;
using MedicalCorporateWebPortal.Repository;
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
        protected IUnitOfWork _unitOfWork;

        protected UserManager<ApplicationUser> _userManager;

        protected RoleManager<ApplicationRole> _roleManager;

        public PatientController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            this._unitOfWork = unitOfWork;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public async Task<IActionResult> Records()
        {
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            if(user != null)
            {
                if (User.IsInRole(UserRole.Пациент.ToString()))
                {
                    var recordsTime = this._unitOfWork.ReservedTimes
                        .Find(r => r.UserID == user.Id && DateTime.Compare(r.Time, DateTime.Now) >= 1);
                    List<RecordViewModel> models = new List<RecordViewModel>();
                    foreach (var time in recordsTime)
                    {
                        DateOfAppointment recordDate = this._unitOfWork.DatesOfAppointments.Get(time.DateOfAppointmentID);
                        Doctor doctor = this._unitOfWork.Doctors.Get(recordDate.DoctorID);
                        Employee employee = this._unitOfWork.Employees.Get(doctor.EmployeeID);
                        ApplicationUser doctorsUser = this._unitOfWork.Users.Get(employee.UserID);
                        Service service = this._unitOfWork.Services.Get(time.ServiceID);

                        models.Add(new RecordViewModel
                        {
                            ApplicationUser = doctorsUser,
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
        public IActionResult CancelRecord(int reservedTimeID)
        {
            var recordTime = this._unitOfWork.ReservedTimes.Get(reservedTimeID);
            this._unitOfWork.ReservedTimes.Remove(recordTime);
            this._unitOfWork.Save();

            ViewBag.Message = "Запись успешно отмнена";
            return View("Info");
        }
    }
}