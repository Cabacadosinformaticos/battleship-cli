using BatalhaNaval.Models;
using BatalhaNaval.Views;

namespace BatalhaNaval.Controllers
{
    /// <summary>
    /// This class contains helper methods used by the GameController to handle specific game actions.
    /// </summary>
    public static class AssistController
    {
        // Size of the square battle grid (rows 1-10, columns A-J)
        private const int GridSize = 10;

        /// <summary>
        /// Converts a row number and a column letter (e.g., "10" and "J") into 1-based
        /// board coordinates.
        /// </summary>
        /// <param name="rowText">Row number typed by the user.</param>
        /// <param name="colText">Column letter typed by the user.</param>
        /// <param name="row">Output: the row, from 1 to 10.</param>
        /// <param name="col">Output: the column, from 1 to 10.</param>
        /// <returns>True if both values describe a cell inside the grid.</returns>
        private static bool TryParseCoordinates(string rowText, string colText, out int row, out int col)
        {
            col = 0;
            if (!int.TryParse(rowText, out row) || colText.Length != 1)
                return false;

            col = colText[0] - 'A' + 1;
            return row >= 1 && row <= GridSize && col >= 1 && col <= GridSize;
        }

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

        /// <summary>
        /// Handles the "CN" command to place a ship on the player's board.
        /// </summary>
        /// <param name="parts">The command arguments.</param>
        /// <param name="gameInProgress">Indicates whether a game is currently in progress.</param>
        /// <param name="combatStarted">Indicates whether combat has started.</param>
        /// <param name="activePlayer1">Reference to the first player.</param>
        /// <param name="activePlayer2">Reference to the second player.</param>
        public static void PlaceShip(
            string[] parts,
            bool gameInProgress,
            bool combatStarted,
            Player? activePlayer1,
            Player? activePlayer2
        )
        {
            // Validate command length (minimum 5, maximum 6 arguments)
            if (parts.Length < 5 || parts.Length > 6)
            {
                CLI.ShowError("Instrução inválida.");
                return;
            }

            // A game must be in progress to place ships
            if (!gameInProgress)
            {
                CLI.ShowError("Não existe jogo em curso.");
                return;
            }

            string playerName = parts[1];
            string typeCode = parts[2];

            // Determine the player issuing the command
            Player? player = (activePlayer1?.Name == playerName) ? activePlayer1 :
                             (activePlayer2?.Name == playerName) ? activePlayer2 : null;

            if (player == null)
            {
                CLI.ShowError("Jogador não participa no jogo em curso.");
                return;
            }

            // Cannot place ships after combat has started
            if (combatStarted)
            {
                CLI.ShowError("Combate iniciado.");
                return;
            }

            // Parse the ship type code (L, S, F, C or P). Enum.TryParse alone would also
            // accept numbers such as "3", so the code must be one known letter.
            if (typeCode.Length != 1 || !char.IsLetter(typeCode[0]) ||
                !Enum.TryParse<ShipType>(typeCode, out var shipType) ||
                !Enum.IsDefined(shipType))
            {
                CLI.ShowError("Instrução inválida.");
                return;
            }

            // Determine ship size and required direction
            int shipSize = ShipTypeData.Sizes[shipType];
            char direction;

            if (shipSize == 1)
            {
                // A single-cell ship has no orientation, whatever was typed
                direction = '-';
            }
            else
            {
                // Larger ships must specify direction
                if (parts.Length != 6)
                {
                    CLI.ShowError("Instrução inválida.");
                    return;
                }
                direction = char.ToUpper(parts[5][0]);
            }

            // The start cell must exist and a multi-cell ship needs a cardinal direction
            if (!TryParseCoordinates(parts[3], parts[4], out int row, out int col) ||
                (shipSize > 1 && (parts[5].Length != 1 || !Ship.IsValidDirection(direction))))
            {
                CLI.ShowError("Posição irregular.");
                return;
            }

            var newShip = new Ship(shipType, row, col, direction);

            // Check if the position is valid (within grid and not overlapping)
            if (!player.ShipBoard.CanPlaceShip(newShip))
            {
                CLI.ShowError("Posição irregular.");
                return;
            }

            // Check if player can place more ships of this type
            if (!player.CanPlaceMoreOfType(shipType))
            {
                CLI.ShowError("Não tem mais navios dessa tipologia disponíveis.");
                return;
            }

            // Check if player already reached the total ship limit
            if (player.ShipsPlaced.Values.Sum() == ShipTypeData.MaxPerPlayer.Values.Sum())
            {
                CLI.ShowError("Não é possível colocar navios.");
                return;
            }

            // Register the ship on the player's board
            player.RegisterShip(newShip);
            CLI.ShowMessage("Navio colocado com sucesso.");
        }

