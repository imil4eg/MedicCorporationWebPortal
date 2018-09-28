using MedicalCorporateWebPortal.Models;
using MedicalCorporateWebPortal.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MedicalCorporateWebPortal.Controllers
{
    public class ProfileController : Controller
    {
        protected IUnitOfWork _unitOfWork;

        protected UserManager<ApplicationUser> _userManager;

        protected RoleManager<ApplicationRole> _roleManager;

        public ProfileController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            this._unitOfWork = unitOfWork;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if(user != null)
            {
                var model = new ProfileViewModel
                {
                    LastName = user.LastName,
                    FirstName = user.FirstName,
                    MiddleName = user.MiddleName,
                    Email = user.Email,
                    Gender = user.Gender,
                    PhoneNumber = user.PhoneNumber,
                    DateOfBirth = user.DateOfBirth.Date
                };

                if (user.Role == UserRole.Пациент)
                {
                    var patient = this._unitOfWork.Patients.Get(user.Id);

                    model.Address = patient.Address;
                    model.SNILS = patient.SNILS;
                    model.InsuranceCompany = patient.InsuranceCompany;
                    model.InsuranceNumber = patient.InsuranceNumber;
                    model.PassportNumber = patient.PassportNumber;
                    model.PassportSeries = patient.PassportSeries;
                }

                return View(model);
            }

            ViewBag.Message = "Пользователя не существует!";
            return View("Info");
        }

        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user != null)
            {
                var model = new ProfileViewModel
                {
                    LastName = user.LastName,
                    FirstName = user.FirstName,
                    MiddleName = user.MiddleName,
                    Email = user.Email,
                    Gender = user.Gender,
                    PhoneNumber = user.PhoneNumber,
                    DateOfBirth = user.DateOfBirth,
                };

                if(user.Role == UserRole.Пациент)
                {
                    var patient = this._unitOfWork.Patients.Get(user.Id);
                    model.Address = patient.Address;
                    model.SNILS = patient.SNILS;
                    model.InsuranceCompany = patient.InsuranceCompany;
                    model.InsuranceNumber = patient.InsuranceNumber;
                    model.PassportNumber = patient.PassportNumber;
                    model.PassportSeries = patient.PassportSeries;
                }

                return View(model);
            }

            ViewBag.Message = "Пользователя не существует!";
            return View("Info");
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(ProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(HttpContext.User);
            if(user != null)
            {
                user.LastName = model.LastName;
                user.FirstName = model.FirstName;
                user.MiddleName = model.MiddleName;
                user.PhoneNumber = model.PhoneNumber;
                user.Email = model.Email;
                user.DateOfBirth = model.DateOfBirth;
                user.Gender = model.Gender;
                if(user.Role == UserRole.Пациент)
                {
                    var patient = this._unitOfWork.Patients.Get(user.Id);
                    patient.Address = model.Address;
                    patient.PassportSeries = model.PassportSeries;
                    patient.PassportNumber = model.PassportNumber;
                    patient.InsuranceNumber = model.InsuranceNumber;
                    patient.InsuranceCompany = model.InsuranceCompany;
                    patient.SNILS = model.SNILS;
                }
            }

            await _userManager.UpdateAsync(user);
            this._unitOfWork.Save();
            ViewBag.Message = "Пользователь успешно изменен";
            return View("Info");
        }
    }
}