using System.Collections.Generic;

namespace BatalhaNaval.Models.Interfaces
{
    /// <summary>
    /// Represents the behavior and state of a player in the battleship game.
    /// </summary>
    public interface IPlayer
    {
        /// <summary>
        /// Gets the name of the player.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets or sets the total number of games played by the player.
        /// </summary>
        int GamesPlayed { get; set; }

        /// <summary>
        /// Gets or sets the number of games the player has won.
        /// </summary>
        int Victories { get; set; }

        /// <summary>
        /// Gets the player's board containing their ships.
        /// </summary>
        Board ShipBoard { get; }

        /// <summary>
        /// Gets a dictionary that tracks how many ships of each type have been placed by the player.
        /// </summary>
        Dictionary<ShipType, int> ShipsPlaced { get; }

        /// <summary>
        /// Checks whether the player can place more ships of a given type, based on game rules.
        /// </summary>
        /// <param name="type">The type of ship to check.</param>
        /// <returns>True if the player can place more ships of the given type, false otherwise.</returns>
        bool CanPlaceMoreOfType(ShipType type);

        /// <summary>
        /// Registers a ship that has been placed by the player.
        /// </summary>
        /// <param name="ship">The ship to register.</param>
        void RegisterShip(Ship ship);
    }
}
