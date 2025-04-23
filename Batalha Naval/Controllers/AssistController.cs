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

        /// <summary>
        /// Handles the "IJ" command to start a game between two registered players.
        /// </summary>
        /// <param name="players">The list of registered players.</param>
        /// <param name="parts">The command arguments.</param>
        /// <param name="activePlayer1">Output: the first player in the match.</param>
        /// <param name="activePlayer2">Output: the second player in the match.</param>
        /// <param name="gameInProgress">Reference: flag indicating if a game is running.</param>
        public static void InitGame(
            List<Player> players,
            string[] parts,
            out Player? activePlayer1,
            out Player? activePlayer2,
            ref bool gameInProgress
        )
        {
            // Reset output values
            activePlayer1 = null;
            activePlayer2 = null;

            // Validate the command has two player names
            if (parts.Length != 3)
            {
                CLI.ShowError("Instrução inválida.");
                return;
            }

            string name1 = parts[1];
            string name2 = parts[2];

            // Try to find both players
            var player1 = players.FirstOrDefault(p => p.Name == name1);
            var player2 = players.FirstOrDefault(p => p.Name == name2);

            if (player1 == null || player2 == null)
            {
                // One or both players not found
                CLI.ShowError("Jogador não registado.");
            }
            else if (gameInProgress)
            {
                // Cannot start a new game if one is already running
                CLI.ShowError("Existe um jogo em curso.");
            }
            else
            {
                // Set active players and start the game
                activePlayer1 = player1;
                activePlayer2 = player2;
                gameInProgress = true;

                // Display a confirmation message in alphabetical order
                var namesOrdered = new[] { name1, name2 }.OrderBy(n => n).ToArray();
                CLI.ShowMessage($"Jogo iniciado entre {namesOrdered[0]} e {namesOrdered[1]}.");
            }
        }
    }
}
