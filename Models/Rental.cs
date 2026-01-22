namespace CarRentalSystem
{
    public class Rental
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public Vehicle Vehicle { get; }
        public Client Client { get; }
        public DateTime StartDate { get; }
        public DateTime PlannedEndDate { get; }
        public DateTime? ActualEndDate { get; private set; }
        public decimal TotalPrice { get; }
        public bool IsActive => ActualEndDate == null;

        public Rental(Vehicle vehicle, Client client, int days, DateTime startDate)
        {
            if (!vehicle.IsAvaible)
                throw new InvalidOperationException("Vehicle is not avaible");
            if (!client.IsVerified)
                throw new InvalidOperationException("Client is not verified");

            Vehicle = vehicle;
            Client = client;
            StartDate = startDate;
            PlannedEndDate = StartDate.AddDays(days);
            TotalPrice = vehicle.CalculatePrice(days);

            vehicle.IsAvaible = false;
        }

        public decimal ReturnVehicle(DateTime returnDate)
        {
            if (!IsActive)
                throw new InvalidOperationException("Rental is already closed");

            if (returnDate < StartDate)
                throw new ArgumentException("Return date cannot be before start date");

            ActualEndDate = returnDate;
            Vehicle.IsAvaible = true;

            return CalculateLateFee();
        }

        private decimal CalculateLateFee()
        {
            if (!ActualEndDate.HasValue || ActualEndDate <= PlannedEndDate)
                return 0;

            double lateDaysRaw = (ActualEndDate.Value - PlannedEndDate).TotalDays;
            int lateDays = (int)Math.Ceiling(lateDaysRaw);

            decimal flatPenalty = 100.00m;
            decimal dailyRate = TotalPrice / (PlannedEndDate - StartDate).Days;
            decimal lateDaysPenalty = lateDays * (dailyRate * 1.5m);

            return flatPenalty + lateDaysPenalty;
        }
    }
}
