using PassionProject.Data;
using PassionProject.Interface;
using PassionProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PassionProject.Services
{
    public class CarService : ICarService
    {
        private readonly ApplicationDbContext _context;

        public CarService(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Lists all cars with their respective owners.
        /// </summary>
        /// <returns>
        /// 200 OK
        /// [{CarDto}, {CarDto}, ...]
        /// </returns>
        /// <example>
        /// ListCars() -> [{CarDto}, {CarDto}, ...]
        /// </example>
        public async Task<IEnumerable<CarDto>> ListCars()
        {
            return await _context.Cars
                .Include(c => c.Owner)
                .Select(car => new CarDto
                {
                    CarId = car.CarId,
                    Make = car.Make,
                    Model = car.Model,
                    Year = car.Year,
                    OwnerId = car.OwnerId,
                    OwnerName = car.Owner.FirstName + " " + car.Owner.LastName
                }).ToListAsync();
        }

        /// <summary>
        /// Retrieves the details of a specific car by ID.
        /// </summary>
        /// <param name="id">The ID of the car.</param>
        /// <returns>
        /// 200 OK
        /// {CarDto}
        /// </returns>
        /// <example>
        /// GetCar(id) -> {CarDto}
        /// </example>
        public async Task<CarDto?> GetCar(int id)
        {
            var car = await _context.Cars
                .Include(c => c.Owner)
                .FirstOrDefaultAsync(c => c.CarId == id);

            if (car == null)
                return null;

            return new CarDto
            {
                CarId = car.CarId,
                Make = car.Make,
                Model = car.Model,
                Year = car.Year,
                OwnerId = car.Owner.OwnerId,
                OwnerName = car.Owner.FirstName + " " + car.Owner.LastName,
            };
        }

        /// <summary>
        /// Creates a new car and associates it with an owner.
        /// </summary>
        /// <param name="carDto">The car details to create.</param>
        /// <returns>
        /// 201 Created
        /// {ServiceResponse with CreatedId}
        /// </returns>
        /// <example>
        /// CreateCar(carDto) -> {ServiceResponse}
        /// </example>
        public async Task<ServiceResponse?> CreateCar(CarDto carDto)
        {
            ServiceResponse serviceResponse = new();

            Car newCar = new Car
            {
                Make = carDto.Make,
                Model = carDto.Model,
                Year = carDto.Year,
                OwnerId = carDto.OwnerId,
                OwnerName = carDto.OwnerName
            };

            _context.Cars.Add(newCar);
            await _context.SaveChangesAsync();

            serviceResponse.Status = ServiceResponse.ServiceStatus.Created;
            serviceResponse.CreatedId = newCar.CarId;
            return serviceResponse;
        }

        /// <summary>
        /// Updates an existing car's details.
        /// </summary>
        /// <param name="id">The ID of the car to update.</param>
        /// <param name="carDto">The new car details.</param>
        /// <returns>
        /// 200 OK
        /// {ServiceResponse}
        /// </returns>
        /// <example>
        /// UpdateCar(id, carDto) -> {ServiceResponse}
        /// </example>
        public async Task<ServiceResponse> UpdateCar(int id, CarDto carDto)
        {
            ServiceResponse serviceResponse = new();
            var car = await _context.Cars.FindAsync(id);
            if (car == null)
            {
                return null;
            }

            car.Make = carDto.Make;
            car.Model = carDto.Model;
            car.Year = carDto.Year;
            car.OwnerId = carDto.OwnerId;
            car.OwnerName = carDto.OwnerName;

            _context.Entry(car).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            serviceResponse.Status = ServiceResponse.ServiceStatus.Updated;

            return serviceResponse;
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
        /// DeleteCar(id) -> {ServiceResponse}
        /// </example>
        public async Task<ServiceResponse> DeleteCar(int id)
        {
            ServiceResponse serviceResponse = new();
            var car = await _context.Cars.FindAsync(id);
            if (car == null)
            {
                return null;
            }

            _context.Cars.Remove(car);
            await _context.SaveChangesAsync();
            serviceResponse.Status = ServiceResponse.ServiceStatus.Deleted;

            return serviceResponse;
        }
    }
}
