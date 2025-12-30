namespace CarRentalSystem
{
    public static class ClientFactory
    {
        public static Client Create(string firstName, string lastName, string phone, string license, DateTime licenseExpiryDate)
        {
            return new Client(firstName, lastName, phone, license, licenseExpiryDate);
        }
    }
}
