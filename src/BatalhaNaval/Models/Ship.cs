using BatalhaNaval.Models.Interfaces;
namespace BatalhaNaval.Models
{
    public class Ship : IShip
    {
        // Type of the ship (e.g., Submarine, Cruiser)
        public ShipType Type { get; }

        // List of coordinates the ship occupies on the board
        public List<(int Row, int Col)> Coordinates { get; }

        // Coordinates of this ship that have already been hit.
        // Kept apart from Coordinates so the ship keeps its full shape after being hit.
        private readonly HashSet<(int Row, int Col)> hits = new();

        // A ship is sunk once every coordinate it occupies has been hit
        public bool IsSunk => hits.Count == Coordinates.Count;

        // Constructs a ship with a given starting point and direction.
        // Fills in all the coordinates the ship will occupy based on its size.
        public Ship(ShipType type, int startRow, int startCol, char direction)
        {
            Type = type;
            Coordinates = new List<(int, int)>();

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

        // Registers a hit on one of the ship's coordinates.
        // Hitting the same coordinate twice has no extra effect.
        public void RegisterHit(int row, int col)
        {
            if (Occupies(row, col))
                hits.Add((row, col));
        }

        // Checks whether a given coordinate of the ship has already been hit
        public bool IsHitAt(int row, int col)
        {
            return hits.Contains((row, col));
        }

        // Checks whether the ship occupies a specific coordinate on the board
        public bool Occupies(int row, int col)
        {
            return Coordinates.Any(pos => pos.Row == row && pos.Col == col);
        }
    }
}
