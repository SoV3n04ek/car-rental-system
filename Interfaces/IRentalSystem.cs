namespace CarRentalSystem
{
    public interface IRentalSystem
    {
        void AddVehicle(Vehicle vehicle);
        IEnumerable<Vehicle> GetVehicles();
        IEnumerable<Vehicle> GetAvaibleVehicles();
        Rental RentVehicle(string vehicleId, string clientPhone, int days);
        decimal ReturnVehicle(string vehicleId);
    }
}