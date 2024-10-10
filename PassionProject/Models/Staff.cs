using System.ComponentModel.DataAnnotations;

namespace PassionProject.Models
{
    public class Staff
    {
        /// <summary>
        /// Primary Key for the Staff entity.
        /// </summary>
        [Key]
        public int StaffId { get; set; }

        /// <summary>
        /// First name of the staff member.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Last name of the staff member.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Position or role of the staff member (e.g., Salesperson, Manager).
        /// </summary>
        public string Position { get; set; }

        /// <summary>
        /// Contact information of the staff member (e.g., phone number or email).
        /// </summary>
        public string Contact { get; set; }

        /// <summary>
        /// Collection of cars that the staff member manages. A staff member can manage many cars.
        /// </summary>
        public ICollection<Car>? Cars { get; set; }
    }

    public class StaffDto
    {
        /// <summary>
        /// ID of the staff member for Data Transfer Object purposes.
        /// </summary>
        public int StaffId { get; set; }

        /// <summary>
        /// First name of the staff member for Data Transfer Object purposes.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Last name of the staff member for Data Transfer Object purposes.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// List of cars that the staff member manages for Data Transfer Object purposes.
        /// </summary>
        public List<CarDto> Cars { get; set; }
    }
}
