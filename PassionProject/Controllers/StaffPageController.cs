using Microsoft.AspNetCore.Mvc;
using PassionProject.Interface;
using PassionProject.Models.ViewModels;
using PassionProject.Models;
using PassionProject.Data;

namespace PassionProject.Controllers
{
    public class StaffPageController : Controller
    {
        private readonly IStaffService _staffService;
        private readonly ICarService _carService;
        private readonly ApplicationDbContext _context;

        // Dependency injection of service interfaces
        public StaffPageController(ApplicationDbContext context, IStaffService staffService, ICarService carService)
        {
            _staffService = staffService;
            _carService = carService;
            _context = context;
        }

        // GET StaffPage/List
        [HttpGet]
        public async Task<IActionResult> List()
        {
            IEnumerable<StaffDto?> staffDtos = await _staffService.ListStaffs();
            return View(staffDtos);
        }

        // GET StaffPage/Details/{id}
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            StaffDto? staffDto = await _staffService.GetStaff(id);
            return View(staffDto);
        }

        // GET StaffPage/New
        public async Task<ActionResult> New()
        {
            var staffDto = new StaffDto();

            // Fetch available cars for selection
            var cars = await _carService.ListCars();
            staffDto.Cars = cars.ToList();  // Pass the list of available cars to the view

            // Return the "new.cshtml" view
            return View("new", staffDto);
        }

        // POST StaffPage/Create
        [HttpPost]
        public async Task<IActionResult> Create(StaffDto staffDto)
        {
            ServiceResponse response = await _staffService.CreateStaff(staffDto);

            if (response.Status == ServiceResponse.ServiceStatus.Created)
            {
                return RedirectToAction("List", "StaffPage");
            }
            else
            {
                return View("Error", new ErrorViewModel { Errors = response.Messages });
            }
        }

        // GET StaffPage/Edit/{id}
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            StaffDto? staffDto = await _staffService.GetStaff(id);
            if (staffDto == null)
            {
                return View("Error");
            }
            return View(staffDto);
        }

        // POST: Staff/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(int id, StaffDto staffDto)
        {
            if (id != staffDto.StaffId)
            {
                return BadRequest();
            }

            ServiceResponse response = await _staffService.UpdateStaff(id, staffDto);

            if (response.Status == ServiceResponse.ServiceStatus.Updated)
            {
                return RedirectToAction("List"); 
            }
            else
            {
                ModelState.AddModelError("", response.Messages.FirstOrDefault() ?? "Error updating staff member.");
            }

            return View(staffDto);
        }
    

    // GET StaffPage/Delete/{id}
    [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            StaffDto? staffDto = await _staffService.GetStaff(id);
            if (staffDto == null)
            {
                return View("Error");
            }
            return View(staffDto);
        }

        // POST StaffPage/DeleteConfirmed/{id}
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            ServiceResponse response = await _staffService.DeleteStaff(id);

            if (response.Status == ServiceResponse.ServiceStatus.Deleted)
            {
                return RedirectToAction("List", "StaffPage");
            }
            else
            {
                return View("Error", new ErrorViewModel { Errors = response.Messages });
            }
        }
    }
}
