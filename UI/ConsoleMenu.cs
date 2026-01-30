using CarRentalSystem.UI;

namespace CarRentalSystem
{
    public class ConsoleMenu
    {
        private readonly IRentalSystem _rentalSystem;
        private readonly IClientRepository _clientRepository;

        public ConsoleMenu(IRentalSystem rentalSystem, IClientRepository clientRepository)
        {
            _rentalSystem = rentalSystem;
            _clientRepository = clientRepository;
        }

        public void Start()
        {
            while (true)
            {
                try
                {
                    DisplayMenu();
                    var choice = Console.ReadLine();

                    if (choice == "0")
                    {
                        Console.WriteLine("Exiting program...");
                        break;
                    }

                    ProcessChoice(choice);
                }
                catch (Exception ex)
                {
                    ConsoleHelper.WriteError($"\n[SYSTEM ERROR]: {ex.Message}");
                    Console.WriteLine("Returning to main menu...");
                }
            }
        }

        private void ProcessChoice(string? choice)
        {
            switch (choice)
            {
                case "1": ShowAvailableVehicles(); break;
                case "2": RegisterClient(); break;
                case "3": RentVehicle(); break;
                case "4": ReturnVehicle(); break;
                case "5": ShowState(); break;
                case "6": RunTests(); break;
                case "7": DeleteClient(); break;
                case "8": UpdateClient(); break;
                case "9": ShowAllClients(); break;
                case "10": ShowAllRentals(); break;
                default: Console.WriteLine("Invalid option. Please choose 0-10."); break;
            }
        }

        private void ShowAvailableVehicles()
        {
            Console.WriteLine("\n--- Available Vehicles ---");
            var vehicles = _rentalSystem.GetAvaibleVehicles();

            if (!vehicles.Any())
                Console.WriteLine("No vehicles available at the moment.");
            else
                foreach (var v in vehicles) Console.WriteLine(v);
        }

        private void RegisterClient()
        {
            Console.WriteLine("\n--- Client Registration ---");

            var fName = ConsoleHelper.GetInput("First Name", DataValidator.ValidateName);
            var lName = ConsoleHelper.GetInput("Last Name", DataValidator.ValidateName);
            var phone = ConsoleHelper.GetInput("Phone (+48...)", DataValidator.ValidatePhone);
            var license = ConsoleHelper.GetInput("License Number", DataValidator.IsNotEmpty);

            var client = Client.Register(fName, lName, phone, license, DateTime.Now.AddYears(2));
            _clientRepository.Add(client);

            ConsoleHelper.WriteSuccess("Client registered successfully!");
        }

        private void RentVehicle()
        {
            Console.WriteLine("\n--- Rent Vehicle ---");
            ShowAvailableVehicles();

            var vehicleId = ConsoleHelper.GetInput("Enter vehicle ID", s => !string.IsNullOrEmpty(s));
            var clientPhone = ConsoleHelper.GetInput("Enter your phone", DataValidator.ValidatePhone);

            if (!_clientRepository.Exists(clientPhone))
            {
                Console.Write("Client not found. Register now? (y/n): ");
                if (Console.ReadLine()?.ToLower() != "y") return;

                RegisterClient();
                if (!_clientRepository.Exists(clientPhone)) return;
            }

            var daysInput = ConsoleHelper.GetInput("Days to rent", s => int.TryParse(s, out int d) && d > 0);
            var rental = _rentalSystem.RentVehicle(vehicleId, clientPhone, int.Parse(daysInput));

            ConsoleHelper.WriteSuccess($"Success! Rental ID: {rental.Id} | Price: {rental.TotalPrice:C}");
        }

        private void ReturnVehicle()
        {
            Console.WriteLine("\n--- Return Vehicle ---");
            var id = ConsoleHelper.GetInput("Enter vehicle ID to return", DataValidator.IsNotEmpty);

            decimal lateFee = _rentalSystem.ReturnVehicle(id);
            ConsoleHelper.WriteSuccess("Vehicle returned successfully!");

            if (lateFee > 0)
                ConsoleHelper.WriteWarning($"LATE FEE APPLIED: {lateFee:C}");
        }

