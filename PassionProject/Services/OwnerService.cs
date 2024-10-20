using PassionProject.Data;
using PassionProject.Interface;
using PassionProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using PassionProject.Data.Migrations;
using System;

namespace PassionProject.Services
{
    public class OwnerService : IOwnerService
    {
        private readonly ApplicationDbContext _context;

        public OwnerService(ApplicationDbContext context)
        {
            _context = context;
        }

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

        public async Task<ServiceResponse> CreateOwner(OwnerDto ownerDto)
        {
            ServiceResponse serviceResponse = new();
            // Map the CarDto list to the actual Car entities
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
