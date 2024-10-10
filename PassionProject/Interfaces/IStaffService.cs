using PassionProject.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PassionProject.Interface
{
    public interface IStaffService
    {
        // Fetch all staff members
        Task<IEnumerable<StaffDto>> ListStaffs();

        // Fetch a specific staff member by ID
        Task<StaffDto> GetStaff(int id);

        // Update an existing staff member
        Task<ServiceResponse> UpdateStaff(int id, StaffDto staffDto);

        // Create a new staff member
        Task<ServiceResponse> CreateStaff(StaffDto staffDto);

        // Delete a staff member by ID
        Task<ServiceResponse> DeleteStaff(int id);
    }
}
