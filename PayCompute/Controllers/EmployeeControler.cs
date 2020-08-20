using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.AspNetCore.Hosting;
using PayCompute.Entity;
using PayCompute.Models;
using PayCompute.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PayCompute.Controllers
{
    public class EmployeeControler : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public EmployeeControler(IEmployeeService employeeService, IWebHostEnvironment hostingEnvironment)
        {
            _employeeService = employeeService;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            var employees = _employeeService.GetAll().Select(employee => new EmployeeIndexViewModel 
            {
                Id = employee.Id,
                EmployeeNo = employee.EmployeeNo,
                ImageUrl = employee.ImageUrl,
                FullName = employee.FullName,
                Gender = employee.Gender,
                Designation = employee.Designation,
                City = employee.City,
                DateJoined = employee.DateJoined
            }).ToList();

            return View(employees);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new EmployeeCreateViewModel();
            return View(model);
        }

        // Use to send data to the server
        [HttpPost]
        [ValidateAntiForgeryToken] // Prevents cross-site Request Forgery Attacks
        public async Task<IActionResult> Create(EmployeeCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var employee = new Employee
                {
                    Id = model.Id,
                    EmployeeNo = model.EmployeeNo,
                    FirstName = model.FirstName,
                    MiddleName = model.MiddleName,
                    LastName = model.LastName,
                    FullName = model.FullName,
                    Gender = model.Gender,
                    Email = model.Email,
                    DOB = model.DOB,
                    DateJoined = model.DateJoined,
                    NationalInsuranceNo = model.NationalInsuranceNo,
                    paymentMethod = model.paymentMethod,
                    studentLoan = model.studentLoan,
                    unionMember = model.unionMember,
                    Address = model.Address,
                    City = model.City,
                    Phone = model.Phone,
                    PostCode = model.PostCode,
                    Designation = model.Designation
                };

                if (model.ImageUrl != null && model.ImageUrl.Length > 0)
                {
                    // this will be the upload direcotory
                    var uploadDir = @"images/employee";
                    // Retrieve the file name without the extension
                    var fileName = Path.GetFileNameWithoutExtension(model.ImageUrl.FileName);
                    // the extension of the file name
                    var extension = Path.GetExtension(model.ImageUrl.FileName);

                    var webRootPath = _hostingEnvironment.WebRootPath;

                    // Every time generates an image, generate a unique file name
                    fileName = DateTime.UtcNow.ToString("yymmssfff") + fileName + extension;

                    // Concatenate and adds a directory separator
                    var path = Path.Combine(webRootPath, uploadDir, fileName);

                    // Read the bytes and save/Create them
                    await model.ImageUrl.CopyToAsync(new FileStream(path, FileMode.Create));

                    // The imageUrl that will go to the database
                    employee.ImageUrl = "/" + uploadDir + "/" + fileName;
                }

                await _employeeService.CreateAsync(employee);

                return RedirectToAction(nameof(Index));
            }

            return View();

        }

        public IActionResult Edit(int id)
        {
            var employee = _employeeService.GetById(id);
            if (employee == null)
            {
                return NotFound();
            }

            var model = new EmployeeEditViewModel
            {
                Id = employee.Id,
                EmployeeNo = employee.EmployeeNo,
                FirstName = employee.FirstName,
                MiddleName = employee.MiddleName,
                LastName = employee.LastName,
                Gender = employee.Gender,
                Email = employee.Email,
                DOB = employee.DOB,
                DateJoined = employee.DateJoined,
                NationalInsuranceNo = employee.NationalInsuranceNo,
                paymentMethod = employee.paymentMethod,
                studentLoan = employee.studentLoan,
                unionMember = employee.unionMember,
                Address = employee.Address,
                City = employee.City,
                Phone = employee.Phone,
                PostCode = employee.PostCode,
                Designation = employee.Designation
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EmployeeEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var employee = _employeeService.GetById(model.Id);
                if (employee == null)
                {
                    return NotFound();
                }

                employee.EmployeeNo = model.EmployeeNo;
                employee.FirstName = model.FirstName;
                employee.LastName = model.LastName;
                employee.MiddleName = model.MiddleName;
                employee.NationalInsuranceNo = model.NationalInsuranceNo;
                employee.Gender = model.Gender;
                employee.Email = model.Email;
                employee.DOB = model.DOB;
                employee.DateJoined = model.DateJoined;
                employee.Phone = model.Phone;
                employee.Designation = model.Designation;
                employee.paymentMethod = model.paymentMethod;
                employee.studentLoan = model.studentLoan;
                employee.unionMember = model.unionMember;
                employee.Address = model.Address;
                employee.City = model.City;
                employee.PostCode = model.PostCode;

                if (employee.ImageUrl != null && model.ImageUrl.Length > 0)
                {
                    // this will be the upload direcotory
                    var uploadDir = @"images/employee";
                    // Retrieve the file name without the extension
                    var fileName = Path.GetFileNameWithoutExtension(model.ImageUrl.FileName);
                    // the extension of the file name
                    var extension = Path.GetExtension(model.ImageUrl.FileName);

                    var webRootPath = _hostingEnvironment.WebRootPath;

                    // Every time generates an image, generate a unique file name
                    fileName = DateTime.UtcNow.ToString("yymmssfff") + fileName + extension;

                    // Concatenate and adds a directory separator
                    var path = Path.Combine(webRootPath, uploadDir, fileName);

                    // Read the bytes and save/Create them
                    await model.ImageUrl.CopyToAsync(new FileStream(path, FileMode.Create));

                    // The imageUrl that will go to the database
                    employee.ImageUrl = "/" + uploadDir + "/" + fileName;
                }

                await _employeeService.UpdateAsync(employee);

                return RedirectToAction(nameof(Index));

            }

            return View();
        }

        [HttpGet]
        public IActionResult Detail(int id)
        {
            var employee = _employeeService.GetById(id);

            if (employee == null)
            {
                return NotFound();
            }

            EmployeeDetailViewModel model = new EmployeeDetailViewModel()
            {
                Id = employee.Id,
                EmployeeNo = employee.EmployeeNo,
                FullName = employee.FullName,
                Gender = employee.Gender,
                DOB = employee.DOB,
                DateJoined = employee.DateJoined,
                Designation = employee.Designation,
                NationalInsuranceNo = employee.NationalInsuranceNo,
                Phone = employee.Phone,
                Email = employee.Email,
                paymentMethod = employee.paymentMethod,
                studentLoan = employee.studentLoan,
                unionMember = employee.unionMember,
                Address = employee.Address,
                City = employee.City,
                ImageUrl = employee.ImageUrl,
                PostCode = employee.PostCode
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var employee = _employeeService.GetById(id);

            if (employee == null)
            {
                return NotFound();
            }

            var model = new EmployeeDeleteViewModel()
            {
                Id = employee.Id,
                FullName = employee.FullName
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(EmployeeDeleteViewModel model)
        {
            await _employeeService.Delete(model.Id);

            return RedirectToAction(nameof(Index));
        }


    }
}
