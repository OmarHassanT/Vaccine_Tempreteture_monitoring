using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Graduation.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Graduation.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Graduation.Controllers
{
    [Authorize(Roles = "superAdmin,Admin")]

    public class DirectorateController : Controller
    {
        private readonly IDirectorateRepository _directorateRepository;
        private readonly UserManager<ApplicationUser> userManager;

        public DirectorateController(IDirectorateRepository directorateRepository,
                                     UserManager<ApplicationUser>userManager)
        {
            _directorateRepository = directorateRepository;
            this.userManager = userManager;
        }
        [Authorize(Roles = "superAdmin")]

        public IActionResult Index()
        {
          IEnumerable<Directorate> directorates=  _directorateRepository.GetDirectorates();


            return View(directorates);
        }

        [Authorize(Roles = "superAdmin")]
        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }
        [Authorize(Roles = "superAdmin")]
        [HttpPost]
        public IActionResult Create(Directorate directorate)
        {
            if (ModelState.IsValid)
            {
                _directorateRepository.AddDirectorate(directorate);
                return RedirectToAction("Details", new { id = directorate.DirectorateId });


            }
            return View();
        }
        private Task<ApplicationUser> GetCurrentUserAsync() => userManager.GetUserAsync(HttpContext.User);
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var user =await GetCurrentUserAsync();
            if (user.DirectorateId == id || await userManager.IsInRoleAsync(user,"superAdmin"))
            {
                Directorate directorate = _directorateRepository.GetDirectorate(id);
                return View(directorate);

            }

            ViewBag.ErrorMessage = "page not found";
            return View("NotFound"); 
        }

        [Authorize(Roles = "superAdmin")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
           Directorate model= _directorateRepository.GetDirectorate(id);
            
            return View(model);
        }
        [Authorize(Roles = "superAdmin")]
        [HttpPost]
        public IActionResult Edit(Directorate directorate)
        {
            if (ModelState.IsValid)
            {
                _directorateRepository.Update(directorate);
                return RedirectToAction("Details", new { id = directorate.DirectorateId });


            }
            return View();
        }
        [Authorize(Roles = "superAdmin")]
        [HttpPost]
        public IActionResult Delete(int id)
        {
            _directorateRepository.Delete(id);
            return RedirectToAction("index");
        }
        [Authorize(Roles = "superAdmin,Admin")]
        public async Task<IActionResult> clinics(int directoreateId)
        {
            var user = await GetCurrentUserAsync();
            if (user.DirectorateId == directoreateId || await userManager.IsInRoleAsync(user,"superAdmin") || await userManager.IsInRoleAsync(user, "Admin"))
            {

           
            HomeClinicsViewModel homeClinicsViewModel = new HomeClinicsViewModel
            {
                Clinics = _directorateRepository.GetClinics(directoreateId),
            pageTitle= _directorateRepository.GetDirectorate(directoreateId).Name

            };

            
            return View(homeClinicsViewModel);
            }
            ViewBag.ErrorMessage = "page not found";
            return View("NotFound");
        }

      
    }
}
