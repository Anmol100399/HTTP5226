namespace PassionProject.Models.ViewModels
{
    public class OwnerDetails
    {
        //An owner page must have an owner
        public required OwnerDto Owner { get; set; }

        //An owner page can have many cars
        public IEnumerable<CarDto>? Cars { get; set; }
        public IEnumerable<CarDto>? AllCars { get; set; }
    }
}

    