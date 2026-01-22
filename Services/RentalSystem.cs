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

    public Rental RentVehicle(string vehicleId, string clientPhone, int days)
    {
        // find vehicle
        var vehicle = _vehicleRepository.GetById(vehicleId)
            ?? throw new Exception("Vehicle not found");

        // find client
        var client = _clientRepository.GetByPhone(clientPhone)
            ?? throw new Exception("Client not found");

        // create rent
        var rental = new Rental(vehicle, client, days, DateTime.Now);
        _rentalRepository.Add(rental);

        return rental;
    }

    public decimal ReturnVehicle(string vehicleId)
    {
        var rental = _rentalRepository.GetActiveRentalByVehicleId(vehicleId)
            ?? throw new Exception("Active rental not found");
        
        decimal lateFee = rental.ReturnVehicle(DateTime.Now);
        _rentalRepository.Update(rental);

        return lateFee;
    }
}