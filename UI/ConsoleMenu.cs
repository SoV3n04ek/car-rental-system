namespace CarRentalSystem
{
    public class ConsoleMenu // : IConsoleMenu
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
            try
            {
                for (; ; )
                {
                    DisplayMenu();
                    var choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            ShowAvailableVehicles();
                            break;
                        case "2":
                            RegisterClient();
                            break;
                        case "3":
                            RentVehicle();
                            break;
                        case "4":
                            ReturnVehicle();
                            break;
                        case "5":
                            ShowState();
                            break;
                        case "6":
                            RunTests();
                            break;
                        case "0":
                            return;
                        default:
                            Console.WriteLine("Invalid option. Please choose 0-6.");
                            break;
                    }
                }
            } 
            catch (Exception ex)
            {
                Console.WriteLine($"\n[SYSTEM ERROR]: ${ex.Message}. Returning to menu...");
            }
        }

        private void RunTests()
        {
            RentalTestSuite.RunAllTests(_rentalSystem, _clientRepository);
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

            Console.WriteLine("\n" + new string('=', 40) + "\n");
        }

        private void ReturnVehicle()
        {
            Console.WriteLine("\n--- Return Vehicle ---");
            var id = GetInput("Enter vehicle ID to return", DataValidator.IsNotEmpty);

            ReturnResult result = _rentalSystem.ReturnVehicle(id);
            if (result.IsSuccess)
            {
                Console.WriteLine("Vehicle returned successfully!");
                if (result.LateFee > 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"LATE FEE: {result.LateFee:C}");
                    Console.ResetColor();
                }
            }
            else
            {
                Console.WriteLine($"Error: {result.Message}");
            }
        }

        private void DisplayMenu()
        {
            Console.WriteLine("\n======= Car Rental System =======");
            Console.WriteLine("1. Show Available Vehicles");
            Console.WriteLine("2. Register Client");
            Console.WriteLine("3. Rent a Vehicle");
            Console.WriteLine("4. Return a Vehicle");
            Console.WriteLine("5. Show System State (All cars & Rentals)");
            Console.WriteLine("6. Run tests");
            Console.WriteLine("0. Exit");
            Console.Write("\nSelect option: ");
        }

        private void RentVehicle()
        {
            Console.WriteLine("\n--- Rent Vehicle ---");

            Console.WriteLine("--- List of avaible vehicles ---");
            ShowAvailableVehicles();

            var vehicleId = GetInput("Enter vehicle ID: ", s => !string.IsNullOrEmpty(s));
            var clientPhone = GetInput("Enter your phone: ", DataValidator.ValidatePhone);
           
            if (!_clientRepository.Exists(clientPhone))
            {
                Console.WriteLine("Client with this phone is not registered");
                Console.WriteLine("Would you like to register now? (y/n): ");

                string response = Console.ReadLine()?.ToLower() ?? "n";
                if (response == "y")
                {
                    RegisterClient();

                    if (!_clientRepository.Exists(clientPhone))
                    {
                        Console.WriteLine("Registration was not completed. Operation cancelled.");
                        return;
                    }
                }
                else
                {
                    Console.WriteLine("Rental cancelled: User not registered.");
                    return;
                }
            }
            
            var days = GetInput("Days to rent: ", s => int.TryParse(s, out int d) && d > 0);

            var result = _rentalSystem.RentVehicle(vehicleId, clientPhone, int.Parse(days));

            if (result.IsSuccess)
                Console.WriteLine($"Success! Rental ID: {result?.Rental?.Id}, Price: {result?.Rental?.TotalPrice:C}");
            else
                Console.WriteLine($"Failed: {result.ErrorMessage}");
        }

        private string GetInput(string prompt,
            Func<string, bool> validator,
            string errorMessage = "Invalid input, try again")
        {
            while (true)
            {
                try
                {
                    Console.Write($"{prompt}: ");
                    string input = Console.ReadLine()?.Trim() ?? "";

                    if (validator(input))
                        return input;

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine(errorMessage);
                    Console.ResetColor();
                }
                catch (Exception)
                {
                    Console.WriteLine("Critical error in entered text. Try again.");
                }
            }
        }

        private void RegisterClient()
        {
            Console.WriteLine("\n--- Client Registration ---");

            var fName = GetInput("First Name (starts with capital)", DataValidator.ValidateName);
            var lName = GetInput("Last Name (starts with capital)", DataValidator.ValidateName);
            var phone = GetInput("Phone (+48...)", DataValidator.ValidatePhone);
            var license = GetInput("License Number", DataValidator.IsNotEmpty);


            try
            {
                // Використовуємо фабрику
                var client = ClientFactory.Create(fName, lName, phone, license, DateTime.Now.AddYears(2));
                _clientRepository.Add(client);
                Console.WriteLine("Client registered successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Registration failed: {ex.Message}");
            }
        }

        private void ShowAvailableVehicles()
        {
            Console.WriteLine("\n--- All Vehicles ---");
            
            foreach (var v in _rentalSystem.GetAvaibleVehicles())
            {
                Console.WriteLine(v);
            }
        }
    }
}