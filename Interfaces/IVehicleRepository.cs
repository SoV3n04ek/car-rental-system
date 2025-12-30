namespace CarRentalSystem
{
    public interface IVehicleRepository
    {
        public void Add(Vehicle vehicle);
        Vehicle GetById(string id);
        IEnumerable<Vehicle> GetAvailable();
        void Update(Vehicle vehicle);
        IEnumerable<Vehicle> GetVehicles();
    }
}
