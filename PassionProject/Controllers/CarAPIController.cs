using PassionProject.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PassionProject.Interface;
using PassionProject.Models;

namespace PassionProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarsAPIController : ControllerBase
    {
        private readonly ICarService _carService;

        public CarsAPIController(ICarService carService)
        {
            _carService = carService;
        }

        /// <summary>
        /// Returns a list of Cars.
        /// </summary>
        /// <returns>
        /// 200 OK
        /// [{CarDto}, {CarDto}, ...]
        /// </returns>
        /// <example>
        /// GET: api/CarsAPI/List -> [{CarDto}, {CarDto}, ...]
        /// </example>
        [HttpGet(template: "List")]
        public async Task<IEnumerable<CarDto>> ListCars()
        {
            return await _carService.ListCars();
        }

        /// <summary>
        /// Returns a specific car by ID.
        /// </summary>
        /// <param name="id">The ID of the car to retrieve.</param>
        /// <returns>
        /// 200 OK
        /// {CarDto}
        /// </returns>
        /// <example>
        /// GET: api/CarsAPI/5 -> {CarDto}
        /// </example>
        [HttpGet("{id}")]
        public async Task<CarDto> GetCar(int id)
        {
            return await _carService.GetCar(id);
        }

        /// <summary>
        /// Creates a new car.
        /// </summary>
        /// <param name="carDto">The data transfer object containing car details.</param>
        /// <returns>
        /// 201 Created
        /// {ServiceResponse}
        /// </returns>
        /// <example>
        /// POST: api/CarsAPI/Add -> {ServiceResponse}
        /// </example>
        [HttpPost(template: "Add")]
        public async Task<ServiceResponse> CreateCar(CarDto carDto)
        {
            return await _carService.CreateCar(carDto);
        }

        /// <summary>
        /// Updates the details of an existing car.
        /// </summary>
        /// <param name="id">The ID of the car to update.</param>
        /// <param name="carDto">The updated data transfer object containing car details.</param>
        /// <returns>
        /// 200 OK
        /// {ServiceResponse}
        /// </returns>
        /// <example>
        /// PUT: api/CarsAPI/Update/5 -> {ServiceResponse}
        /// </example>
        [HttpPut(template: "Update/{id}")]
        public async Task<ServiceResponse> UpdateCarDetails(int id, CarDto carDto)
        {
            return await _carService.UpdateCar(id, carDto);
        }

        /// <summary>
        /// Deletes a specific car by ID.
        /// </summary>
        /// <param name="id">The ID of the car to delete.</param>
        /// <returns>
        /// 200 OK
        /// {ServiceResponse}
        /// </returns>
        /// <example>
        /// DELETE: api/CarsAPI/Delete/5 -> {ServiceResponse}
        /// </example>
        [HttpDelete("Delete/{id}")]
        public async Task<ServiceResponse> DeleteCar(int id)
        {
            return await _carService.DeleteCar(id);
        }
    }
}
