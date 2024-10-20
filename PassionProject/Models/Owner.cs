using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace PassionProject.Models
{
    public class Owner
    {
        /// <summary>
        /// Primary Key for the Owner entity.
        /// </summary>
        [Key]
        public int OwnerId { get; set; }

        /// <summary>
        /// First name of the owner.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Last name of the owner.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Contact information of the owner (e.g., phone number or email).
        /// </summary>
        public string Contact { get; set; }

        /// <summary>
        /// Collection of cars owned by the owner. An owner can have many cars.
        /// </summary>
        public ICollection<Car> Cars { get; set; }
    }

    public class OwnerDto
    {
        /// <summary>
        /// ID of the owner for Data Transfer Object purposes.
        /// </summary>
        public int OwnerId { get; set; }

        /// <summary>
        /// First name of the owner for Data Transfer Object purposes.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Last name of the owner for Data Transfer Object purposes.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Contact of the owner for Data Transfer Object purposes.
        /// </summary>
        public string Contact { get; set; }
        // A collection of artwork IDs associated with this artist
        public List<int> CarId { get; set; } = new List<int>();

        // Optionally, you can also include a collection of detailed ArtworkDto objects
        public List<CarDto> Cars { get; set; } = new List<CarDto>();
    }
}
