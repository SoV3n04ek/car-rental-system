namespace CarRentalSystem
{
    public class RentalFactory
    {
        public static Rental CreateRental(Vehicle vehicle, Client client, int days)
        {
            if (!vehicle.IsAvaible) throw new Exception("Car is busy");
            if (!client.IsVerified) throw new Exception("Client not verified");

            return new Rental(vehicle, client, days);
        }
    }
}
