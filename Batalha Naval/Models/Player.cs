using BatalhaNaval.Models.Interfaces;
namespace BatalhaNaval.Models
{
    public class Player : IPlayer
    {
        // Player's name (used as identifier)
        public string Name { get; }

        // Total number of games played by this player
        public int GamesPlayed { get; set; }

        // Total number of victories by this player
        public int Victories { get; set; }

        // Board where the player's ships are placed
        public Board ShipBoard { get; }

        // Tracks how many ships of each type the player has placed
        public Dictionary<ShipType, int> ShipsPlaced { get; }

        // Initializes a new player with an empty board and ship counts set to 0
        public Player(string name)
        {
            Name = name;
            GamesPlayed = 0;
            Victories = 0;
            ShipBoard = new Board();
            ShipsPlaced = new Dictionary<ShipType, int>();

            // Set default count of 0 for all ship types
            foreach (var type in Enum.GetValues(typeof(ShipType)).Cast<ShipType>())
            {
                ShipsPlaced[type] = 0;
            }
        }

        // Determines if the player can still place another ship of the specified type
        public bool CanPlaceMoreOfType(ShipType type)
        {
            return ShipsPlaced[type] < ShipTypeData.MaxPerPlayer[type];
        }

        // Registers a ship on the player's board and updates count
        public void RegisterShip(Ship ship)
        {
            ShipsPlaced[ship.Type]++;
            ShipBoard.PlaceShip(ship);
        }
    }
}