        /// <summary>
        /// Handles the "RN" command to remove a ship from the player's board.
        /// </summary>
        /// <param name="parts">The command arguments.</param>
        /// <param name="gameInProgress">Indicates whether a game is currently in progress.</param>
        /// <param name="combatStarted">Indicates whether combat has started.</param>
        /// <param name="activePlayer1">Reference to the first active player.</param>
        /// <param name="activePlayer2">Reference to the second active player.</param>
        public static void RemoveShip(
            string[] parts,
            bool gameInProgress,
            bool combatStarted,
            Player? activePlayer1,
            Player? activePlayer2
        )
        {
            // A game must be in progress to remove ships
            if (!gameInProgress)
            {
                CLI.ShowError("Não existe jogo em curso.");
                return;
            }

            // Cannot remove ships after combat has started
            if (combatStarted)
            {
                CLI.ShowError("Combate iniciado.");
                return;
            }

            // Validate that we received exactly 4 arguments: RN, player, row, column
            if (parts.Length != 4)
            {
                CLI.ShowError("Instrução inválida.");
                return;
            }

            string playerName = parts[1];

            // Try to parse the row number
            if (!int.TryParse(parts[2], out int row))
            {
                CLI.ShowError("Linha inválida.");
                return;
            }

            // Convert column letter to index
            char colChar = char.ToUpper(parts[3][0]);
            int col = colChar - 'A' + 1;

            // Find the corresponding player
            Player? player = (activePlayer1?.Name == playerName) ? activePlayer1 :
                             (activePlayer2?.Name == playerName) ? activePlayer2 : null;

            if (player == null)
            {
                CLI.ShowError("Jogador não participa no jogo em curso.");
                return;
            }

            // Find the ship occupying the given position
            var shipToRemove = player.ShipBoard.Ships.FirstOrDefault(s => s.Occupies(row, col));

            if (shipToRemove == null)
            {
                CLI.ShowError("Não existe navio na posição.");
                return;
            }

            // Remove ship from the board
            player.ShipBoard.Ships.Remove(shipToRemove);

            // Clear the ship's coordinates from the grid
            foreach (var (r, c) in shipToRemove.Coordinates)
            {
                player.ShipBoard.Grid[r - 1, c - 1] = '.';
            }

            // Update ship counter
            if (player.ShipsPlaced.ContainsKey(shipToRemove.Type) && player.ShipsPlaced[shipToRemove.Type] > 0)
            {
                player.ShipsPlaced[shipToRemove.Type]--;
            }

            CLI.ShowMessage("Navio removido com sucesso.");
        }

        /// <summary>
        /// Handles the "IC" command to start the combat phase if both players have placed all ships.
        /// </summary>
        /// <param name="parts">Command input parts.</param>
        /// <param name="gameInProgress">Indicates if a game is in progress.</param>
        /// <param name="combatStarted">Reference: will be set to true if combat starts.</param>
        /// <param name="activePlayer1">First active player.</param>
        /// <param name="activePlayer2">Second active player.</param>
        public static void StartCombat(
            string[] parts,
            bool gameInProgress,
            ref bool combatStarted,
            Player? activePlayer1,
            Player? activePlayer2
        )
        {
            // A game must be in progress to start combat
            if (!gameInProgress)
            {
                CLI.ShowError("Não existe jogo em curso.");
                return;
            }

            // Cannot start combat if it already started
            if (combatStarted)
            {
                CLI.ShowError("Combate iniciado.");
                return;
            }

            // Validate that there are no extra arguments
            if (parts.Length != 1)
            {
                CLI.ShowError("Instrução inválida.");
                return;
            }

            // Check if both players have placed all their ships
            bool player1Ready = activePlayer1 != null &&
                activePlayer1.ShipsPlaced.Values.Sum() == ShipTypeData.MaxPerPlayer.Values.Sum();

            bool player2Ready = activePlayer2 != null &&
                activePlayer2.ShipsPlaced.Values.Sum() == ShipTypeData.MaxPerPlayer.Values.Sum();

            if (!player1Ready || !player2Ready)
            {
                CLI.ShowError("Navios não colocados.");
                return;
            }

            // All checks passed — start combat
            combatStarted = true;
            CLI.ShowMessage("Combate iniciado.");
        }

