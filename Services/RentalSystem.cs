namespace CarRentalSystem;

public class RentalSystem : IRentalSystem
{
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IClientRepository _clientRepository;
    private readonly IRentalRepository _rentalRepository;

    public RentalSystem(IVehicleRepository vehicleRepo,
        IClientRepository clientRepo,
        IRentalRepository rentalRepo)
    {
        _vehicleRepository = vehicleRepo;
        _clientRepository = clientRepo;
        _rentalRepository = rentalRepo;
    }

    public void AddVehicle(Vehicle vehicle)
    {
        _vehicleRepository.Add(vehicle);
    }

    public IEnumerable<Vehicle> GetAvaibleVehicles()
    {
        return _vehicleRepository.GetAvailable();
    }

    public IEnumerable<Vehicle> GetVehicles()
    {
        return _vehicleRepository.GetVehicles();
    }

    public RentalResult RentVehicle(string vehicleId, string clientPhone, int days)
    {
        // find vehicle
        var vehicle = _vehicleRepository.GetById(vehicleId);
        if (vehicle == null || !vehicle.IsAvaible)
            return RentalResult.Failure("Vehicle not available");

        // find client
        var client = _clientRepository.GetByPhone(clientPhone);
        if (client == null)
            return RentalResult.Failure("Client not found");
        if (!client.IsVerified)
            return RentalResult.Failure("Client not verified");

        // create rent
        var rental = new Rental(vehicle, client, days);
        _rentalRepository.Add(rental);

        return RentalResult.Success(rental);
    }

    public ReturnResult ReturnVehicle(string vehicleId)
    {
        var rental = _rentalRepository.GetActiveRentalByVehicleId(vehicleId);
        if (rental == null) 
            return ReturnResult.Failure("Active rental not found for this vehicle");

        decimal lateFee = rental.ReturnVehicle();
        _rentalRepository.Update(rental);

        return ReturnResult.Success(lateFee);
    }
}