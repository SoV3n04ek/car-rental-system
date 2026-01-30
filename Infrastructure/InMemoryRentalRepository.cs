namespace CarRentalSystem
{
    public class InMemoryRentalRepository : IRentalRepository
    {
        private readonly List<Rental> _rentals = new();
        public void Add(Rental rental) => _rentals.Add(rental);
        public void Update(Rental rental) { }
        public Rental GetActiveRentalByVehicleId(string vehicleId)
        {
            return _rentals.FirstOrDefault(r => r.Vehicle.Id == vehicleId
            && r.IsActive);
        }

        public IEnumerable<Rental> GetAll() => _rentals;
    }
}