        /// <summary>
        /// Handles the "T" command to perform a shot in the game.
        /// </summary>
        public static void ExecuteShot(
            string[] parts,
            bool gameInProgress,
            bool combatStarted,
            Player? activePlayer1,
            Player? activePlayer2,
            ref Player? currentTurn,
            ref bool gameOver,
            ref bool combat,
            List<Player> players
        )
        {
            // Validate command format (should contain 4 parts: T, player, row, column)
            if (parts.Length != 4)
            {
                CLI.ShowError("Instrução inválida.");
                return;
            }

            // Ensure a game is running and combat has started
            if (!gameInProgress)
            {
                CLI.ShowError("Não existe jogo em curso.");
                return;
            }

            if (!combatStarted)
            {
                CLI.ShowError("Jogo em curso sem combate iniciado.");
                return;
            }

            string name = parts[1];

            // Try to parse the row number
            if (!int.TryParse(parts[2], out int row))
            {
                CLI.ShowError("Linha inválida.");
                return;
            }

            // Convert column character to numeric index
            char colChar = char.ToUpper(parts[3][0]);
            int col = colChar - 'A' + 1;

            // Identify the player taking the shot
            Player? shooter = (activePlayer1?.Name == name) ? activePlayer1 :
                              (activePlayer2?.Name == name) ? activePlayer2 : null;

            if (shooter == null)
            {
                CLI.ShowError("Jogador não participa no jogo em curso.");
                return;
            }

            // Validate if the coordinates are inside the board
            if (row < 1 || row > 10 || col < 1 || col > 10)
            {
                CLI.ShowError("Posição irregular.");
                return;
            }

            // Handle the first turn or enforce turn alternation
            if (currentTurn == null)
            {
                currentTurn = shooter; // First move of the match
            }
            else if (currentTurn.Name != name)
            {
                CLI.ShowError("Instrução inválida.");
                return;
            }

            // Identify the opponent
            Player opponent = (shooter == activePlayer1) ? activePlayer2! : activePlayer1!;

            // Prevent shooting the same position twice
            if (opponent.ShipBoard.HasShot(row, col))
            {
                CLI.ShowError("Posição irregular.");
                return;
            }

            // Register the shot on the opponent's board
            opponent.ShipBoard.RegisterShot(row, col);

            // Check if the shot hit any ship
            var target = opponent.ShipBoard.Ships.FirstOrDefault(s => s.Occupies(row, col));

            if (target == null)
            {
                CLI.ShowMessage("Tiro na água.");
            }
            else
            {
                target.RegisterHit(row, col);

                if (target.IsSunk)
                {
                    // Check if this was the last ship
                    bool allSunk = opponent.ShipBoard.Ships.All(s => s.IsSunk);

                    if (allSunk)
                    {
                        // End the game: update statistics
                        shooter.Victories++;
                        shooter.GamesPlayed++;
                        opponent.GamesPlayed++;

                        CLI.ShowMessage($"Navio {target.Type} afundado. Jogo terminado.");

                        gameOver = false;
                        combat = false;
                        currentTurn = null;
                        return;
                    }
                    else
                    {
                        CLI.ShowMessage($"Navio {target.Type} afundado.");
                    }
                }
                else
                {
                    CLI.ShowMessage($"Tiro em navio {target.Type}.");
                }
            }

            // Change turn to opponent
            currentTurn = opponent;
        }

