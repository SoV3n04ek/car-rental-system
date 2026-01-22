namespace CarRentalSystem
{
    public class Client
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string PhoneNumber { get; private set; }
        public string DrivingLicenseNumber { get; init; }
        public DateTime LicenseExpiryDate { get; init; }
        public DateTime RegistrationDate { get; init; } = DateTime.Now;
        public bool IsVerified { get; private set; }

        private Client(string firstName, string lastName, string phone, string license, DateTime expiry)
        {
            if (!DataValidator.ValidateName(firstName))
                throw new ArgumentException("Invalid first name");
            
            if (!DataValidator.ValidateName(lastName)) 
                throw new ArgumentException("Invalid last name");
            
            if (!DataValidator.ValidatePhone(phone)) 
                throw new ArgumentException("Invalid phone number");
            
            if (expiry <= DateTime.Now.AddMonths(1))
                throw new ArgumentException("License expires too soon");

            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phone;
            DrivingLicenseNumber = license;
            LicenseExpiryDate = expiry;
            IsVerified = true;
        }

        public static Client Register(string firstName, string lastName, string phone, string license, DateTime expiry)
        {
            return new Client(firstName, lastName, phone, license, expiry);
        }

        public void UpdateContact(string newPhone)
        {
            if (!DataValidator.ValidatePhone(newPhone)) 
                throw new ArgumentException("Invalid phone");
            PhoneNumber = newPhone;
        }
    }
}