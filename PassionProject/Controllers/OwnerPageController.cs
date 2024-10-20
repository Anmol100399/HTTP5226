using Microsoft.AspNetCore.Mvc;
using PassionProject.Data;
using PassionProject.Data.Migrations;
using PassionProject.Interface;
using PassionProject.Models;
using PassionProject.Models.ViewModels;

namespace PassionProject.Controllers
{
    public class OwnerPageController : Controller
    {
        private readonly IOwnerService _ownerService;
        private readonly ICarService _carService;
        private readonly ApplicationDbContext _context;

        // Dependency injection of service interface
        public OwnerPageController(ApplicationDbContext context, IOwnerService OwnerService, ICarService CarService)
        {
            _carService = CarService;
            _ownerService = OwnerService;
            _context = context;
        }
        public IActionResult Index()
        {
            return RedirectToAction("List");
        }

        // Dependency injection of service interface


        // GET: OwnerPage/List
        [HttpGet]
        public async Task<IActionResult> List()
        {
            return View(await _ownerService.ListOwners());
        }

        // GET: OwnerPage/Details/{id}
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            return View(await _ownerService.FindOwner(id));
        }

        // GET OwnerPage/New
        [HttpGet]
        public async Task<ActionResult> New()
        {

            var ownerDto = new OwnerDto();
            var cars = await _carService.ListCars();  // Fetch the list of cars
            ViewBag.Cars = cars;  // Pass the cars to the view using ViewBag
            ownerDto.Cars = cars.ToList();  // Pass the list of available cars to the view

            return View(ownerDto);
        }

        // POST OwnerPage/Add
        [HttpPost]
        public async Task<IActionResult> Create(OwnerDto ownerDto)
        {
            ServiceResponse response = await _ownerService.CreateOwner(ownerDto);

            if (response.Status == ServiceResponse.ServiceStatus.Created)
            {
                return RedirectToAction("List", "OwnerPage");
            }
            else
            {
                return View("Error", new ErrorViewModel() { Errors = response.Messages });
            }
        }

        // GET: Owner/Edit/{id}
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var ownerDto = await _ownerService.FindOwner(id);
            if (ownerDto == null)
            {
                return NotFound(); // Return 404 if the owner is not found
            }
            return View(ownerDto); // Pass the ownerDto to the edit view
        }

        // POST: Owner/Edit/{id}
        [HttpPost]
        public async Task<IActionResult> Update(int id, OwnerDto ownerDto)
        {
            if (id != ownerDto.OwnerId)
            {
                return BadRequest(); // Return 400 if IDs do not match
            }

            var response = await _ownerService.UpdateOwner(id, ownerDto);
            if (response.Status == ServiceResponse.ServiceStatus.Updated)
            {
                return RedirectToAction("List"); // Redirect to the list after successful update
            }
            return View(ownerDto); // Return to the edit view if there was an error
        }
    


        // GET OwnerPage/ConfirmDelete/{id}
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            OwnerDto? Owner = await _ownerService.FindOwner(id);
            if (Owner == null)
            {
                return NotFound();
            }
            else
            {
                return View(Owner);
            }
        }


        // POST OwnerPage/Delete/{id}
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            ServiceResponse response = await _ownerService.DeleteOwner(id);

            if (response.Status == ServiceResponse.ServiceStatus.Deleted)
            {
                return RedirectToAction("List", "OwnerPage");
            }
            else
            {
                return View("Error", new Models.ErrorViewModel() { Errors = response.Messages });
            }

        }
    }
}