        /// <summary>
        /// Handles the "V" command to show player statistics and opponent boards.
        /// </summary>
        /// <param name="gameInProgress">Whether a game is currently running.</param>
        /// <param name="combatStarted">Whether the combat has already started.</param>
        /// <param name="activePlayer1">First active player.</param>
        /// <param name="activePlayer2">Second active player.</param>
        public static void VisualizeGame(
            bool gameInProgress,
            bool combatStarted,
            Player? activePlayer1,
            Player? activePlayer2
        )
        {
            // A game must be in progress to visualize stats
            if (!gameInProgress)
            {
                CLI.ShowError("Não existe jogo em curso.");
                return;
            }
        
            // Combat must have started to see progress
            if (!combatStarted)
            {
                CLI.ShowError("Jogo em curso sem combate iniciado.");
                return;
            }
        
            // Order players alphabetically by name for consistent display
            List<Player> gamePlayers = new() { activePlayer1!, activePlayer2! };
            gamePlayers = gamePlayers.OrderBy(p => p.Name).ToList();
        
            foreach (var player in gamePlayers)
            {
                // The opponent is the other player in the match
                Player opponent = (player == activePlayer1) ? activePlayer2! : activePlayer1!;
        
                // Gather stats based on opponent's board (what this player has done)
                int totalShots = opponent.ShipBoard.TotalShots;
                int hits = opponent.ShipBoard.Hits;
                int sunk = opponent.ShipBoard.Ships.Count(s => s.IsSunk);
        
                // Display summary line
                CLI.ShowMessage($"{player.Name} {totalShots} {hits} {sunk}");
        
                // Show opponent's board (what this player shot)
                CLI.ShowBoard(opponent.ShipBoard);
            }
        }

        /// <summary>
        /// Handles the "D" command to process player(s) forfeiting the game.
        /// </summary>
        /// <param name="parts">Command arguments.</param>
        /// <param name="gameInProgress">Reference to gameInProgress flag to be reset.</param>
        /// <param name="combatStarted">Reference to combatStarted flag to be reset.</param>
        /// <param name="activePlayer1">Reference to activePlayer1 to be reset.</param>
        /// <param name="activePlayer2">Reference to activePlayer2 to be reset.</param>
        /// <param name="currentTurn">Reference to currentTurn to be reset.</param>
        /// <param name="players">The global player list.</param>
        public static void HandleForfeit(
            string[] parts,
            ref bool gameInProgress,
            ref bool combatStarted,
            ref Player? activePlayer1,
            ref Player? activePlayer2,
            ref Player? currentTurn,
            List<Player> players
        )
        {
            // A game must be in progress to allow forfeiting
            if (!gameInProgress)
            {
                CLI.ShowError("Não existe jogo em curso.");
                return;
            }
        
            // Must have at least one and at most two players listed in the command
            if (parts.Length < 2 || parts.Length > 3)
            {
                CLI.ShowError("Instrução inválida.");
                return;
            }
        
            // Extract names of players who are forfeiting
            List<string> quitters = parts.Skip(1).ToList();
            List<string> activeNames = new() { activePlayer1!.Name, activePlayer2!.Name };
        
            // All players listed must be participating in the current game
            if (quitters.Any(name => !activeNames.Contains(name)))
            {
                CLI.ShowError("Jogador não participa no jogo em curso.");
                return;
            }
        
            if (quitters.Count == 1)
            {
                // One player forfeited → the other wins
                string loser = quitters[0];
                string winner = activeNames.First(name => name != loser);
        
                Player winnerPlayer = players.First(p => p.Name == winner);
                Player loserPlayer = players.First(p => p.Name == loser);
        
                // Update stats: winner gets victory, both count as having played
                winnerPlayer.Victories++;
                winnerPlayer.GamesPlayed++;
                loserPlayer.GamesPlayed++;
            }
            else
            {
                // Both players forfeited → both only count the game as played
                foreach (string name in activeNames)
                {
                    var p = players.First(pl => pl.Name == name);
                    p.GamesPlayed++;
                }
            }
        
            // Reset game state
            gameInProgress = false;
            combatStarted = false;
            activePlayer1 = null;
            activePlayer2 = null;
            currentTurn = null;
        
            CLI.ShowMessage("Desistência com sucesso. Jogo terminado.");
        }
    }
}
