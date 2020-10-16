using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManagement.Models;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly IEmployeeRespository _employeeRespository;
        private readonly IHostingEnvironment _hostingEnvironment;

        public HomeController(IEmployeeRespository employeeRespository,
                               IHostingEnvironment hostingEnvironment)
        {
            _employeeRespository = employeeRespository;
            _hostingEnvironment = hostingEnvironment;

        }
       
       
       [Route("/")]
        public ViewResult Index()
        {
           var model= _employeeRespository.GetAllEmplyees();
            return View(model);
        }

        public ViewResult  Details(int? id)
        {
           
            Employee employee = _employeeRespository.GetEmployee(id.Value);
            if (employee == null)
            {
                Response.StatusCode = 404;
                return View("EmployeeNotFound",id.Value);
            }
            HomeDetailsViewMdoel homeDetailsViewMdoel = new HomeDetailsViewMdoel()
            {
                Employee= employee,
             PageTitle = "Details of Employee"
            };
       
            return  View(homeDetailsViewMdoel);
        }

        [HttpGet]
        [Authorize]
        public ViewResult Create()
        {

            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create(EmployeeCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = FilePhotoUplaodProcess(model);
                Employee NewEmployee = new Employee
                {
                    Name = model.Name,
                    Department = model.Department,
                    Email = model.Email,
                    PhotoPath = uniqueFileName
                };
                _employeeRespository.addEmployee(NewEmployee);
                return RedirectToAction("details", new { id = NewEmployee.Id });
            }
            return View();
         
        }
        [HttpGet]
        [Authorize]
        public ViewResult Edit(int id)
        {
           
           Employee employee = _employeeRespository.GetEmployee(id);
            EmployeeEditViewModel employeeEditViewModel = new EmployeeEditViewModel
            {
                Id = employee.Id,
                Name = employee.Name,
                Department = employee.Department,
                Email = employee.Email,
                ExistingPhotoPath = employee.PhotoPath
            };
           
            
            return View(employeeEditViewModel);
        }
        [HttpPost]
        [Authorize]
        public IActionResult Edit(EmployeeEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                Employee employee = _employeeRespository.GetEmployee(model.Id);
                employee.Name = model.Name;
                employee.Email = model.Email;
                employee.Department = model.Department;

                if(model.Photo != null)
                {
                    if (model.ExistingPhotoPath != null)
                    {
                        string FilePath=Path.Combine(_hostingEnvironment.WebRootPath, "images", model.ExistingPhotoPath);
                        System.IO.File.Delete(FilePath);
                    }
                    string uniqueFileName = FilePhotoUplaodProcess(model);
                    employee.PhotoPath = uniqueFileName;

                }
                
                _employeeRespository.Update(employee);
                return RedirectToAction("index");
            }
            return View();

        }

        private string FilePhotoUplaodProcess(EmployeeCreateViewModel model)
        {
            string uniqueFileName = null;
            if (model.Photo != null)
            {

                string uploadesFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                string filePath = Path.Combine(uploadesFolder, uniqueFileName);
                using(var fileSetrem= new FileStream(filePath, FileMode.Create))
                {
                    model.Photo.CopyTo(fileSetrem);

                }


            }

            return uniqueFileName;
        }
    }
}