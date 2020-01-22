using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebSiteIdentityHTTP.Models;

namespace WebSiteIdentityHTTP.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<Role> roleManager;
        private readonly UserManager<User> userManager;

        public RoleController(RoleManager<Role> roleManager,UserManager<User> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }
        public IActionResult Index()
        {
            var roles = roleManager.Roles.Select(rl => new RoleViewModel() { RoleName = rl.Id }).ToList();
            return View(roles);
        }
        public IActionResult CreateRoles()
        {
            return View();
        }
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRole(RoleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("CreateRoles", model);
            }
            var role = new Role
            {
                Id=model.RoleName
            };
            await roleManager.CreateAsync(role);
            return RedirectToAction( "Index");
        }
         public IActionResult AssignUsers()
        {
            var role = roleManager.Roles.ToList();
            var user = userManager.Users.ToList();
            ViewBag.Role = role;
            ViewBag.User = user;
            return View();

        }
        public async Task<IActionResult> AssignUser(UserToRoleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("AssignUsers", model);
            }
            var role = await roleManager.FindByIdAsync(model.RoleName);
            if (role==null)
            {
                ModelState.AddModelError("", $"Role Name{model.RoleName} dose not exist");
                return  View("AssignUsers", model);
            }
            var user = await userManager.FindByIdAsync(model.UserName);
            if (user==null)
            {
                ModelState.AddModelError("", $"user name{model.UserName} dose nit exist");
                return View("AssignUsers", model);
            }
            role.users.Add(user);
            await roleManager.UpdateAsync(role);
            return RedirectToAction("Index");
        }
    }
}