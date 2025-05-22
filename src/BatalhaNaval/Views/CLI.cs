using BatalhaNaval.Models;
using System;

namespace BatalhaNaval.Views
{
    /// <summary>
    /// Console view of the game. It is the only place that writes to the console,
    /// keeping the presentation apart from the models and the controllers.
    /// </summary>
    public static class CLI
    {
        /// <summary>
        /// Writes one line of text. Lines always end in "\n" (not the Windows "\r\n"),
        /// so the output matches the expected test files byte by byte on every system.
        /// </summary>
        /// <param name="text">The text to write.</param>
        private static void WriteLine(string text)
        {
            Console.Out.Write(text + "\n");
        }

        /// <summary>
        /// Shows the success message of an instruction.
        /// </summary>
        /// <param name="message">The message to show.</param>
        public static void ShowMessage(string message)
        {
            WriteLine(message);
        }

        /// <summary>
        /// Shows the error message of an instruction that could not be executed.
        /// </summary>
        /// <param name="error">The error message to show.</param>
        public static void ShowError(string error)
        {
            WriteLine(error);
        }

        /// <summary>
        /// Shows a board with the shots it has received.
        /// </summary>
        /// <param name="board">The board to show.</param>
        public static void ShowBoard(Board board)
        {
            foreach (string line in board.RenderShots())
            {
                WriteLine(line);
            }
        }
    }
}
