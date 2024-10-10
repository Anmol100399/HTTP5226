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
    public class StaffService : IStaffService
    {
        private readonly ApplicationDbContext _context;

        public StaffService(ApplicationDbContext context)
        {
            _context = context;
        }

        // List all staff members
        public async Task<IEnumerable<StaffDto>> ListStaffs()
        {

            var staffs = await _context.Staffs
                .Include(e => e.Cars)
                .ThenInclude(a => a.Owner)
                .ToListAsync();

            return staffs.Select(staff => new StaffDto
            {
                StaffId = staff.StaffId,
                FirstName = staff.FirstName,
                LastName = staff.LastName,
                Cars = staff.Cars.Select(a => new CarDto
                {
                    CarId = a.CarId,
                    Make = a.Make,
                    Model = a.Model,
                    Year = a.Year,
                    OwnerId = a.OwnerId,
                    OwnerName = a.Owner != null
                        ? $"{a.Owner.FirstName} {a.Owner.LastName}"
                        : "Unknown Owner"
                }).ToList()
            }).ToList();
        }

        // Fetch a specific staff member by ID
        public async Task<StaffDto> GetStaff(int id)
        {
            var staff = await _context.Staffs
                .Include(e => e.Cars)
                .ThenInclude(a => a.Owner)
                .FirstOrDefaultAsync(e => e.StaffId == id);

            if (staff == null)
                return null;
            return new StaffDto
            {
                StaffId = staff.StaffId,
                FirstName = staff.FirstName,
                LastName = staff.LastName,
                Cars = staff.Cars.Select(a => new CarDto
                {
                    CarId = a.CarId,
                    Make = a.Make,
                    Model = a.Model,
                    Year = a.Year, 
                    OwnerId = a.OwnerId,
                    OwnerName = $"{a.Owner.FirstName} {a.Owner.LastName}"
                }).ToList()
            };
        }
        // Create a new staff member
        public async Task<ServiceResponse> CreateStaff(StaffDto staffDto)
        {
            ServiceResponse serviceResponse = new();

            // Validate and fetch Artworks
            var cars = await _context.Cars
                .Where(a => staffDto.Cars.Select(adto => adto.CarId).Contains(a.CarId))
                .ToListAsync();

            if (cars.Count != staffDto.Cars.Count)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add("Some cars provided do not exist. Please check carIds.");
                return serviceResponse;
            }

            // Create a new Staff
            var staff = new Staff
            {
                StaffId = staffDto.StaffId,
                FirstName = staffDto.FirstName,
                LastName = staffDto.LastName,
                Cars = cars
            };

            _context.Staffs.Add(staff);
            await _context.SaveChangesAsync();

            serviceResponse.Status = ServiceResponse.ServiceStatus.Created;
            serviceResponse.CreatedId = staff.StaffId;
            serviceResponse.Messages.Add("Staff created successfully.");

            return serviceResponse;
        }

        // Update an existing staff member
        public async Task<ServiceResponse> UpdateStaff(int id, StaffDto staffDto)
        {
            ServiceResponse serviceResponse = new();

            var existingStaff = await _context.Staffs
                .Include(e => e.Cars)
                .FirstOrDefaultAsync(e => e.StaffId == id);

            if (existingStaff == null)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.NotFound;
                serviceResponse.Messages.Add("Staff not found.");
                return serviceResponse;
            }

            existingStaff.FirstName = staffDto.FirstName;
            existingStaff.LastName = staffDto.LastName; 

            var cars = await _context.Cars
                    .Where(a => staffDto.Cars.Select(adto => adto.CarId).Contains(a.CarId))
                    .ToListAsync();

                if (cars.Count != staffDto.Cars.Count)
                {
                    serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                    serviceResponse.Messages.Add("Some Cars provided do not exist. Please check carId.");
                    return serviceResponse;
                }

                existingStaff.Cars = cars;
            
            await _context.SaveChangesAsync();

            serviceResponse.Status = ServiceResponse.ServiceStatus.Updated;
            serviceResponse.Messages.Add("Staff updated successfully.");

            return serviceResponse;
        }

        // Create a new staff member
        public async Task<ActionResult<Staff>> CreateStaff(Staff staff)
        {
            _context.Staffs.Add(staff);
            await _context.SaveChangesAsync();

            return new CreatedAtActionResult("GetStaff", "StaffAPI", new { id = staff.StaffId }, staff); // Adjust this if needed
        }

        // Delete a staff member by ID
        public async Task<ServiceResponse> DeleteStaff(int id)
        {
            ServiceResponse response = new();

            var staff = await _context.Staffs.FindAsync(id);
            if (staff == null)
            {
                response.Status = ServiceResponse.ServiceStatus.NotFound;
                response.Messages.Add("Staff not found.");
                return response;
            }

            _context.Staffs.Remove(staff);
            await _context.SaveChangesAsync();

            response.Status = ServiceResponse.ServiceStatus.Deleted;
            response.Messages.Add("Staff deleted successfully.");

            return response;
        }

    }
}
