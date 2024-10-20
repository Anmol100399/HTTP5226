using System.ComponentModel.DataAnnotations;

namespace PassionProject.Models
{
    public class Car
    {
        /// <summary>
        /// Primary Key for the Car entity.
        /// </summary>
        [Key]
        public int CarId { get; set; }

        /// <summary>
        /// The manufacturer of the car (e.g., Toyota, Camry).
        /// </summary>
        public string Make { get; set; }

        /// <summary>
        /// The model of the car (e.g., Camry, Mustang).
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// The year the car was manufactured.
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// Foreign key for the Owner entity. A car belongs to one owner.
        /// </summary>
        public int OwnerId { get; set; }



        /// <summary>
        /// Name of the car's owner for Data Transfer Object purposes.
        /// </summary>
        public string OwnerName { get; set; }


        /// <summary>
        /// Navigation property to the Owner entity.
        /// </summary>
        public virtual Owner Owner { get; set; }

        /// <summary>
        /// Collection of staff members associated with the car. A car can have many staff members.
        /// </summary>
        public ICollection<Staff>? Staffs { get; set; }
    }

    public class CarDto
    {
        /// <summary>
        /// ID of the car for Data Transfer Object purposes.
        /// </summary>
        public int CarId { get; set; }

        /// <summary>
        /// The manufacturer of the car for Data Transfer Object purposes.
        /// </summary>
        public string Make { get; set; }

        /// <summary>
        /// The model of the car for Data Transfer Object purposes.
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// The year the car was manufactured for Data Transfer Object purposes.
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// ID of the car's owner for Data Transfer Object purposes.
        /// </summary>
        public int OwnerId { get; set; }

        /// <summary>
        /// Name of the car's owner for Data Transfer Object purposes.
        /// </summary>
        public string OwnerName { get; set; }

    }
}
