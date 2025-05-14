using BatalhaNaval.Models;
using System;

namespace BatalhaNaval.Views
{
    public static class CLI
    {
        public static void ShowMessage(string message)
        {
            Console.WriteLine(message);
        }

        public static void ShowError(string error)
        {
            Console.WriteLine(error);
        }

        public static void ShowBoard(Board board)
        {
            board.ShowFormattedGrid();
        }
    }
}