using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Graduation.Models;
using Graduation.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Graduation.Controllers
{
    [Authorize(Roles = "superAdmin")]

    public class AdministrationController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManger;

        public AdministrationController(UserManager<ApplicationUser> userManager,
                                         RoleManager<IdentityRole> roleManger)
        {
            this.userManager = userManager;
            this.roleManger = roleManger;
        }
        [HttpGet]
        public IActionResult allRole()
        {
            var Roles = roleManger.Roles;
            return View(Roles);
        }

        [HttpGet]
        public IActionResult AllUsers()
        {
            var users = userManager.Users;
            return View(users);

        }
        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityRole role = new IdentityRole()
                {
                    Name = model.RoleName
                };
              var result=  await roleManger.CreateAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction("allRole");
                }
                else
                {
                    foreach(var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
          
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> deleteUser(string id)
        {
          var user= await userManager.FindByIdAsync(id);
            if (user == null)
            {
                ViewBag.ErrorMessage = "Can not find the user that you want to delete.";
                return View("NotFound");
            }
            var resutl = await userManager.DeleteAsync(user);
            if (resutl.Succeeded)
            {
                return RedirectToAction("AllUsers");
            }
            foreach(var error in resutl.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return RedirectToAction("AllUsers");

        }
       

    }
}