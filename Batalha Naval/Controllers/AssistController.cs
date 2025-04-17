using BatalhaNaval.Models;
using BatalhaNaval.Views;

namespace BatalhaNaval.Controllers
{
    /// <summary>
    /// This class contains helper methods used by the GameController to handle specific game actions.
    /// </summary>
    public static class AssistController
    {
        /// <summary>
        /// Handles the "RJ" command to register a new player.
        /// </summary>
        /// <param name="players">The list of current players.</param>
        /// <param name="parts">The array of command arguments.</param>
        public static void RegisterPlayer(List<Player> players, string[] parts)
        {
            // Validate the command has exactly one argument (player name)
            if (parts.Length != 2)
            {
                CLI.ShowError("Instrução inválida.");
                return;
            }
        
            string playerName = parts[1];
        
            // Check if a player with the same name already exists
            if (players.Any(p => p.Name == playerName))
            {
                CLI.ShowError("Jogador existente.");
            }
            else
            {
                // Add the new player to the list
                players.Add(new Player(playerName));
                CLI.ShowMessage("Jogador registado com sucesso.");
            }
        }

        /// <summary>
        /// Handles the "EJ" command to remove a player from the list.
        /// </summary>
        /// <param name="players">The list of current players.</param>
        /// <param name="parts">The command arguments.</param>
        /// <param name="gameInProgress">Indicates if a game is currently in progress.</param>
        /// <param name="activePlayer1">The first active player in the game.</param>
        /// <param name="activePlayer2">The second active player in the game.</param>
        public static void RemovePlayer(List<Player> players, string[] parts, bool gameInProgress, Player? activePlayer1, Player? activePlayer2)
        {
            // Validate the command has exactly one argument (player name)
            if (parts.Length != 2)
            {
                CLI.ShowError("Instrução inválida.");
                return;
            }

            string playerName = parts[1];

            // Try to find the player in the list
            var player = players.FirstOrDefault(p => p.Name == playerName);

            if (player == null)
            {
                // Player not found
                CLI.ShowError("Jogador não existente.");
            }
            else if ((activePlayer1?.Name == playerName || activePlayer2?.Name == playerName) && gameInProgress)
            {
                // Player is currently playing an active game
                CLI.ShowError("Jogador participa no jogo em curso.");
            }
            else
            {
                // Remove player from the list
                players.Remove(player);
                CLI.ShowMessage("Jogador removido com sucesso.");
            }
        }

        /// <summary>
        /// Handles the "LJ" command to list all registered players in alphabetical order.
        /// </summary>
        /// <param name="players">The list of current players.</param>
        /// <param name="parts">The command arguments.</param>
        public static void ListPlayers(List<Player> players, string[] parts)
        {
            // Validate the command has no extra arguments
            if (parts.Length != 1)
            {
                CLI.ShowError("Instrução inválida.");
                return;
            }

            // If there are no registered players
            if (players.Count == 0)
            {
                CLI.ShowError("Não existem jogadores registados.");
            }
            else
            {
                // Sort players alphabetically by name
                var sortedPlayers = players.OrderBy(p => p.Name);

                // Display each player's name, games played, and victories
                foreach (var player in sortedPlayers)
                {
                    CLI.ShowMessage($"{player.Name} {player.GamesPlayed} {player.Victories}");
                }
            }
        }
    }
}
