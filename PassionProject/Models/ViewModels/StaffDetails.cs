namespace PassionProject.Models.ViewModels
{
    public class StaffDetails
    {
        //A staff page must have a staff
        public required StaffDto Staff { get; set; }

        //A staff page can manage many cars
        public IEnumerable<CarDto>? StaffCars { get; set; }
    }
}
