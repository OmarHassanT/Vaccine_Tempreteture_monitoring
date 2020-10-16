using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Graduation.Models;
using Graduation.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Graduation.Controllers
{
    [Authorize(Roles = "superAdmin,Admin,User")]

    public class ClinicController : Controller
    {
        private readonly IClinicRepository _clinicRepository;
        private readonly IDirectorateRepository _directorateRepository;
        private readonly UserManager<ApplicationUser> usermanager;

        public ClinicController(IClinicRepository clinicRepository,IDirectorateRepository directorateRepository,
                                UserManager<ApplicationUser> usermanager)
        {
            _clinicRepository = clinicRepository;
            _directorateRepository = directorateRepository;
            this.usermanager = usermanager;
        }
        [Authorize(Roles = "superAdmin")]
        public IActionResult Index()
        {
            var clinics =_clinicRepository.GetClinics();
            return View(clinics);
        }
        [Authorize(Roles = "superAdmin")]
        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }
        [Authorize(Roles = "superAdmin")]
        [HttpPost]
        public IActionResult Create(Clinic clinic)
        {
            if (ModelState.IsValid)
            {
                _clinicRepository.AddClinc(clinic);
                return RedirectToAction("details", new { id = clinic.clinicId });
            }

            return View();
        }
        public ViewResult Details(int id)
        {
           Clinic model= _clinicRepository.GetClinic(id);
            return View(model);
        }
        [Authorize(Roles = "superAdmin")]

        [HttpPost]
        public IActionResult Delete(int id)
        {
            _clinicRepository.Delete(id);

            return RedirectToAction("index");
        }
        [Authorize(Roles = "superAdmin")]

        [HttpGet]
        public ViewResult Edit(int id)
        {
          var model=  _clinicRepository.GetClinic(id);
            return View(model);
        }
        [Authorize(Roles = "superAdmin")]

        [HttpPost]
        public IActionResult Edit(Clinic  clinic)
        {
            if(ModelState.IsValid)
            {
                var model = _clinicRepository.Update(clinic);
                return RedirectToAction("details", new { id = model.clinicId });
            }
            return View();
        }
        private Task<ApplicationUser> GetCurrentUserAsync() => usermanager.GetUserAsync(HttpContext.User);

        public async Task<IActionResult> FridegTempretures(int ClinId,int pageIndex=1)
        {          
            var clinic = _clinicRepository.GetClinic(ClinId);
            if (clinic == null)
            {
                ViewBag.ErrorMessage = $"clinic with Id:{ClinId} can not be found";
                return View("NotFound");
            }

            var user = await GetCurrentUserAsync();
            if ((user.clinicId == ClinId || user.DirectorateId==clinic.DirectorateId) || await usermanager.IsInRoleAsync(user, "superAdmin"))
            {

            
            var model= await _clinicRepository.getTempretures(ClinId, 7, pageIndex);       
            ViewBag.Title = _clinicRepository.GetClinic(ClinId).Name;

            return View(model);
            }
            ViewBag.ErrorMessage = "page not found";
            return View("NotFound");
        }
    }
}