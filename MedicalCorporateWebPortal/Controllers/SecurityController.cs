using MedicalCorporateWebPortal.Models;
using MedicalCorporateWebPortal.AppData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace MedicalCorporateWebPortal.Controllers
{
    [Authorize]
    public class SecurityController : Controller
    {
        protected MedicCroporateContext _context;

        /// <summary>
        /// The manager for handling user creation, deletion, searching, roles
        /// </summary>
        protected UserManager<User> _userManager;

        /// <summary>
        /// The manager for handling sign in and out for our users 
        /// </summary>
        protected SignInManager<User> _signInManager;

        public SecurityController(MedicCroporateContext context, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            var model = new LoginViewModel();
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            User user = await _userManager.FindByNameAsync(model.UserName);
            if(user == null)
            {
                ViewBag.errorMessage = "Неверный логин или пароль";
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, false, false);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "Неверный логин или пароль");
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string returnUrl = null)
        {
            var model = new RegisterViewModel();
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByNameAsync(model.Login);

            if(user != null)
            {
                ModelState.AddModelError("", "Подобный логин уже используется");
                return View(model);
            }

            if (ModelState.IsValid)
            {
                var result = await _userManager.CreateAsync(new User
                {
                    Email = model.Email,
                    UserName = model.Login,
                    Password = model.Password,
                    LastName = model.LastName,
                    FirstName = model.FirstName,
                    MiddleName = model.MiddleName,
                    PhoneNumber = model.Phone,
                    DateOfBirth = model.DateOfBirth.Date,
                    Gender = model.Gender,
                }, model.Password);

                if (result.Succeeded)
                {
                    user = await _userManager.FindByNameAsync(model.Login);

                    if (User.IsInRole(UserRole.Администратор.ToString()))
                    {
                        var employee = new Employee
                        {
                            UserID = user.Id,
                            User = user
                        };
                        user.Role = UserRole.Ресепшен;
                        await _context.Employees.AddAsync(employee);
                        await _userManager.AddToRoleAsync(user, UserRole.Ресепшен.ToString());
                    }
                    else
                    {
                        var patient = new Patient
                        {
                            UserID = user.Id,
                            User = user
                        };
                        user.Role = UserRole.Пациент;
                        await _context.Patients.AddAsync(patient);
                        await _userManager.AddToRoleAsync(user, UserRole.Пациент.ToString());
                    }

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    ViewBag.Message = "Пользователь зарегистрирован";
                    await _context.SaveChangesAsync();
                    return View("Info");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            ViewBag.Message = "Ошибка при регистрации";
            return View(model);
        }
    }
}