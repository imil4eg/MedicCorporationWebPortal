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
    public class EmployeeController : Controller
    {
        protected IUnitOfWork _unitOfWork;

        protected UserManager<ApplicationUser> _userManager;

        protected RoleManager<ApplicationRole> _roleManager;

        public EmployeeController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            this._unitOfWork = unitOfWork;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult Employees()
        {
            var doctors = this._unitOfWork.Doctors.Find(d => !d.IsDeleted);
            List<EmployeeViewModel> models = new List<EmployeeViewModel>();
            foreach (Doctor doctor in doctors)
            {
                Employee employee = this._unitOfWork.Employees.Get(doctor.EmployeeID);
                ApplicationUser user = this._unitOfWork.Users.Get(employee.UserID);
                Specialty specialty = this._unitOfWork.Specialtys.Get(doctor.SpecialtyID);
                var workedDates = this._unitOfWork.DatesOfAppointments
                    .Find(date => date.DoctorID == doctor.ID && date.Date.Month == DateTime.Today.Month);
                int workedTime = 0;
                foreach (DateOfAppointment date in workedDates)
                {
                    string[] time = date.PeriodOfWorking.Split('-');
                    workedTime += int.Parse(time[1]) - int.Parse(time[0]);
                }
                models.Add(new EmployeeViewModel
                {
                    DoctorId = doctor.ID,
                    DoctorLastName = user.LastName,
                    DoctorFirstName = user.FirstName,
                    DoctorSpecilty = specialty.Name,
                    WorkedTime = workedTime
                });
            }

            return View(models);
        }
    }
}