        private void ShowState()
        {
            Console.WriteLine("\n" + new string('=', 40));
            Console.WriteLine("       SYSTEM CURRENT STATE");
            Console.WriteLine(new string('=', 40));

            Console.WriteLine("\n[VEHICLES]");
            foreach (var v in _rentalSystem.GetVehicles())
            {
                string status = v.IsAvaible ? "[FREE]" : "[RENTED]";
                Console.WriteLine($"{status} {v.Id}: {v.Model} ({v.BasePricePerDay:C}/day)");
            }
            Console.WriteLine("\n" + new string('=', 40));
        }

        private void RunTests()
        {
            Test.RunAllTests(_rentalSystem, _clientRepository);
        }

        private void DeleteClient()
        {
            Console.WriteLine("\n--- Delete Client ---");
            var phone = ConsoleHelper.GetInput("Enter client phone", DataValidator.ValidatePhone);
            _rentalSystem.RemoveClient(phone);
            ConsoleHelper.WriteSuccess("Client deleted successfully (Soft Delete).");
        }

        private void UpdateClient()
        {
            Console.WriteLine("\n--- Update Client Data ---");
            var phone = ConsoleHelper.GetInput("Enter client phone", DataValidator.ValidatePhone);
            var client = _clientRepository.GetByPhone(phone);

            if (client == null)
            {
                ConsoleHelper.WriteWarning("Client not found.");
                return;
            }

            Console.WriteLine($"Current name: {client.FirstName} {client.LastName}");

            Console.Write("Enter new First Name (leave empty to keep current): ");
            var newFirstName = Console.ReadLine();

            Console.Write("Enter new Last Name (leave empty to keep current): ");
            var newLastName = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(newFirstName) || !string.IsNullOrWhiteSpace(newLastName))
            {
                client.UpdateName(newFirstName ?? "", newLastName ?? "");
                _clientRepository.Update(client);
                ConsoleHelper.WriteSuccess("Client updated successfully.");
            }
        }

        private void ShowAllClients()
        {
            Console.WriteLine("\n--- Registered Clients List ---");
            var clients = _rentalSystem.GetClients();

            if (!clients.Any())
            {
                Console.WriteLine("No active clients registered.");
                return;
            }

            foreach (var client in clients)
            {
                Console.WriteLine($"Name: {client.FirstName} {client.LastName} | Phone: {client.PhoneNumber}");
                Console.WriteLine(new string('-', 35));
            }
        }

        private void ShowAllRentals()
        {
            Console.WriteLine("\n--- Rental History ---");
            var rentals = _rentalSystem.GetRentals();

            if (!rentals.Any())
            {
                Console.WriteLine("No rental history found.");
                return;
            }

            foreach (var rental in rentals)
            {
                var status = rental.IsActive ? "ACTIVE" : "COMPLETED";
                Console.WriteLine($"ID: {rental.Id.ToString().Substring(0, 8)}... | Car: {rental.Vehicle.Model}");
                Console.WriteLine($"Client: {rental.Client.FirstName} {rental.Client.LastName} | Status: {status}");
                Console.WriteLine($"Total Price: {rental.TotalPrice:C}");
                Console.WriteLine(new string('-', 35));
            }
        }

        private void DisplayMenu()
        {
            Console.WriteLine("\n======= Car Rental System =======");
            Console.WriteLine("1. Vehicles Available   2. Register Client");
            Console.WriteLine("3. Rent Vehicle         4. Return Vehicle");
            Console.WriteLine("5. System State         6. Run Tests");
            Console.WriteLine("7. Delete Client        8. Update Client");
            Console.WriteLine("9. List Clients        10. Rental History");
            Console.WriteLine("0. Exit");
            Console.Write("\nSelect option: ");
        }
    }
}