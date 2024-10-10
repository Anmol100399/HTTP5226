using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PassionProject.Interface;
using PassionProject.Models;

namespace PassionProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StaffAPIController : ControllerBase
    {
        private readonly IStaffService _staffService;

        // Dependency injection of service interfaces
        public StaffAPIController(IStaffService staffService)
        {
            _staffService = staffService;
        }

        /// <summary>
        /// This endpoint returns a list of all staff members in the system.
        /// </summary>
        /// <returns>
        /// 200 OK
        /// [{StaffDto}, {StaffDto}, ...]
        /// </returns>
        /// <example>
        /// GET: api/StaffAPI/List ->
        /// [{"StaffId":1,"FirstName":"John","LastName":"Doe","Position":"Manager","Contact":"John@123gmail.com"},
        /// {"StaffId":2,"FirstName":"Jane","LastName":"Doe","Position":"Salesperson","Contact":"Jane123@gmail.com"}]
        /// </example>
        [HttpGet(template: "List")]
        public async Task<IEnumerable<StaffDto>> ListStaffs()
        {
            return await _staffService.ListStaffs();
        }

        /// <summary>
        /// This endpoint finds a specific staff member by ID.
        /// </summary>
        /// <param name="id">The Staff ID</param>
        /// <returns>
        /// 200 OK
        /// {StaffDto}
        /// or
        /// 404 Not Found
        /// </returns>
        /// <example>
        /// GET: api/StaffAPI/Find/1 -> {StaffDto}
        /// {"StaffId":1,"FirstName":"John","LastName":"Doe","Position":"Manager","Contact":"John123@gmail.com"}
        /// </example>
        [HttpGet(template: "Find/{id}")]
        public async Task<StaffDto> GetStaff(int id)
        {
            return await _staffService.GetStaff(id);
        }

        /// <summary>
        /// This endpoint updates an existing staff member's information.
        /// </summary>
        /// <param name="id">Staff ID</param>
        /// <param name="staffDto">Updated staff information</param>
        /// <returns>
        /// 404 Not Found
        /// or 
        /// 400 Bad Request
        /// </returns>
        /// <example>
        /// PUT: api/StaffAPI/Update/1
        /// Request Headers: Content-Type: application/json
        /// Request Body: {StaffDto}
        /// -> 
        /// Response Code: 204 No Content
        /// </example>
        [HttpPut("{id}")]
        public async Task<ServiceResponse> UpdateStaff(int id, StaffDto staffDto)
        {
            return await _staffService.UpdateStaff(id, staffDto);
        }

        /// <summary>
        /// Adds a new staff member.
        /// </summary>
        /// <param name="staffDto">The required information to add the staff member (StaffId, FirstName, LastName, etc.)</param>
        /// <returns>
        /// 201 Created
        /// Location: api/StaffAPI/Find/{StaffId}
        /// {StaffDto}
        /// or
        /// 404 Not Found
        /// </returns>
        /// <example>
        /// POST: api/StaffAPI/Add
        /// Request Headers: Content-Type: application/json
        /// Request Body: {StaffDto}
        /// -> 
        /// Response Code: 201 Created
        /// Response Headers: Location: api/StaffAPI/Find/{StaffId}
        /// </example>
        [HttpPost(template: "Add")]
        public async Task<ServiceResponse> CreateStaff(StaffDto staffDto)
        {
            return await _staffService.CreateStaff(staffDto);
        }

        /// <summary>
        /// Deletes a staff member by ID.
        /// </summary>
        /// <param name="id">The ID of the staff member to delete</param>
        /// <returns>
        /// 204 No Content
        /// or
        /// 404 Not Found
        /// </returns>
        /// <example>
        /// DELETE: api/StaffAPI/Delete/1
        /// -> 
        /// Response Code: 204 No Content
        /// </example>
        [HttpDelete("{id}")]
        public async Task<ServiceResponse> DeleteStaff(int id)
        {
            return await _staffService.DeleteStaff(id);
        }
    }
}