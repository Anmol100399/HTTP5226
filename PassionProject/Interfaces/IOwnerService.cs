using PassionProject.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PassionProject.Interface
{
    public interface IOwnerService
    {
        // Fetch all owners
        Task<IEnumerable<OwnerDto>> ListOwners();

        // Fetch a specific owner by ID
        Task<OwnerDto> FindOwner(int id);

        // Update an existing owner
        Task<ServiceResponse> UpdateOwner(int id, OwnerDto ownerDto);

        // Create a new owner
        Task<ServiceResponse> CreateOwner(OwnerDto ownerDto);

        // Delete an owner by ID
        Task<ServiceResponse> DeleteOwner(int id);

        // related methods

        // Task<IEnumerable<OwnerDto>> ListOwnersForCar(int id);
    }
}
