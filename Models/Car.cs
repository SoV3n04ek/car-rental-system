namespace CarRentalSystem
{
    public class Car : Vehicle
    {
        public Car(string id, string model, decimal pricePerDay)
            : base(id, model, pricePerDay)
        {

        }

        public override decimal CalculatePrice(int days)
        {
            return BasePricePerDay * days;
        }
    }
}
