using System.Text;
using BatalhaNaval.Controllers;

namespace BatalhaNaval
{
    class Program
    {
        // Entry point of the application
        static void Main(string[] args)
        {
            // Messages contain Portuguese characters (ã, ç, ...), so input and output
            // use UTF-8 without a byte order mark, whatever the console default is
            Console.InputEncoding = new UTF8Encoding(false);
            Console.OutputEncoding = new UTF8Encoding(false);

            // Creates an instance of the game controller that will process commands
            var controller = new GameController();

            string? line;

            // Continuously reads user input from the console
            // until a blank line (empty or only spaces) or EOF is encountered
            while (!string.IsNullOrWhiteSpace(line = Console.ReadLine()))
            {
                // Processes each command entered by the user
                controller.ProcessCommand(line);
            }
        }
    }
}
