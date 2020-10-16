using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManagement.Models;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EmployeeManagement.Controllers
{
    public class AdministrationController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ILogger<AdministrationController> logger;

        public AdministrationController(RoleManager<IdentityRole> roleManager,
                                        UserManager<ApplicationUser> userManager,
                                        ILogger<AdministrationController> logger)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.logger = logger;
        }
        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {   if(ModelState.IsValid)
            {
                IdentityRole NewRole = new IdentityRole
                {
                    Name = model.RoleName
                    
                };
                var result = await roleManager.CreateAsync(NewRole);
           
                if(result.Succeeded)
                {
                    return RedirectToAction("ListRoles", "Administration");
                }
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }


                
            }

            return View(model);
        }
        [HttpGet]
        public IActionResult ListRoles()
        {
            IEnumerable< IdentityRole> Roles = roleManager.Roles;
            return View(Roles);
        }

        [HttpGet]
        public async Task< IActionResult> EditRole(string Id)
        {
           var Role= await roleManager.FindByIdAsync(Id);
            if(Role==null)
            {
                ViewBag.ErrorMessage = $"Role with Id={Id} not found.";
                return View("NotFound");

            }
           

            var model = new EditRoleViewModel
            {
                Id = Role.Id,
                RoleName = Role.Name
            };
            foreach(var user in userManager.Users)
            {
                if (await userManager.IsInRoleAsync(user,model.RoleName))
                {
                    model.Users.Add(user.UserName);
                }
            }
            return View(model);
            
          

        }

   
        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleViewModel model)
        {
            if (ModelState.IsValid)
            {

                var Role = await roleManager.FindByIdAsync(model.Id);
                if (Role == null)
                {
                    ViewBag.ErrorMessage = $"Role with Id={model.Id} not found.";
                    return View("NotFound");

                }
                else
                {
                    Role.Name = model.RoleName;
                    var result = await roleManager.UpdateAsync(Role);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("ListRoles", "administration");
                    }
                    foreach(var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }


            }


            return View(model);



        }
        [HttpGet]
        public  async Task<IActionResult> ManageUserRole(string userId)
        {
            ViewBag.userId = userId;

            var user =await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                
                ViewBag.ErrorMessage = $"user with ID:{userId} cannot be found.";
                return View("NotFound");
            }

            var model = new List<UserRolesViewModel>();
           
            foreach(var role in roleManager.Roles)
            {
                var userRolesViewModel = new UserRolesViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name
                };
                if(await userManager.IsInRoleAsync(user, role.Name))
                {
                    userRolesViewModel.IsSelected = true;
                }
                else
                {
                    userRolesViewModel.IsSelected = false;

                }
                model.Add(userRolesViewModel);

            }
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> ManageUserRole(List<UserRolesViewModel> model,string userId)
        {
            

            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {

                ViewBag.ErrorMessage = $"user with ID:{userId} cannot be found.";
                return View("NotFound");
            }

            var roles = await userManager.GetRolesAsync(user);
            var result = await userManager.RemoveFromRolesAsync(user, roles);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "cannot delete existing roles from the user");
                return View(model);
            }

            result =await userManager.AddToRolesAsync(user,
                model.Where(x => x.IsSelected).Select(y => y.RoleName));
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "cannot add roles to the user");
                return View(model);
            }

            return RedirectToAction("EditUSer",new { id= userId});
        }


        [HttpPost]
        public async Task<IActionResult> DeleteRole(string id)
        {
           var role= await roleManager.FindByIdAsync(id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with ID:{id} cannot found";
                return View("NotFound");
            }
            try
            {
               
                var result = await roleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

            }
            catch (DbUpdateException e)
            {
                logger.LogError($"deleting  role {e}");
                ViewBag.ErrorTitle = $"{role.Name} role is in used.";
                ViewBag.ErrorMessage = $"{role.Name} role used by other users. " +
                    $"if you want to delete it, first delete the users who use this role.Then delete the role.";

                return View("Error");
            }


            return RedirectToAction("ListRoles");


        }

        [HttpGet]
        public async Task<IActionResult> EditUsersInRole(string roleId)
        {
            ViewBag.roleId = roleId;
            var role = await roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id={roleId} cannot found";
                return View("NotFound");
            }
            var model = new List<UserRoleViewModel>();
            foreach(var user in userManager.Users)
            {
                var userRoleViewModel = new UserRoleViewModel
                    {
                        UserName = user.UserName,
                        UserId = user.Id,
                    };

                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    userRoleViewModel.IsSelected = true;
                }
                else
                {
                    userRoleViewModel.IsSelected = false;

                }
                model.Add(userRoleViewModel);
            }
            return View(model);

                
        }
        [HttpPost]
        public async Task<IActionResult> EditUsersInRole(List<UserRoleViewModel> model, string roleId)
        {
            var role = await roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be found";
                return View("NotFound");
            }

            for (int i = 0; i < model.Count; i++)
            {
                var user = await userManager.FindByIdAsync(model[i].UserId);

                IdentityResult result = null;

                if (model[i].IsSelected && !(await userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await userManager.AddToRoleAsync(user, role.Name);
                }
                else if (!model[i].IsSelected && await userManager.IsInRoleAsync(user, role.Name))
                {
                    result = await userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue;
                }

            }

            return RedirectToAction("EditRole", new { Id = roleId });
        }
        [HttpGet]
        [Authorize(Roles ="Admin")]
        public ViewResult ListUsers()
        {
            var Users = userManager.Users;
            return View(Users);
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            ApplicationUser user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with ID:{id} Cannot found";
                return View("NotFound");
            }
            
            var userClaims = await userManager.GetClaimsAsync(user);
            var userRols = await userManager.GetRolesAsync(user);
            var  model = new EditUserViewModel
            {
                UserName = user.UserName,
                Email = user.Email,
                City = user.city,
                Id = user.Id,
                Rols = userRols,
                claims = userClaims.Select(c => c.Value).ToList()
            };
                
           return View(model);

        }
        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {

            
                ApplicationUser user = await userManager.FindByIdAsync(model.Id);
                if (user == null)
                {
                    ViewBag.ErrorMessage = $"User with ID:{model.Id} Cannot found";
                    return View("NotFound");
                }
                user.UserName = model.UserName;
                user.Email = model.Email;
                user.city = model.City;
              var result=await  userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListUsers", "administration");
                }

                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);

        }
        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {

            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with ID:{id} Cannot found";
                return View("NotFound");
            }

            var result=await userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction("listUsers");
            }
            foreach(var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
             return View("listUsers", "Administration");
        }
    }
}