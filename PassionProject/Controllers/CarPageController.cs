using Microsoft.AspNetCore.Mvc;
using PassionProject.Interface;
using PassionProject.Models.ViewModels;
using PassionProject.Models;
using Azure;
using PassionProject.Data;

namespace PassionProject.Controllers
{
    public class CarPageController : Controller
    {
        /// <summary>
        /// Redirects to the car list page.
        /// </summary>
        /// <returns>
        /// Redirect to <see cref="List"/>.
        /// </returns>
        public IActionResult Index()
        {
            return RedirectToAction("List");
        }

        private readonly IStaffService _staffService;
        private readonly ICarService _carService;
        private readonly IOwnerService _ownerService;
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Dependency injection of service interfaces.
        /// </summary>
        /// <param name="CarService">Car service interface.</param>
        /// <param name="StaffService">Staff service interface.</param>
        /// <param name="OwnerService">Owner service interface.</param>
        public CarPageController(ICarService CarService, IStaffService StaffService, IOwnerService OwnerService)
        {
            _carService = CarService;
            _staffService = StaffService;
            _ownerService = OwnerService;
        }

        /// <summary>
        /// Retrieves and displays a list of cars.
        /// </summary>
        /// <returns>View displaying the list of cars.</returns>
        /// <example>
        /// GET: CarPage/List
        /// </example>
        [HttpGet]
        public async Task<IActionResult> List()
        {
            IEnumerable<CarDto?> CarDtos = await _carService.ListCars();
            return View(CarDtos);
        }

        /// <summary>
        /// Retrieves and displays details of a specific car by ID.
        /// </summary>
        /// <param name="id">The ID of the car.</param>
        /// <returns>View displaying car details.</returns>
        /// <example>
        /// GET: CarPage/Details/5
        /// </example>
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            return View(await _carService.GetCar(id));
        }

        /// <summary>
        /// Returns the view to create a new car.
        /// </summary>
        /// <returns>View for creating a new car.</returns>
        /// <example>
        /// GET: CarPage/New
        /// </example>
        public ActionResult New()
        {
            return View();
        }

        /// <summary>
        /// Adds a new car to the system.
        /// </summary>
        /// <param name="carDto">The car data transfer object (DTO).</param>
        /// <returns>
        /// Redirects to <see cref="List"/> on success, or displays error on failure.
        /// </returns>
        /// <example>
        /// POST: CarPage/Add
        /// </example>
        [HttpPost]
        public async Task<IActionResult> Add(CarDto carDto)
        {
            ServiceResponse response = await _carService.CreateCar(carDto);

            if (response.Status == ServiceResponse.ServiceStatus.Created)
            {
                return RedirectToAction("List", "CarPage");
            }
            else
            {
                return View("Error", new Models.ErrorViewModel() { Errors = response.Messages });
            }
        }

        /// <summary>
        /// Retrieves and displays the edit form for a specific car.
        /// </summary>
        /// <param name="id">The ID of the car to edit.</param>
        /// <returns>View for editing car details.</returns>
        /// <example>
        /// GET: CarPage/Edit/5
        /// </example>
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            CarDto? carDto = await _carService.GetCar(id);
            if (carDto == null)
            {
                return View("Error", new ErrorViewModel { Errors = new List<string> { "Car not found." } });
            }
            return View(carDto);
        }

        /// <summary>
        /// Updates the details of a specific car.
        /// </summary>
        /// <param name="id">The ID of the car to update.</param>
        /// <param name="carDto">The updated car data transfer object (DTO).</param>
        /// <returns>
        /// Redirects to <see cref="List"/> on success, or displays error on failure.
        /// </returns>
        /// <example>
        /// POST: CarPage/Edit/5
        /// </example>
        [HttpPost]
        public async Task<IActionResult> Edit(int id, CarDto carDto)
        {
            if (id != carDto.CarId)
            {
                return View("Error", new ErrorViewModel { Errors = new List<string> { "Car ID mismatch." } });
            }

            if (ModelState.IsValid)
            {
                ServiceResponse response = await _carService.UpdateCar(id, carDto);
                if (response.Status == ServiceResponse.ServiceStatus.Updated)
                {
                    return RedirectToAction("List", "CarPage");
                }
                else
                {
                    return View("Error", new ErrorViewModel { Errors = response.Messages });
                }
            }

            return View(carDto);
        }

        /// <summary>
        /// Retrieves and displays the confirmation view for deleting a specific car.
        /// </summary>
        /// <param name="id">The ID of the car to delete.</param>
        /// <returns>View for confirming the deletion of a car.</returns>
        /// <example>
        /// GET: CarPage/Delete/5
        /// </example>
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            CarDto carDto = await _carService.GetCar(id);
            if (carDto == null)
            {
                return NotFound();
            }

            return View(carDto);
        }

        /// <summary>
        /// Deletes a specific car by ID.
        /// </summary>
        /// <param name="id">The ID of the car to delete.</param>
        /// <returns>
        /// Redirects to <see cref="List"/> on success, or displays error on failure.
        /// </returns>
        /// <example>
        /// POST: CarPage/DeleteConfirmed/5
        /// </example>
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            ServiceResponse response = await _carService.DeleteCar(id);
            if (response.Status == ServiceResponse.ServiceStatus.Deleted)
            {
                return RedirectToAction("List", "CarPage");
            }
            else
            {
                return View("Error", new Models.ErrorViewModel() { Errors = response.Messages });
            }
        }
    }
}
