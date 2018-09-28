using MedicalCorporateWebPortal.Models;
using MedicalCorporateWebPortal.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalCorporateWebPortal.Controllers
{
    public class ServiceController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ServiceController(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult Services()
        {
            var services = this._unitOfWork.Services.Find(s => !s.IsDeleted);

            return View(services);
        }

        [HttpGet]
        public IActionResult CreateService()
        {
            var model = new ServiceViewModel();

            return View(model);
        }

        [HttpPost]
        public IActionResult CreateService(ServiceViewModel model)
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

            this._unitOfWork.Services.Add(service);
            this._unitOfWork.Save();

            ViewBag.Message = "Услуга успешно создана";
            return View("Info");
        }

        [HttpGet]
        public IActionResult EditService(int serviceId)
        {
            var service = this._unitOfWork.Services.Get(serviceId);
            var model = new ServiceViewModel
            {
                ServiceID = service.ServiceID,
                Name = service.Name,
                Price = service.Price
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult EditService(ServiceViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var service = this._unitOfWork.Services.Get(model.ServiceID);
            service.Name = model.Name;
            service.Price = model.Price;
            this._unitOfWork.Save();

            ViewBag.Message = "Услуга успешно изменена";
            return View("Info");
        }

        [HttpGet]
        public IActionResult DeleteService(int serviceId)
        {
            var service = this._unitOfWork.Services.Get(serviceId);
            service.IsDeleted = true;
            this._unitOfWork.Save();

            ViewBag.Message = "Услуга успешно удалена";
            return View("Info");
        }

        [HttpGet]
        public IActionResult ServiceProfile(int serviceId)
        {
            Service service = this._unitOfWork.Services.Get(serviceId);
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
            foreach (Doctor doctor in this._unitOfWork.Doctors
                .Find(d => this._unitOfWork.DoctorProvideServices.Find(ps => ps.ServiceID == service.ServiceID && d.ID == ps.DoctorID).Any()))
            {
                Employee employee = this._unitOfWork.Employees.Get(doctor.EmployeeID);
                ApplicationUser user = this._unitOfWork.Users.Get(employee.UserID);
                Specialty specialty = this._unitOfWork.Specialtys.Get(doctor.SpecialtyID);
                var datesOfAppointemnt = this._unitOfWork.DatesOfAppointments
                    .Find(date => doctor.ID == date.DoctorID
                                                            && date.Date >= beginingOfWeek && date.Date <= endOfWeek);

                model.Doctors.Add(new DoctorViewModel
                {
                    Doctor = doctor,
                    ApplicationUser = user,
                    SpecialtyName = specialty.Name,
                    DatesOfAppointment = datesOfAppointemnt
                });
            }

            return View(model);
        }
    }
}