using MedicalCorporateWebPortal.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalCorporateWebPortal.Controllers
{
    public class RolesController : Controller
    {
        protected RoleManager<ApplicationRole> _roleManager;
        protected UserManager<ApplicationUser> _userManager;

        public RolesController(RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid userId)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(userId.ToString());
            if(user != null)
            {
                var userRole = await _userManager.GetRolesAsync(user);
                var allRoles = _roleManager.Roles.ToList();
                ChangeRoleViewModel model = new ChangeRoleViewModel
                {
                    UserID = user.Id,
                    UserEmail = user.Email,
                    UserRole = userRole[0],
                    AllRoles = allRoles
                };

                return View(model);
            }

            return NotFound();
        }
        
        [HttpPost]
        public async Task<IActionResult> Edit(Guid userId, string role)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(userId.ToString());
            if(user != null)
            {
                var userRole = await _userManager.GetRolesAsync(user);

                var allRoles = _roleManager.Roles.ToList();

                if(userRole[0] != role)
                {
                    await _userManager.AddToRoleAsync(user, role);
                    await _userManager.RemoveFromRoleAsync(user, userRole[0]);
                }

                return View();
            }

            return NotFound();
        }
    }
}