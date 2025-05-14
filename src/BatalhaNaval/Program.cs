using BatalhaNaval.Controllers;

namespace BatalhaNaval
{
    class Program
    {
        // Entry point of the application
        static void Main(string[] args)
        {
            // Creates an instance of the game controller that will process commands
            var controller = new GameController();

            string? line;

            // Continuously reads user input from the console
            // until an empty line or EOF is encountered
            while (!string.IsNullOrEmpty(line = Console.ReadLine()))
            {
                // Processes each command entered by the user
                controller.ProcessCommand(line);
            }
        }
    }
}
