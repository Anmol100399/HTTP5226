using PassionProject.Data;
using PassionProject.Interface;
using PassionProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace PassionProject.Services
{
    public class OwnerService : IOwnerService
    {
        private readonly ApplicationDbContext _context;

        public OwnerService(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Lists all owners.
        /// </summary>
        /// <returns>
        /// 200 OK
        /// [{OwnerDto}, {OwnerDto}, ...]
        /// </returns>
        /// <example>
        /// GET: api/Owner/List -> [{OwnerDto}, {OwnerDto}, ...]
        /// </example>
        public async Task<IEnumerable<OwnerDto>> ListOwners()
        {
            return await _context.Owners.Select(owner => new OwnerDto
            {
                OwnerId = owner.OwnerId,
                FirstName = owner.FirstName,
                LastName = owner.LastName,
                Contact = owner.Contact
            }).ToListAsync();
        }

        /// <summary>
        /// Finds and returns a specific owner by ID.
        /// </summary>
        /// <param name="id">The ID of the owner to retrieve.</param>
        /// <returns>
        /// 200 OK
        /// {OwnerDto}
        /// </returns>
        /// <example>
        /// GET: api/Owner/{id} -> {OwnerDto}
        /// </example>
        public async Task<OwnerDto> FindOwner(int id)
        {
            var owner = await _context.Owners.FindAsync(id);
            if (owner == null)
            {
                return null;
            }
            return new OwnerDto
            {
                OwnerId = owner.OwnerId,
                FirstName = owner.FirstName,
                LastName = owner.LastName,
                Contact = owner.Contact
            };
        }

        /// <summary>
        /// Creates a new owner and associates cars.
        /// </summary>
        /// <param name="ownerDto">The data transfer object containing owner details and cars.</param>
        /// <returns>
        /// 201 Created
        /// {ServiceResponse}
        /// </returns>
        /// <example>
        /// POST: api/Owner/Create -> {ServiceResponse}
        /// </example>
        public async Task<ServiceResponse> CreateOwner(OwnerDto ownerDto)
        {
            ServiceResponse serviceResponse = new();
            // Map the CarDto list to actual Car entities
            var carEntities = await _context.Cars
                .Where(c => ownerDto.Cars.Select(dto => dto.CarId).Contains(c.CarId))
                .ToListAsync();

            var owner = new Owner
            {
                FirstName = ownerDto.FirstName,
                LastName = ownerDto.LastName,
                OwnerId = ownerDto.OwnerId,
                Contact = ownerDto.Contact,
                Cars = carEntities
            };

            _context.Owners.Add(owner);
            await _context.SaveChangesAsync();
            serviceResponse.Status = ServiceResponse.ServiceStatus.Created;
            serviceResponse.CreatedId = owner.OwnerId;
            return serviceResponse;
        }

        /// <summary>
        /// Updates an existing owner by ID.
        /// </summary>
        /// <param name="id">The ID of the owner to update.</param>
        /// <param name="ownerDto">The updated data transfer object containing owner details.</param>
        /// <returns>
        /// 200 OK
        /// {ServiceResponse}
        /// </returns>
        /// <example>
        /// PUT: api/Owner/{id} -> {ServiceResponse}
        /// </example>
        public async Task<ServiceResponse> UpdateOwner(int id, OwnerDto ownerDto)
        {
            ServiceResponse serviceResponse = new();
            var owner = await _context.Owners.FindAsync(id);
            if (owner == null)
                return null;

            owner.FirstName = ownerDto.FirstName;
            owner.LastName = ownerDto.LastName;
            owner.Contact = ownerDto.Contact;
            owner.OwnerId = ownerDto.OwnerId;

            _context.Entry(owner).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            serviceResponse.Status = ServiceResponse.ServiceStatus.Updated;
            return serviceResponse;
        }

        /// <summary>
        /// Deletes a specific owner by ID.
        /// </summary>
        /// <param name="id">The ID of the owner to delete.</param>
        /// <returns>
        /// 200 OK
        /// {ServiceResponse}
        /// </returns>
        /// <example>
        /// DELETE: api/Owner/{id} -> {ServiceResponse}
        /// </example>
        public async Task<ServiceResponse> DeleteOwner(int id)
        {
            ServiceResponse serviceResponse = new();
            var owner = await _context.Owners.FindAsync(id);
            if (owner == null)
                return null;

            _context.Owners.Remove(owner);
            await _context.SaveChangesAsync();
            serviceResponse.Status = ServiceResponse.ServiceStatus.Deleted;
            return serviceResponse;
        }
    }
}
