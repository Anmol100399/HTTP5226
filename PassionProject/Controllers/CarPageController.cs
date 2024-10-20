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
        public IActionResult Index()
        {
            return RedirectToAction("List");
        }
        private readonly IStaffService _staffService;
        private readonly ICarService _carService;
        private readonly IOwnerService _ownerService;
        private readonly ApplicationDbContext _context;


        // dependency injection of service interface
        public CarPageController(ICarService CarService, IStaffService StaffService, IOwnerService OwnerService)
        {
            _carService = CarService;
            _staffService = StaffService;
            _ownerService = OwnerService;
        }
        // GET: CarPage/List
        [HttpGet]
        public async Task<IActionResult> List()
        {
            IEnumerable<CarDto?> CarDtos = await _carService.ListCars();
            return View(CarDtos);
        }

        // GET: CarPage/Details/{id}
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            return View(await _carService.GetCar(id)); // Pass the carDto to the view
        }


        // GET: Create car view
        public ActionResult New()
        {
            return View();
        }



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

        // GET: CarPage/Edit/{id}
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            // Retrieve car details using the service method
            CarDto? carDto = await _carService.GetCar(id);

            if (carDto == null)
            {
                // If car not found, return error page
                return View("Error", new ErrorViewModel { Errors = new List<string> { "Car not found." } });
            }

            // Pass the car details to the view for editing
            return View(carDto);
        }

        // POST: CarPage/Edit/{id}
        [HttpPost]
        public async Task<IActionResult> Edit(int id, CarDto carDto)
        {
            // Check if the car ID in the URL matches the carDto's ID
            if (id != carDto.CarId)
            {
                return View("Error", new ErrorViewModel { Errors = new List<string> { "Car ID mismatch." } });
            }

            // Check if the model state is valid before saving changes
            if (ModelState.IsValid)
            {
                // Call the service to update the car details
                ServiceResponse response = await _carService.UpdateCar(id, carDto);

                // Check if the update was successful
                if (response.Status == ServiceResponse.ServiceStatus.Updated)
                {
                    // Redirect to a list or detail page after successful update
                    return RedirectToAction("List", "CarPage");
                }
                else
                {
                    // Handle any errors during update
                    return View("Error", new ErrorViewModel { Errors = response.Messages });
                }
            }

            // If validation fails, return the same view with the existing data
            return View(carDto);
        }


        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            CarDto carDto = await _carService.GetCar(id);

            if (carDto == null)
            {
                return NotFound(); // Return 404 if the car is not found
            }

            return View(carDto); // Pass the car details to the delete confirmation view
        }
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
                return View("Error", new Models.ErrorViewModel() { Errors = response.Messages }); ; // Handle cases where the car is not found or the deletion fails

            }
        }

    }
}
