namespace CarRentalSystem
{
    public static class Test
    {
        // Constants for test data to ensure consistency across all test methods
        private const string ValidTestPhone = "+48111222333";
        private const string ExpiredTestPhone = "+48999000999";
        private const string TargetVehicleId = "C001";

        public static void RunAllTests(IRentalSystem system, IClientRepository clientRepo)
        {
            Console.Clear();
            Console.WriteLine("=== SYSTEM INTEGRATION TESTS ===");

            // Phase 1: Setup environment
            SeedDatabase(clientRepo);

            // Phase 2: Execute test cases
            int passed = 0;
            int total = 4;

            if (Test_Successful_Rental_And_Return(system)) passed++;
            if (Test_Cannot_Rent_Already_Occupied_Vehicle(system)) passed++;
            if (Test_Fail_On_Invalid_Client(system)) passed++;
            if (Test_Late_Fee_Calculation(system)) passed++;

            // Phase 3: Reporting results
            Console.WriteLine("\n" + new string('=', 30));
            Console.WriteLine($"TOTAL RESULTS: {passed}/{total} PASSED");
            Console.WriteLine(new string('=', 30));

            Console.WriteLine("\nPress any key to return...");
            Console.ReadKey();
        }

        private static void SeedDatabase(IClientRepository clientRepo)
        {
            // Register a valid client if doesn't exist
            if (!clientRepo.Exists(ValidTestPhone))
            {
                var validClient = Client.Register("John", "Doe", ValidTestPhone, "DL12345", DateTime.Now.AddYears(2));
                clientRepo.Add(validClient);
            }

            // Attempting to register an invalid client to verify constructor safety
            if (!clientRepo.Exists(ExpiredTestPhone))
            {
                try
                {
                    // This should fail due to internal validation (license expires too soon)
                    Client.Register("Expired", "User", ExpiredTestPhone, "BAD000", DateTime.Now.AddDays(5));
                }
                catch (ArgumentException)
                {
                    // Expected behavior: validation blocked invalid data
                }
            }
        }

        private static bool Test_Successful_Rental_And_Return(IRentalSystem system)
        {
            Console.Write("Test: Simple Rent/Return Flow... ");
            try
            {
                system.RentVehicle(TargetVehicleId, ValidTestPhone, 2);
                var car = system.GetVehicles().First(v => v.Id == TargetVehicleId);

                if (car.IsAvaible) return Fail("Vehicle status not updated to 'Occupied'");

                system.ReturnVehicle(TargetVehicleId);
                if (!car.IsAvaible) return Fail("Vehicle status not updated to 'Available'");

                return Success();
            }
            catch (Exception ex) { return Fail(ex.Message); }
        }

        private static bool Test_Cannot_Rent_Already_Occupied_Vehicle(IRentalSystem system)
        {
            Console.Write("Test: Double Rental Protection... ");
            try
            {
                system.RentVehicle(TargetVehicleId, ValidTestPhone, 1);

                // Second attempt should throw an exception
                try
                {
                    system.RentVehicle(TargetVehicleId, ValidTestPhone, 1);
                    return Fail("System allowed renting an already occupied vehicle!");
                }
                catch (InvalidOperationException)
                {
                    system.ReturnVehicle(TargetVehicleId); // Cleanup
                    return Success();
                }
            }
            catch (Exception ex) { return Fail(ex.Message); }
        }

        private static bool Test_Fail_On_Invalid_Client(IRentalSystem system)
        {
            Console.Write("Test: Non-existent Client Protection... ");
            try
            {
                system.RentVehicle(TargetVehicleId, "000000000", 3);
                return Fail("System allowed rental for non-existent phone number!");
            }
            catch (Exception) { return Success(); }
        }

        private static bool Test_Late_Fee_Calculation(IRentalSystem system)
        {
            Console.Write("Test: Late Fee Calculation... ");
            try
            {
                // Create a manual rental starting 5 days ago for a 1-day period
                var vehicle = system.GetVehicles().First();
                var client = Client.Register("Test", "Tester", "+48555444333", "TEMP1", DateTime.Now.AddYears(1));

                var rental = new Rental(vehicle, client, 1, DateTime.Now.AddDays(-5));

                // Returning now (4 days late)
                decimal fee = rental.ReturnVehicle(DateTime.Now);

                return fee > 0 ? Success($"Calculated fee: {fee:C}") : Fail("Fee was not applied for late return");
            }
            catch (Exception ex) { return Fail(ex.Message); }
        }

        private static bool Success(string msg = "")
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("PASSED " + (msg != "" ? $"({msg})" : ""));
            Console.ResetColor();
            return true;
        }

        private static bool Fail(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("FAILED: " + msg);
            Console.ResetColor();
            return false;
        }
    }
}