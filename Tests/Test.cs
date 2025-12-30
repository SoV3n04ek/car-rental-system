namespace CarRentalSystem
{
    public static class RentalTestSuite
    {
        public static void RunAllTests(IRentalSystem system, IClientRepository clientRepo)
        {
            Console.WriteLine("\n=== RUNNING AUTOMATED TESTS ===");

            // Preparation
            var testPhone = "+380991112233";
            if (!clientRepo.Exists(testPhone))
            {
                var testClient = ClientFactory.Create("Test", "User", testPhone, "ABC123456", DateTime.Now.AddYears(1));
                clientRepo.Add(testClient);
            }

            bool res1 = RunReservationTest(system, testPhone);
            bool res2 = RunReturnTest(system);

            Console.WriteLine("\n=== TEST RESULTS ===");
            Console.WriteLine($"Reservation Test: {(res1 ? "PASSED" : "FAILED")}");
            Console.WriteLine($"Return Test: {(res2 ? "PASSED" : "FAILED")}");
            Console.WriteLine("==============================\n");

            Console.WriteLine("Press any key to return to menu...");
            Console.ReadKey();
        }

        private static bool RunReservationTest(IRentalSystem system, string phone)
        {
            Console.WriteLine("Test 1: Reserving C001...");
            var result = system.RentVehicle("C001", phone, 3);

            // Перевіряємо, чи успішно і чи стала машина зайнятою
            var car = system.GetVehicles().First(v => v.Id == "C001");
            return result.IsSuccess && car.IsAvaible == false;
        }

        private static bool RunReturnTest(IRentalSystem system)
        {
            Console.WriteLine("Test 2: Returning C001...");
            var result = system.ReturnVehicle("C001");

            var car = system.GetVehicles().First(v => v.Id == "C001");
            return result.IsSuccess && car.IsAvaible == true;
        }
    }
}