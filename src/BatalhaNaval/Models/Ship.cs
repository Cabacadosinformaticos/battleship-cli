using BatalhaNaval.Models.Interfaces;
namespace BatalhaNaval.Models
{
    public class Ship : IShip
    {
        // Type of the ship (e.g., Submarine, Cruiser)
        public ShipType Type { get; }

        // List of coordinates the ship occupies on the board
        public List<(int Row, int Col)> Coordinates { get; }

        // Indicates whether the ship has been sunk (all positions hit)
        public bool IsSunk { get; private set; }

        // Constructs a ship with a given starting point and direction.
        // Fills in all the coordinates the ship will occupy based on its size.
        public Ship(ShipType type, int startRow, int startCol, char direction)
        {
            Type = type;
            Coordinates = new List<(int, int)>();
            IsSunk = false;

            int size = ShipTypeData.Sizes[type];

            for (int i = 0; i < size; i++)
            {
                int r = startRow, c = startCol;

                // Determine position of each segment based on direction
                switch (char.ToUpper(direction))
                {
                    case 'N': r -= i; break; // North: upward
                    case 'S': r += i; break; // South: downward
                    case 'E': c += i; break; // East: right
                    case 'O': c -= i; break; // West: left
                    case '-': break;        // Single-cell ship, no direction
                    default: throw new ArgumentException("Direção inválida");
                }

                Coordinates.Add((r, c));
            }
        }

        // Registers a hit: removes the coordinate that was hit
        // If all coordinates are hit, the ship is considered sunk
        public void RegisterHit(int row, int col)
        {
            Coordinates.RemoveAll(pos => pos.Row == row && pos.Col == col);
            if (Coordinates.Count == 0)
                IsSunk = true;
        }

        // Checks whether the ship occupies a specific coordinate on the board
        public bool Occupies(int row, int col)
        {
            return Coordinates.Any(pos => pos.Row == row && pos.Col == col);
        }
    }
}
