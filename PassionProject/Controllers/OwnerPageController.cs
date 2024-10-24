using Microsoft.AspNetCore.Mvc;
using PassionProject.Data;
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

        /// <summary>
        /// Redirects to the List action.
        /// </summary>
        /// <returns>Redirects to the List view.</returns>
        public IActionResult Index()
        {
            return RedirectToAction("List");
        }

        /// <summary>
        /// Retrieves a list of all owners.
        /// </summary>
        /// <returns>A view displaying a list of owners.</returns>
        /// <example>GET: OwnerPage/List -> List of owners.</example>
        [HttpGet]
        public async Task<IActionResult> List()
        {
            return View(await _ownerService.ListOwners());
        }

        /// <summary>
        /// Retrieves the details of a specific owner by ID.
        /// </summary>
        /// <param name="id">The ID of the owner to display.</param>
        /// <returns>A view displaying the owner details.</returns>
        /// <example>GET: OwnerPage/Details/{id} -> Owner details.</example>
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            return View(await _ownerService.FindOwner(id));
        }

        /// <summary>
        /// Displays the form for creating a new owner.
        /// </summary>
        /// <returns>A view for creating a new owner, with available cars.</returns>
        /// <example>GET: OwnerPage/New -> New owner form.</example>
        [HttpGet]
        public async Task<ActionResult> New()
        {
            var ownerDto = new OwnerDto();
            var cars = await _carService.ListCars();
            ViewBag.Cars = cars;
            ownerDto.Cars = cars.ToList();

            return View(ownerDto);
        }

        /// <summary>
        /// Creates a new owner.
        /// </summary>
        /// <param name="ownerDto">The data transfer object for the new owner.</param>
        /// <returns>Redirects to the list view or displays errors if creation fails.</returns>
        /// <example>POST: OwnerPage/Add -> Owner created.</example>
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

        /// <summary>
        /// Displays the form to edit an existing owner by ID.
        /// </summary>
        /// <param name="id">The ID of the owner to edit.</param>
        /// <returns>A view for editing the owner.</returns>
        /// <example>GET: OwnerPage/Edit/{id} -> Edit owner form.</example>
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var ownerDto = await _ownerService.FindOwner(id);
            if (ownerDto == null)
            {
                return NotFound();
            }
            return View(ownerDto);
        }

        /// <summary>
        /// Updates an existing owner by ID.
        /// </summary>
        /// <param name="id">The ID of the owner to update.</param>
        /// <param name="ownerDto">The updated data transfer object containing owner details.</param>
        /// <returns>Redirects to the list view or displays errors if update fails.</returns>
        /// <example>POST: OwnerPage/Edit/{id} -> Owner updated.</example>
        [HttpPost]
        public async Task<IActionResult> Update(int id, OwnerDto ownerDto)
        {
            if (id != ownerDto.OwnerId)
            {
                return BadRequest();
            }

            var response = await _ownerService.UpdateOwner(id, ownerDto);
            if (response.Status == ServiceResponse.ServiceStatus.Updated)
            {
                return RedirectToAction("List");
            }
            return View(ownerDto);
        }

        /// <summary>
        /// Displays the confirmation view for deleting an owner by ID.
        /// </summary>
        /// <param name="id">The ID of the owner to delete.</param>
        /// <returns>A view displaying the owner details for confirmation.</returns>
        /// <example>GET: OwnerPage/ConfirmDelete/{id} -> Owner deletion confirmation.</example>
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

        /// <summary>
        /// Confirms and deletes an owner by ID.
        /// </summary>
        /// <param name="id">The ID of the owner to delete.</param>
        /// <returns>Redirects to the list view or displays errors if deletion fails.</returns>
        /// <example>POST: OwnerPage/Delete/{id} -> Owner deleted.</example>
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
