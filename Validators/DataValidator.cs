namespace CarRentalSystem
{
    public static class DataValidator
    {
        public static bool ValidateName(string name)
        {
            return !string.IsNullOrWhiteSpace(name) 
                && name.Length >= 2 
                && char.IsUpper(name[0]);
        }

        public static bool ValidatePhone(string phone)
        {
            return !string.IsNullOrWhiteSpace(phone) &&
                phone.Length >= 9 &&
                phone.All(c => char.IsDigit(c) || c == '+');
        }

        public static bool IsNotEmpty(string text)
        {
            return !string.IsNullOrWhiteSpace(text);   
        }
    }
}
