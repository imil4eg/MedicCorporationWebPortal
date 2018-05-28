using MedicalCorporateWebPortal.AppData;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

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
            var services = _context.Services.Select(s => s);

            return View(services);
        }
    }
}