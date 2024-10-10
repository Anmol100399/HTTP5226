using PassionProject.Data;
using PassionProject.Interface;
using PassionProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using PassionProject.Data.Migrations;

namespace PassionProject.Services
{
    public class CarService : ICarService
    {
            private readonly ApplicationDbContext _context;

            public CarService(ApplicationDbContext context)
            {
                _context = context;
            }

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
                        OwnerName = car.Owner.FirstName
                    }).ToListAsync();
            }

            public async Task<CarDto> GetCar(int id)
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
                    OwnerId = car.OwnerId,
                    OwnerName = car.Owner.FirstName
                };
            }

            public async Task<ServiceResponse> CreateCar(CarDto carDto)
            {
            ServiceResponse serviceResponse = new();
            var owner = await _context.Owners.FindAsync(carDto.OwnerId);
                if (owner == null)
                {
                return null;
                }

                var newCar = new Car
                {
                    Make = carDto.Make,
                    Model = carDto.Model,
                    Year = carDto.Year,
                    OwnerId = carDto.OwnerId,

                };

            _context.Cars.Add(newCar);
            await _context.SaveChangesAsync();

            serviceResponse.Status = ServiceResponse.ServiceStatus.Created;
            serviceResponse.CreatedId = newCar.CarId;
            return serviceResponse;

            }

        public async Task<ServiceResponse> UpdateCarDetails(int id, CarDto carDto)
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

            _context.Entry(car).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            serviceResponse.Status = ServiceResponse.ServiceStatus.Updated;

            return serviceResponse;
        }

        public async Task<ServiceResponse> DeleteCar(int id)
        {

        ServiceResponse serviceResponse = new();
        var car = await _context.Cars.FindAsync(id);
            if (car == null)
            {
            return null;
            }

        _context.Entry(car).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        serviceResponse.Status = ServiceResponse.ServiceStatus.Updated;

        return serviceResponse;
        }
    }
}