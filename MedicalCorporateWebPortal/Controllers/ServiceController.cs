using MedicalCorporateWebPortal.AppData;
using MedicalCorporateWebPortal.Models;
using Microsoft.AspNetCore.Mvc;
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
    }
}