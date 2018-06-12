using MedicalCorporateWebPortal.AppData;
using MedicalCorporateWebPortal.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalCorporateWebPortal.Controllers
{
    public class ServiceController : Controller
    {
        private readonly MedicCroporateContext _context;

        public ServiceController(MedicCroporateContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Services()
        {
            var services = _context.Services.Where(s => !s.IsDeleted);

            return View(services);
        }

        [HttpGet]
        public IActionResult CreateService()
        {
            var model = new ServiceViewModel();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateService(ServiceViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var service = new Service
            {
                Name = model.Name,
                Price = model.Price
            };

            await _context.Services.AddAsync(service);
            await _context.SaveChangesAsync();

            ViewBag.Message = "Услуга успешно создана";
            return View("Info");
        }

        [HttpGet]
        public async Task<IActionResult> EditService(int serviceId)
        {
            var service = await _context.Services.FindAsync(serviceId);
            var model = new ServiceViewModel
            {
                ServiceID = service.ServiceID,
                Name = service.Name,
                Price = service.Price
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditService(ServiceViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var service = await _context.Services.FindAsync(model.ServiceID);
            service.Name = model.Name;
            service.Price = model.Price;
            await _context.SaveChangesAsync();

            ViewBag.Message = "Услуга успешно изменена";
            return View("Info");
        }

        [HttpGet]
        public async Task<IActionResult> DeleteService(int serviceId)
        {
            var service = await _context.Services.FindAsync(serviceId);
            service.IsDeleted = true;
            await _context.SaveChangesAsync();

            ViewBag.Message = "Услуга успешно удалена";
            return View("Info");
        }

        [HttpGet]
        public async Task<IActionResult> ServiceProfile(int serviceId)
        {
            Service service = await _context.Services.FindAsync(serviceId);
            ServiceViewModel model = new ServiceViewModel
            {
                ServiceID = service.ServiceID,
                Name = service.Name,
                Descripition = service.Description,
                Price = service.Price
            };

            DateTime beginingOfWeek = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday);
            DateTime endOfWeek = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Friday);
            model.Doctors = new List<DoctorViewModel>();
            foreach (Doctor doctor in _context.Doctors.Where(d => _context.ProvideServices.Any(ps => ps.ServiceID == service.ServiceID && d.ID == ps.DoctorID)))
            {
                Employee employee = await _context.Employees.FindAsync(doctor.EmployeeID);
                User user = await _context.Users.FindAsync(employee.UserID);
                Specialty specialty = await _context.Specialties.FindAsync(doctor.SpecialtyID);
                var datesOfAppointemnt = _context.AppointmentDates.Where(date => doctor.ID == date.DoctorID
                                                            && date.Date >= beginingOfWeek && date.Date <= endOfWeek);

                model.Doctors.Add(new DoctorViewModel
                {
                    Doctor = doctor,
                    User = user,
                    SpecialtyName = specialty.Name,
                    DatesOfAppointment = datesOfAppointemnt
                });
            }

            return View(model);
        }
    }
}