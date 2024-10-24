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

        /// <summary>
        /// Retrieves a list of staff members.
        /// </summary>
        /// <returns>
        /// 200 OK
        /// [{StaffDto}, {StaffDto}, ...]
        /// </returns>
        /// <example>
        /// GET StaffPage/List -> [{StaffDto}, {StaffDto}, ...]
        /// </example>
        [HttpGet]
        public async Task<IActionResult> List()
        {
            IEnumerable<StaffDto?> staffDtos = await _staffService.ListStaffs();
            return View(staffDtos);
        }

        /// <summary>
        /// Retrieves details of a specific staff member by ID.
        /// </summary>
        /// <param name="id">The ID of the staff member.</param>
        /// <returns>
        /// 200 OK
        /// {StaffDto}
        /// </returns>
        /// <example>
        /// GET StaffPage/Details/{id} -> {StaffDto}
        /// </example>
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            StaffDto? staffDto = await _staffService.GetStaff(id);
            return View(staffDto);
        }

        /// <summary>
        /// Loads the form to create a new staff member.
        /// </summary>
        /// <returns>
        /// 200 OK
        /// {StaffDto with Cars}
        /// </returns>
        /// <example>
        /// GET StaffPage/New -> {StaffDto}
        /// </example>
        public async Task<ActionResult> New()
        {
            var staffDto = new StaffDto();
            var cars = await _carService.ListCars();
            staffDto.Cars = cars.ToList();
            return View("new", staffDto);
        }

        /// <summary>
        /// Creates a new staff member.
        /// </summary>
        /// <param name="staffDto">The data transfer object containing staff details.</param>
        /// <returns>
        /// 201 Created
        /// Redirect to List
        /// </returns>
        /// <example>
        /// POST StaffPage/Create -> Redirect to List
        /// </example>
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

        /// <summary>
        /// Retrieves details for editing a specific staff member by ID.
        /// </summary>
        /// <param name="id">The ID of the staff member.</param>
        /// <returns>
        /// 200 OK
        /// {StaffDto}
        /// </returns>
        /// <example>
        /// GET StaffPage/Edit/{id} -> {StaffDto}
        /// </example>
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

        /// <summary>
        /// Updates the details of a specific staff member by ID.
        /// </summary>
        /// <param name="id">The ID of the staff member.</param>
        /// <param name="staffDto">The updated data transfer object containing staff details.</param>
        /// <returns>
        /// 200 OK
        /// Redirect to List
        /// </returns>
        /// <example>
        /// POST StaffPage/Edit/{id} -> Redirect to List
        /// </example>
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

        /// <summary>
        /// Retrieves a staff member for deletion confirmation by ID.
        /// </summary>
        /// <param name="id">The ID of the staff member.</param>
        /// <returns>
        /// 200 OK
        /// {StaffDto}
        /// </returns>
        /// <example>
        /// GET StaffPage/Delete/{id} -> {StaffDto}
        /// </example>
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

        /// <summary>
        /// Confirms and deletes a specific staff member by ID.
        /// </summary>
        /// <param name="id">The ID of the staff member.</param>
        /// <returns>
        /// 200 OK
        /// Redirect to List
        /// </returns>
        /// <example>
        /// POST StaffPage/DeleteConfirmed/{id} -> Redirect to List
        /// </example>
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