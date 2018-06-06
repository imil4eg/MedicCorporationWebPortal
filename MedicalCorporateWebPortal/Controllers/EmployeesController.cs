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
    public class EmployeeController : Controller
    {
        protected MedicCroporateContext _context;

        protected UserManager<User> _userManager;

        protected RoleManager<ApplicationRole> _roleManager;

        public EmployeeController(MedicCroporateContext contex, UserManager<User> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _context = contex;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public async Task<IActionResult> Employees()
        {
            var doctors = _context.Doctors.Where(d => !d.IsDeleted);
            List<EmployeeViewModel> models = new List<EmployeeViewModel>();
            foreach (Doctor doctor in doctors)
            {
                Employee employee = await _context.Employees.FindAsync(doctor.EmployeeID);
                User user = await _context.Users.FindAsync(employee.UserID);
                Specialty specialty = await _context.Specialties.FindAsync(doctor.SpecialtyID);
                var workedDates = _context.AppointmentDates.Where(date => date.DoctorID == doctor.ID && date.Date.Month == DateTime.Today.Month);
                int workedTime = 0;
                foreach(DateOfAppointment date in workedDates)
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