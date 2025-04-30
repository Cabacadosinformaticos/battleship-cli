using BatalhaNaval.Views;
using BatalhaNaval.Models;

namespace BatalhaNaval.Controllers
{
    public class GameController
    {
        private readonly List<Player> players = new();
        private bool gameInProgress = false;
        private Player? activePlayer1 = null;
        private Player? activePlayer2 = null;
        private bool combatStarted = false;
        private Player? currentTurn = null;

        public void ProcessCommand(string input)
        {
            var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0)
                return;

            string command = parts[0];

            switch (command)
            {
                case "RJ":

                    AssistController.RegisterPlayer(players, parts);

                    break;

                case "EJ":

                    AssistController.RemovePlayer(players, parts, gameInProgress, activePlayer1, activePlayer2);

                    break;

                case "LJ":

                    AssistController.ListPlayers(players, parts);

                    break;

                case "IJ":

                    AssistController.InitGame(players, parts, out activePlayer1, out activePlayer2, ref gameInProgress);

                    break;

                case "CN":

                    AssistController.PlaceShip(parts, gameInProgress, combatStarted, activePlayer1, activePlayer2);

                    break;

                case "RN":

                    AssistController.RemoveShip(parts, gameInProgress, combatStarted, activePlayer1, activePlayer2);

                    break;

                default:

                    CLI.ShowError("Instrução inválida.");

                    break;
                    
            }
        }
    }
}