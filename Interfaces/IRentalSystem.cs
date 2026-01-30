namespace CarRentalSystem
{
    public interface IRentalSystem
    {
        void AddVehicle(Vehicle vehicle);
        IEnumerable<Vehicle> GetVehicles();
        IEnumerable<Vehicle> GetAvaibleVehicles();
        Rental RentVehicle(string vehicleId, string clientPhone, int days);
        decimal ReturnVehicle(string vehicleId);
        void RemoveClient(string phone);
        void RemoveVehicle(string id);
        IEnumerable<Client> GetClients();
        IEnumerable<Rental> GetRentals();
    }
}