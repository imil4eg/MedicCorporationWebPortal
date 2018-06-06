using MedicalCorporateWebPortal.AppData;
using MedicalCorporateWebPortal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalCorporateWebPortal.Controllers
{
    [Authorize(Roles = "Администратор")]
    public class AdminController : Controller
    {
        protected MedicCroporateContext _context;

        protected UserManager<User> _userManager;

        protected RoleManager<ApplicationRole> _roleManager;

        public AdminController(MedicCroporateContext context, UserManager<User> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Users()
        {
            var users = _context.Users.Where(u => !string.IsNullOrEmpty(u.Password));

            return View(users.ToList());
        }

        [HttpGet]
        public IActionResult UserProfile(string userName)
        {
            var user = _context.Users.SingleOrDefault(u => u.UserName == userName);
            
            if(user != null)
            {
                UserViewModel model = new UserViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    LastName = user.LastName,
                    FirstName = user.FirstName,
                    MiddleName = user.MiddleName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    DateOfBirth = user.DateOfBirth,
                    Gender = user.Gender,
                    Role = user.Role
                };

                return View(model);
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserProfile(UserViewModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);

            if(user != null)
            {
                var userRole = await _userManager.GetRolesAsync(user);
                if(userRole[0] != model.Role.ToString())
                {
                    await this.ChangeUserRole(user, model.Role);
                }
            }

            ViewBag.Message = "Роль успешно изменена!";
            return View("Info");
        }

        [HttpGet]
        public async Task<IActionResult> DeleteUser(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if(user != null)
            {
                var result = await _userManager.RemoveLoginAsync(user, user.UserName, user.Id.ToString());
                await _userManager.RemovePasswordAsync(user);
                user.UserName = string.Empty;
                user.Password = string.Empty;
                await _context.SaveChangesAsync();
            }

            ViewBag.Message = "Пользователь удален успешно";

            return View("Info");
        }

        [NonAction]
        public async Task ChangeUserRole(User user, UserRole changedRole)
        {
            if(user.Role == UserRole.Администратор || user.Role == UserRole.Бухгалтер || user.Role == UserRole.Ресепшен)
            {
                var employee = _context.Employees.SingleOrDefault(e => e.UserID == user.Id);
                var exisistDoctor = await _context.Doctors.FindAsync(employee.EmployeeID);

                if (exisistDoctor == null && changedRole == UserRole.Врач)
                {
                    var doctor = new Doctor
                    {
                        EmployeeID = employee.EmployeeID,
                        Employee = employee,
                        SpecialtyID = _context.Specialties.First().ID
                    };
                }
                else if(exisistDoctor != null)
                {
                    exisistDoctor.IsDeleted = false;
                }
            }
            else if(user.Role == UserRole.Врач)
            {
                var employee = _context.Employees.SingleOrDefault(e => e.UserID == user.Id);
                var doctor = _context.Doctors.SingleOrDefault(d => d.EmployeeID == employee.EmployeeID);
                doctor.IsDeleted = true;
            }

            var role = user.Role;
            user.Role = changedRole;
            await _userManager.AddToRoleAsync(user, changedRole.ToString());
            await _userManager.RemoveFromRoleAsync(user, role.ToString());
            await _context.SaveChangesAsync();
        }
    }
}