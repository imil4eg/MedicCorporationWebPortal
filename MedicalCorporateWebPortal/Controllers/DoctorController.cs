using Microsoft.AspNetCore.Mvc;
using MedicalCorporateWebPortal.AppData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using MedicalCorporateWebPortal.Models;

namespace MedicalCorporateWebPortal.Controllers
{
    public class DoctorController : Controller
    {
        private readonly MedicCroporateContext _context;

        public DoctorController(MedicCroporateContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Doctors()
        {
            CultureInfo ci = new CultureInfo("ru-Ru");
            DateTimeFormatInfo dtfi = ci.DateTimeFormat;
            var users = _context.Users.Where(u => _context.Doctors.Any(d => d.EmployeeID == u.UserID));

            var tupleList = new List<Tuple<User, Doctor, IEnumerable<string>>>();
            foreach (var doctor in _context.Doctors)
            {
                List<string> AppointmentDates = new List<string>();
                var dates = _context.AppointmentDates.Where(date => users.Any(u => date.DoctorID == doctor.ID));
                if (dates.Count() > 0)
                {
                    foreach (var date in dates)
                    {
                        AppointmentDates.Add(dtfi.GetShortestDayName(date.Date.DayOfWeek));
                    }
                }

                tupleList.Add(new Tuple<User, Doctor, IEnumerable<string>>
                    (users.Single(u => u.UserID == doctor.EmployeeID), doctor, AppointmentDates));
            }
            return View(tupleList);
        }

        [HttpGet]
        public IActionResult DoctorProfile(int id)
        {
            var doctor = _context.Doctors.SingleOrDefault(d => d.EmployeeID == id);
            var employeee = _context.Employees.SingleOrDefault(e => e.UserID == doctor.EmployeeID);
            var user = _context.Users.SingleOrDefault(u => u.UserID == employeee.UserID);
            var appoitmentDate = _context.AppointmentDates.Where(date => date.DoctorID == doctor.EmployeeID);
            var provideServeces = _context.ProvideServices.Where(ps => ps.DoctorID == doctor.EmployeeID);
            var tuple = new Tuple<User, Doctor, IEnumerable<DateOfAppointment>, IEnumerable<DoctorProvideService>>(user, doctor, appoitmentDate, provideServeces);

            return View(tuple);
        }
    }
}