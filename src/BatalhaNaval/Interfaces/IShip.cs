using System.Collections.Generic;

namespace BatalhaNaval.Models.Interfaces
{
    /// <summary>
    /// Represents the behavior and state of a ship in the battleship game.
    /// </summary>
    public interface IShip
    {
        /// <summary>
        /// Gets the type of the ship (e.g., Submarine, Cruiser).
        /// </summary>
        ShipType Type { get; }

        /// <summary>
        /// Gets all the coordinates occupied by the ship.
        /// </summary>
        List<(int Row, int Col)> Coordinates { get; }

        /// <summary>
        /// Gets a value indicating whether the ship has been completely destroyed.
        /// </summary>
        bool IsSunk { get; }

        /// <summary>
        /// Registers a hit on the ship at the specified coordinates.
        /// </summary>
        /// <param name="row">The row of the hit.</param>
        /// <param name="col">The column of the hit.</param>
        void RegisterHit(int row, int col);

        /// <summary>
        /// Checks whether the ship occupies the specified coordinates.
        /// </summary>
        /// <param name="row">The row to check.</param>
        /// <param name="col">The column to check.</param>
        /// <returns>True if the ship occupies the position, false otherwise.</returns>
        bool Occupies(int row, int col);

        /// <summary>
        /// Checks whether the ship has already been hit at the specified coordinates.
        /// </summary>
        /// <param name="row">The row to check.</param>
        /// <param name="col">The column to check.</param>
        /// <returns>True if that part of the ship was hit, false otherwise.</returns>
        bool IsHitAt(int row, int col);
    }
}
