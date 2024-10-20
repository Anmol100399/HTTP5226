namespace PassionProject.Models.ViewModels
{
    public class CarDetails
    {

        // A car page must have a car
        // FindCar(carid)
        public required CarDto Car { get; set; }

        // A car may have Staffs associated to it
        // ListStaffForCar(carid)
        public IEnumerable<StaffDto>? CarStaffs { get; set; }


        // All owners for this car
        public OwnerDto? CarOwner { get; set; }

    }
}
