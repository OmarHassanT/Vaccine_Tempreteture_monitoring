using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Graduation.Models;
using Graduation.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Graduation.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IDirectorateRepository directorateRepositery;
        private readonly IClinicRepository clinicRepository;
        private readonly RoleManager<IdentityRole> roleManager;

        public AccountController(UserManager<ApplicationUser> userManager,
                                 SignInManager<ApplicationUser> signInManager,
                                 IDirectorateRepository directorateRepositery,
                                 IClinicRepository clinicRepository,
                                 RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.directorateRepositery = directorateRepositery;
            this.clinicRepository = clinicRepository;
            this.roleManager = roleManager;
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        //private Task<ApplicationUser> GetCurrentUserAsync() => userManager.GetUserAsync(HttpContext.User);

        [HttpPost]
        public async Task<IActionResult> Login(loginViewModel model,string returnURL)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError("", "Email or Password is inCorrect");
                    return View(model);

            }

                var result = await signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe,false);
                if (result.Succeeded)
                {
                  /*  if(!(await userManager.IsInRoleAsync(user,"superAdmin")))
                    {
                        if (user.DirectorateId != null && user.clinicId != null)

                            return RedirectToAction("FridegTempretures", "clinic",new { ClinId =user.clinicId});
                        else if(user.DirectorateId != null && user.clinicId==null)
                        {
                            return RedirectToAction("Details", "directorate", new { id = user.DirectorateId });
                        }
                    }else
                    if(!string.IsNullOrEmpty(returnURL)&& Url.IsLocalUrl(returnURL))
                    {
                        return Redirect(returnURL);
                    }*/
                    return RedirectToAction("index", "home");
                }
                ModelState.AddModelError("", "Email or Password is inCorrect");
            }
            return View(model);
        }
        [Authorize(Roles = "superAdmin")]
        [HttpGet]
        public IActionResult Register()
        {
            var model = new RegisterViewModel
            {
                Directorates = directorateRepositery.GetDirectorates().ToList(),
                
            };

            return View(model);
        }
        [Authorize(Roles = "superAdmin")]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {

           
                ApplicationUser user = new ApplicationUser
                {
                    Email = model.Email,
                    UserName = model.UserName,
                    DirectorateId = model.DirctorateId,
                    clinicId = model.ClinicId,
                

                };
             
               var result= await userManager.CreateAsync(user,model.Password);
                    if (result.Succeeded)
                    {
                    if(user.clinicId==null && user.DirectorateId == null)
                    {
                        await userManager.AddToRoleAsync(user, "superAdmin");
                    }
                    else if (user.clinicId == null)
                        {
                            await userManager.AddToRoleAsync(user, "Admin");
                        }
                        else
                        {
                            await userManager.AddToRoleAsync(user, "User");
                        
                        }
                        if (signInManager.IsSignedIn(User))
                            {
                                return RedirectToAction("AllUsers", "Administration");
                            }

                            await signInManager.SignInAsync(user,false);
                            return RedirectToAction("index", "home");
                    }
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
               

            }
            model.Directorates = directorateRepositery.GetDirectorates().ToList();
            return View(model);
        }

        [HttpGet]
        public JsonResult GetClinincsByDirecID(int directorateId)
        {
         var ClinicList=clinicRepository.GetClinicsFromDirectorate(directorateId);
            return Json(new SelectList(ClinicList, "clinicId", "Name"));
        }
        [Authorize(Roles = "superAdmin")]

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"can not find the user with id:{id}";
                return View("NotFound");

            }
            EditViewModel model = new EditViewModel
            {
                ClinicId = user.clinicId,
                UserName = user.UserName,
                DirctorateId = user.DirectorateId,
                Email = user.Email,
                Directorates = directorateRepositery.GetDirectorates().ToList(),
            };



            return View(model);
        }
        [Authorize(Roles = "superAdmin")]
        [HttpPost]
       public async Task<IActionResult> Edit(EditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);

                if (user == null)
                {
                    ViewBag.ErrorMessage = $"can not find the user with email:{model.Email}";
                    return View("NotFound");

                }
                user.clinicId = model.ClinicId;
                user.UserName = model.UserName;
                user.DirectorateId = model.DirctorateId;
                user.Email = model.Email;
            var result= await userManager.ChangePasswordAsync(user, model.OldPassword, model.Password);

                if (result.Succeeded)
                {
                    return RedirectToAction( "AllUsers", "administration");
                }
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);
            }
            return View(model);
        }
        
    }
}