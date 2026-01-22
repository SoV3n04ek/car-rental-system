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
            do
            {
                try
                {
                    DisplayMenu();
                    var choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1": ShowAvailableVehicles(); break;
                        case "2": RegisterClient(); break;
                        case "3": RentVehicle(); break;
                        case "4": ReturnVehicle(); break;
                        case "5": ShowState(); break;
                        case "6": RunTests(); break;
                        case "0": return;
                        default:
                            Console.WriteLine("Invalid option. Please choose 0-6.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    // Global catch to prevent app from crashing
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"\n[SYSTEM ERROR]: {ex.Message}");
                    Console.ResetColor();
                    Console.WriteLine("Returning to main menu...");
                }
            } while (true);
        }

        private void RunTests()
        {
            Test.RunAllTests(_rentalSystem, _clientRepository);
        }

        private void ShowState()
        {
            Console.WriteLine("\n" + new string('=', 40));
            Console.WriteLine("       SYSTEM CURRENT STATE");
            Console.WriteLine(new string('=', 40));

            Console.WriteLine("\n[VEHICLES]");
            foreach (var v in _rentalSystem.GetVehicles())
            {
                var status = v.IsAvaible ? "[FREE]" : "[RENTED]";
                Console.WriteLine($"{status} {v.Id}: {v.Model} ({v.BasePricePerDay:C}/day)");
            }
            Console.WriteLine("\n" + new string('=', 40));
        }

        private void ReturnVehicle()
        {
            Console.WriteLine("\n--- Return Vehicle ---");
            var id = GetInput("Enter vehicle ID to return", DataValidator.IsNotEmpty);

            try
            {
                // Now returns a simple decimal (late fee) or throws exception
                decimal lateFee = _rentalSystem.ReturnVehicle(id);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Vehicle returned successfully!");
                Console.ResetColor();

                if (lateFee > 0)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"LATE FEE APPLIED: {lateFee:C}");
                    Console.ResetColor();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private void RentVehicle()
        {
            Console.WriteLine("\n--- Rent Vehicle ---");
            ShowAvailableVehicles();

            var vehicleId = GetInput("Enter vehicle ID", s => !string.IsNullOrEmpty(s));
            var clientPhone = GetInput("Enter your phone", DataValidator.ValidatePhone);

            // Logic to handle unregistered clients
            if (!_clientRepository.Exists(clientPhone))
            {
                Console.Write("Client not found. Register now? (y/n): ");
                if (Console.ReadLine()?.ToLower() == "y")
                {
                    RegisterClient();
                    if (!_clientRepository.Exists(clientPhone)) return;
                }
                else return;
            }

            var daysInput = GetInput("Days to rent", s => int.TryParse(s, out int d) && d > 0);

            try
            {
                var rental = _rentalSystem.RentVehicle(vehicleId, clientPhone, int.Parse(daysInput));

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Success! Price: {rental.TotalPrice:C}");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Rental failed: {ex.Message}");
            }
        }

        private void RegisterClient()
        {
            Console.WriteLine("\n--- Client Registration ---");

            var fName = GetInput("First Name", DataValidator.ValidateName);
            var lName = GetInput("Last Name", DataValidator.ValidateName);
            var phone = GetInput("Phone (+48...)", DataValidator.ValidatePhone);
            var license = GetInput("License Number", DataValidator.IsNotEmpty);

            try
            {
                var client = Client.Register(fName, lName, phone, license, DateTime.Now.AddYears(2));
                _clientRepository.Add(client);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Client registered successfully!");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Registration failed: {ex.Message}");
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

        private string GetInput(string prompt, Func<string, bool> validator)
        {
            do
            {
                Console.Write($"{prompt}: ");
                string input = Console.ReadLine()?.Trim() ?? "";

                if (validator(input)) return input;

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Invalid input, please try again.");
                Console.ResetColor();
            } while (true);
        }

        private void DisplayMenu()
        {
            Console.WriteLine("\n======= Car Rental System =======");
            Console.WriteLine("1. Show Available Vehicles");
            Console.WriteLine("2. Register Client");
            Console.WriteLine("3. Rent a Vehicle");
            Console.WriteLine("4. Return a Vehicle");
            Console.WriteLine("5. Show System State");
            Console.WriteLine("6. Run Integration Tests");
            Console.WriteLine("0. Exit");
            Console.Write("Select option: ");
        }
    }
}