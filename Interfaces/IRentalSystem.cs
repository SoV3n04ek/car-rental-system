namespace CarRentalSystem
{
    public interface IRentalSystem
    {
        void AddVehicle(Vehicle vehicle);
        IEnumerable<Vehicle> GetVehicles();
        IEnumerable<Vehicle> GetAvaibleVehicles();
        RentalResult RentVehicle(string vehicleId, string clientPhone, int days);
        ReturnResult ReturnVehicle(string vehicleId);
    }
}