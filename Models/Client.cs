namespace CarRentalSystem
{
    public class Client
    {
        public Guid Id { get; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string PhoneNumber { get; private set; }
        public string DrivingLicenseNumber { get; }
        public DateTime LicenseExpiryDate { get; }
        public DateTime RegistrationDate { get; }      
        public bool IsVerified { get; private set; }

        public Client(
            string firstName, 
            string lastName,
            string phoneNumber, 
            string drivingLicenseNumber,
            DateTime licenseExpiryDate)
        {
            // TODO: refactor to fabric method or Builder 
            if (!ValidateName(firstName))
                throw new ArgumentException("Invalid first name");
            if (!ValidateName(lastName))
                throw new ArgumentException("Invalid last name");
            if (!ValidatePhone(phoneNumber))
                throw new ArgumentException("Invalid phone number");
            if (string.IsNullOrWhiteSpace(drivingLicenseNumber))
                throw new ArgumentException("Invalid driving license number");
            if (licenseExpiryDate <= DateTime.Now)
                throw new ArgumentException("License has expired");

            Id = Guid.NewGuid();
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            DrivingLicenseNumber = drivingLicenseNumber;
            LicenseExpiryDate = licenseExpiryDate;
            RegistrationDate = DateTime.Now;
            IsVerified = licenseExpiryDate > DateTime.Now.AddMonths(1);
        }

        private static bool ValidateName(string name)
        {
            return !string.IsNullOrWhiteSpace(name)
                && name.Length >= 2
                && char.IsUpper(name[0]);
        }

        private static bool ValidatePhone(string phone)
        {
            return !string.IsNullOrWhiteSpace(phone)
                && phone.Length >= 9
                && phone.All(c => char.IsDigit(c) || c == '+');
        }
    }
}
