using PassionProject.Data;
using PassionProject.Interface;
using PassionProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PassionProject.Services
{
    public class StaffService : IStaffService
    {
        private readonly ApplicationDbContext _context;

        public StaffService(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Lists all staff members.
        /// </summary>
        /// <returns>A collection of staff DTOs.</returns>
        public async Task<IEnumerable<StaffDto>> ListStaffs()
        {
            return await _context.Staffs.Select(staff => new StaffDto
            {
                StaffId = staff.StaffId,
                FirstName = staff.FirstName,
                LastName = staff.LastName,
                Contact = staff.Contact,
                Position = staff.Position
            }).ToListAsync();
        }

        /// <summary>
        /// Fetches a specific staff member by ID.
        /// </summary>
        /// <param name="id">The ID of the staff member.</param>
        /// <returns>The staff DTO or null if not found.</returns>
        public async Task<StaffDto?> GetStaff(int id)
        {
            var staff = await _context.Staffs
                .Include(e => e.Cars)
                .ThenInclude(a => a.Owner)
                .FirstOrDefaultAsync(e => e.StaffId == id);

            if (staff == null) return null;

            return new StaffDto
            {
                StaffId = staff.StaffId,
                FirstName = staff.FirstName,
                LastName = staff.LastName,
                Position = staff.Position,
                Contact = staff.Contact,
                Cars = staff.Cars.Select(a => new CarDto
                {
                    CarId = a.CarId,
                    Make = a.Make,
                    Model = a.Model,
                    Year = a.Year,
                    OwnerId = a.Owner?.OwnerId ?? 0,
                    OwnerName = a.Owner != null ? $"{a.Owner.FirstName} {a.Owner.LastName}" : "Unknown Owner"
                }).ToList()
            };
        }

        /// <summary>
        /// Creates a new staff member.
        /// </summary>
        /// <param name="staffDto">The staff DTO containing the details to create a new staff member.</param>
        /// <returns>A response indicating the result of the creation.</returns>
        public async Task<ServiceResponse> CreateStaff(StaffDto staffDto)
        {
            ServiceResponse serviceResponse = new();

            if (staffDto.Cars == null || !staffDto.Cars.Any())
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add("No cars were selected.");
                return serviceResponse;
            }

            // Validate and fetch Cars
            var carIds = staffDto.Cars.Select(c => c.CarId).ToList();
            var cars = await _context.Cars.Where(c => carIds.Contains(c.CarId)).ToListAsync();

            if (!cars.Any())
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add("Selected cars not found.");
                return serviceResponse;
            }

            // Create a new Staff
            var staff = new Staff
            {
                FirstName = staffDto.FirstName,
                LastName = staffDto.LastName,
                Position = staffDto.Position,
                Contact = staffDto.Contact,
                Cars = cars
            };

            _context.Staffs.Add(staff);
            await _context.SaveChangesAsync();

            serviceResponse.Status = ServiceResponse.ServiceStatus.Created;
            serviceResponse.CreatedId = staff.StaffId;
            serviceResponse.Messages.Add("Staff created successfully.");

            return serviceResponse;
        }

        /// <summary>
        /// Updates an existing staff member.
        /// </summary>
        /// <param name="id">The ID of the staff member to update.</param>
        /// <param name="staffDto">The updated staff DTO.</param>
        /// <returns>A response indicating the result of the update.</returns>
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

            if (staffDto.Cars != null && staffDto.Cars.Any())
            {
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
            }

            await _context.SaveChangesAsync();

            serviceResponse.Status = ServiceResponse.ServiceStatus.Updated;
            serviceResponse.Messages.Add("Staff updated successfully.");

            return serviceResponse;
        }

        /// <summary>
        /// Deletes a staff member by ID.
        /// </summary>
        /// <param name="id">The ID of the staff member to delete.</param>
        /// <returns>A response indicating the result of the deletion.</returns>
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
