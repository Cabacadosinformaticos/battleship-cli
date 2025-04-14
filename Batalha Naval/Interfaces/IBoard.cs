using System.Collections.Generic;

namespace BatalhaNaval.Models.Interfaces
{
    /// <summary>
    /// Represents the behavior of a battleship game board.
    /// </summary>
    public interface IBoard
    {
        /// <summary>
        /// List of ships currently placed on the board.
        /// </summary>
        List<Ship> Ships { get; }

        /// <summary>
        /// Two-dimensional character grid representing the board state.
        /// </summary>
        char[,] Grid { get; }

        /// <summary>
        /// Total number of shots fired on this board.
        /// </summary>
        int TotalShots { get; }

        /// <summary>
        /// Total number of successful hits made on this board.
        /// </summary>
        int Hits { get; }

        /// <summary>
        /// Checks whether a ship can be placed on the board at its given coordinates.
        /// </summary>
        /// <param name="ship">The ship to be placed.</param>
        /// <returns>True if the ship can be placed, false otherwise.</returns>
        bool CanPlaceShip(Ship ship);

        /// <summary>
        /// Places a ship on the board at its specified coordinates.
        /// </summary>
        /// <param name="ship">The ship to place on the board.</param>
        void PlaceShip(Ship ship);

        /// <summary>
        /// Registers a shot fired at the specified position on the board.
        /// </summary>
        /// <param name="row">Row of the shot.</param>
        /// <param name="col">Column of the shot.</param>
        void RegisterShot(int row, int col);

        /// <summary>
        /// Checks if a shot has already been fired at the specified position.
        /// </summary>
        /// <param name="row">Row to check.</param>
        /// <param name="col">Column to check.</param>
        /// <returns>True if the position was already targeted, false otherwise.</returns>
        bool HasShot(int row, int col);

        /// <summary>
        /// Displays the current formatted board grid in the console, showing previous shots.
        /// </summary>
        void ShowFormattedGrid();
    }
}
