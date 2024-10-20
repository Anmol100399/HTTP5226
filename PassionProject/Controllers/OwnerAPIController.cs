using Microsoft.AspNetCore.Mvc;
using PassionProject.Interface;
using PassionProject.Models;

namespace PassionProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OwnerController : ControllerBase
    {
        private readonly IOwnerService _ownerService;

        public OwnerController(IOwnerService ownerService)
        {
            _ownerService = ownerService;
        }

        /// <summary>
        /// Returns a list of Owners.
        /// </summary>
        /// <returns>
        /// 200 OK
        /// [{OwnerDto}, {OwnerDto}, ...]
        /// </returns>
        /// <example>
        /// GET: api/Owner/List -> [{OwnerDto}, {OwnerDto}, ...]
        /// </example>
        [HttpGet(template: "List")]
        public async Task<IEnumerable<OwnerDto>> ListOwners()
        {
            return await _ownerService.ListOwners();
        }

        /// <summary>
        /// Finds a specific Owner by ID.
        /// </summary>
        /// <param name="id">The ID of the owner to retrieve.</param>
        /// <returns>
        /// 200 OK
        /// {OwnerDto}
        /// </returns>
        /// <example>
        /// GET: api/Owner/{id} -> {OwnerDto}
        /// </example>
        [HttpGet("{id}")]
        public async Task<OwnerDto> FindOwner(int id)
        {
            return await _ownerService.FindOwner(id);
        }

        /// <summary>
        /// Updates an existing Owner's details by ID.
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
        [HttpPut("{id}")]
        public async Task<ServiceResponse> UpdateOwner(int id, OwnerDto ownerDto)
        {
            return await _ownerService.UpdateOwner(id, ownerDto);
        }

        /// <summary>
        /// Creates a new Owner.
        /// </summary>
        /// <param name="ownerDto">The data transfer object containing owner details.</param>
        /// <returns>
        /// 201 Created
        /// {ServiceResponse}
        /// </returns>
        /// <example>
        /// POST: api/Owner/Add -> {ServiceResponse}
        /// </example>
        [HttpPost(template: "Add")]
        public async Task<ServiceResponse> CreateOwner(OwnerDto ownerDto)
        {
            return await _ownerService.CreateOwner(ownerDto);
        }

        /// <summary>
        /// Deletes an Owner by ID.
        /// </summary>
        /// <param name="id">The ID of the owner to delete.</param>
        /// <returns>
        /// 200 OK
        /// {ServiceResponse}
        /// </returns>
        /// <example>
        /// DELETE: api/Owner/{id} -> {ServiceResponse}
        /// </example>
        [HttpDelete("{id}")]
        public async Task<ServiceResponse> DeleteOwner(int id)
        {
            return await _ownerService.DeleteOwner(id);
        }
    }
}
