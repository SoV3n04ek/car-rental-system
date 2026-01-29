namespace CarRentalSystem
{
    public interface IRentalRepository
    {
        Rental GetActiveRentalByVehicleId(string vehicleId);
        void Add(Rental rental);
        void Update(Rental rental);
        IEnumerable<Rental> GetAllRentals();
    }
}
