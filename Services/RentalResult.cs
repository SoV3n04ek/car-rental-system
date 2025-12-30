namespace CarRentalSystem;

public record RentalResult(
    bool IsSuccess,
    Rental? Rental,
    string ErrorMessage)
{
    public static RentalResult Success(Rental rental) => new(true, rental, "");
    public static RentalResult Failure(string message) => new(false, null, message);
}