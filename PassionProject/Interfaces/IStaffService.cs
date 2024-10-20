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

        // related methods


        //Task<IEnumerable<StaffDto>> ListStaffsForCar(int id);
        // Task<ServiceResponse> ListStaffsForCar(int categoryId, int productId);

        // Task<ServiceResponse> UnlinkStaffsForCar(int categoryId, int productId);
    }
}
