namespace CarRentalSystem
{
    public abstract class Vehicle
    {
        public string Id { get; set; }
        public string Model { get; set; }
        public decimal BasePricePerDay { get; }
        internal bool IsAvaible { get; set; } = true;
        public bool IsDeleted { get; private set; } = false;

        public void MarkAsDeleted() => IsDeleted = true;

        protected Vehicle(string id, string model, decimal price)
        {
            Id = id;
            Model = model;
            BasePricePerDay = price;
        }

        public abstract decimal CalculatePrice(int days);

        public override string ToString()
        {
            string status = IsAvaible ? "Free" : "Ordered";
            return $"{status} ID: {Id} | {Model} | Price: {BasePricePerDay:C}$/day";
        }
    }
}
