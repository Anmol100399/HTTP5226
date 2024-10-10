using PassionProject.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PassionProject.Interface
{
    public interface ICarService
    {
        // Fetch all cars
        Task<IEnumerable<CarDto>> ListCars();

        // Fetch a car by ID
        Task<CarDto> GetCar(int id);

        // Update an existing car
        Task<ServiceResponse> UpdateCarDetails(int id, CarDto carDto);

        // Create a new car
        Task<ServiceResponse> CreateCar(CarDto carDto);

        // Delete a car by ID
        Task<ServiceResponse> DeleteCar(int id);
    }
}
