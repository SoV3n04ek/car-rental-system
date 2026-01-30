namespace CarRentalSystem.UI
{
    public static class ConsoleHelper
    {
        public static string GetInput(string prompt, Func<string, bool> validator)
        {
            while (true)
            {
                Console.Write($"{prompt}: ");
                string input = Console.ReadLine()?.Trim() ?? "";

                if (validator(input)) return input;

                WriteWarning("Invalid input, please try again.");
            }
        }

        public static void WriteError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public static void WriteSuccess(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public static void WriteWarning(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}