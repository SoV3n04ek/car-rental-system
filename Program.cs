using CarRentalSystem;

var vehicleRepo = new InMemoryVehicleRepository();
var clientRepo = new InMemoryClientRepository();
var rentalRepo = new InMemoryRentalRepository();

vehicleRepo.Add(VehicleFactory.CreateVehicle("car", "C001", "Toyota Camry", 50));
vehicleRepo.Add(VehicleFactory.CreateVehicle("car", "C002", "Tesla Model 3", 100));
vehicleRepo.Add(VehicleFactory.CreateVehicle("car", "C003", "BMW M4", 150));

var rentalSystem = new RentalSystem(vehicleRepo, clientRepo, rentalRepo);

var menu = new ConsoleMenu(rentalSystem, clientRepo);
menu.Start();