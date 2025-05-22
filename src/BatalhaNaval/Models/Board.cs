using BatalhaNaval.Models.Interfaces;
namespace BatalhaNaval.Models
{
    public class Board : IBoard
    {
        // Board dimensions (10x10 grid)
        private const int Rows = 10;
        private const int Columns = 10;

        // List of ships placed on this board
        public List<Ship> Ships { get; } = new();

        // Grid representing ship placements
        public char[,] Grid { get; } = new char[Rows, Columns];

        // Total number of shots fired on this board
        public int TotalShots { get; private set; } = 0;

        // Total number of hits achieved
        public int Hits { get; private set; } = 0;

        // Tracks all coordinates that have been shot at
        private HashSet<(int Row, int Col)> ShotsFired = new();

        // Initializes an empty board with '.' in each cell
        public Board()
        {
            for (int r = 0; r < Rows; r++)
                for (int c = 0; c < Columns; c++)
                    Grid[r, c] = '.';
        }

        // Verifies whether a ship can be placed at its coordinates
        // without overlapping another ship or touching it (even diagonally)
        public bool CanPlaceShip(Ship ship)
        {
            foreach (var (row, col) in ship.Coordinates)
            {
                if (!IsInsideBoard(row, col)) return false; // Outside grid boundaries
                if (Grid[row - 1, col - 1] != '.') return false; // Already occupied
                if (IsTouchingAnotherShip(row, col)) return false; // Adjacent to another ship
            }
            return true;
        }

        // Helper to check if coordinates are within the board limits
        private bool IsInsideBoard(int row, int col)
        {
            return row >= 1 && row <= Rows && col >= 1 && col <= Columns;
        }

        // Checks all 8 surrounding tiles to prevent ship adjacency
        private bool IsTouchingAnotherShip(int row, int col)
        {
            for (int dr = -1; dr <= 1; dr++)
            {
                for (int dc = -1; dc <= 1; dc++)
                {
                    int nr = row + dr;
                    int nc = col + dc;
                    if (IsInsideBoard(nr, nc) && Grid[nr - 1, nc - 1] == 'N')
                        return true;
                }
            }
            return false;
        }

        // Places a ship on the board and marks its cells with 'N'
        public void PlaceShip(Ship ship)
        {
            foreach (var (row, col) in ship.Coordinates)
            {
                Grid[row - 1, col - 1] = 'N';
            }
            Ships.Add(ship);
        }

        // Registers a shot: adds the coordinate, increments counters, checks for hit
        public void RegisterShot(int row, int col)
        {
            TotalShots++;
            ShotsFired.Add((row, col));
            var hit = Ships.Any(s => s.Occupies(row, col));
            if (hit) Hits++;
        }

        // Checks if a shot was already fired at a given position
        public bool HasShot(int row, int col)
        {
            return ShotsFired.Contains((row, col));
        }

        // Text representation of the shots received by this board, one string per line:
        // X = hit, * = miss, blank = not yet targeted.
        // Row numbers are right-aligned so that row 10 lines up with the others,
        // and trailing spaces are removed from every line.
        // The board only builds the text; printing it is the job of the view.
        public List<string> RenderShots()
        {
            var lines = new List<string> { "   A B C D E F G H I J" };
            for (int r = 1; r <= Rows; r++)
            {
                var cells = new char[Columns];
                for (int c = 1; c <= Columns; c++)
                {
                    if (ShotsFired.Contains((r, c)))
                        cells[c - 1] = Ships.Any(s => s.Occupies(r, c)) ? 'X' : '*';
                    else
                        cells[c - 1] = ' ';
                }
                lines.Add($"{r,2} {string.Join(' ', cells)}".TrimEnd());
            }
            return lines;
        }
    }
}