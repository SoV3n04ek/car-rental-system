namespace CarRentalSystem
{
    public record ReturnResult(bool IsSuccess, decimal LateFee, string Message)
    {
        public static ReturnResult Success(decimal lateFee) => new(true, lateFee, "Success");
        public static ReturnResult Failure(string message) => new(false, 0, message);
    }
}
