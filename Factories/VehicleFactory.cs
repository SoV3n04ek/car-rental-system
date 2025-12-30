namespace CarRentalSystem
{
    public class VehicleFactory
    {
        public static Vehicle CreateVehicle(string type, 
            string id, string model, decimal price)
        {
            return type.ToLower() switch
            {
                "car" => new Car(id, model, price),
                _ => throw new ArgumentException("Unknown vehicle type")
            };
        }
    }
}
