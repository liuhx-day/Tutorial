using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using Newtonsoft.Json;
using Tutorial.Web.Model;
using Tutorial.Web.Services;
using Tutorial.Web.ViewModels;


namespace Tutorial.Web.Controllers
{
    public class HomeController:Controller
    {
        private readonly IRepository<Student> _repository;

        public HomeController(IRepository<Student> repository)
        {
            _repository = repository;
        }
        public IActionResult Index()
        {
            var list = _repository.GetAll();
            var vms = list.Select(x => new StudentViewModel
            {
                Id= x.Id,
                Name = $"{x.FirstName} {x.LastName}",
                Age = DateTime.Now.Subtract(x.BirthDate).Days / 365
            });

            var vm = new HomeIndexViewModel
            {
                Students = vms
            };
            return View(vm);
            
            
        }

        public IActionResult Detail(int id)
        {
            var student = _repository.GetById(id);
            if (student== null)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(StudentCreateViewModel student)
        {
            if (ModelState.IsValid)
            {
                var newStudent = new Student
                {
                    FirstName = student.FirstName,
                    LastName = student.LastName,
                    Gender = student.Gender,
                    BirthDate = student.BirthDate
                };
                var newModel = _repository.Add(newStudent);

                return RedirectToAction(nameof(Detail), new { id = newStudent.Id });
            }
            else
            {
                return View();
            }
            
        }
    }
}
