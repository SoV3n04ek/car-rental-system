namespace CarRentalSystem
{
    public class Rental
    {
        public Guid Id { get; }
        public Vehicle Vehicle { get; }
        public Client Client { get; }
        public DateTime StartDate { get; }
        public DateTime PlannedEndDate { get; }
        public DateTime? ActualEndDate { get; private set; }
        public decimal TotalPrice { get; }
        public bool IsActive => ActualEndDate == null;

        public Rental(Vehicle vehicle, Client client, int days)
        {
            // TODO: refactor to fabric method or Builder
            if (!vehicle.IsAvaible)
                throw new InvalidOperationException("Vehicle is not avaible");
            if (!client.IsVerified)
                throw new InvalidOperationException("Client is not verified");

            Id = Guid.NewGuid();
            Vehicle = vehicle;
            Client = client;
            StartDate = DateTime.Now;
            PlannedEndDate = StartDate.AddDays(days);
            TotalPrice = vehicle.CalculatePrice(days);

            vehicle.IsAvaible = false;
        }

        public decimal ReturnVehicle()
        {
            if (!IsActive)
            {
                throw new InvalidOperationException("Rental is already closed");
            }

            ActualEndDate = DateTime.Now;
            Vehicle.IsAvaible = true;

            return CalculateLateFee();
        }

        private decimal CalculateLateFee()
        {
            if (ActualEndDate <= PlannedEndDate)
                return 0;

            var lateDays = (ActualEndDate.Value - PlannedEndDate).Days;
            return TotalPrice * 0.1m * lateDays; // 10% for each days
        }
    }
}
