using CarRentalSystem;

var vehicleRepo = new InMemoryVehicleRepository();
var clientRepo = new InMemoryClientRepository();
var rentalRepo = new InMemoryRentalRepository();

vehicleRepo.Add(VehicleFactory.CreateVehicle("car", "C001", "Toyota Camry", 350));
vehicleRepo.Add(VehicleFactory.CreateVehicle("car", "C002", "Tesla Model 3", 500));
vehicleRepo.Add(VehicleFactory.CreateVehicle("car", "C003", "BMW M4", 750));
vehicleRepo.Add(VehicleFactory.CreateVehicle("car", "C004", "Volvo XC60", 450));
vehicleRepo.Add(VehicleFactory.CreateVehicle("car", "C005", "Mazda CX5", 350));

RentalSystem rentalSystem = new RentalSystem(vehicleRepo, clientRepo, rentalRepo);

ConsoleMenu menu = new ConsoleMenu(rentalSystem, clientRepo);
menu.Start();