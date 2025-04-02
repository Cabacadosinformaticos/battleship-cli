namespace BatalhaNaval.Models
{
    public enum ShipType
    {
        L, // Lancha - occupies 1 cell (small patrol boat)
        S, // Submarino - occupies 2 cells (stealth underwater vessel)
        F, // Fragata - occupies 3 cells (medium-sized escort ship)
        C, // Cruzador - occupies 4 cells (heavily armed combat ship)
        P  // Porta-aviões - occupies 5 cells (largest naval vessel, carries aircraft)
    }

    public static class ShipTypeData
    {
        // Maps ship types to their respective sizes (number of cells occupied)
        public static readonly Dictionary<ShipType, int> Sizes = new()
        {
            { ShipType.L, 1 },
            { ShipType.S, 2 },
            { ShipType.F, 3 },
            { ShipType.C, 4 },
            { ShipType.P, 5 }
        };

        // Maximum number of ships of each type a player can place
        public static readonly Dictionary<ShipType, int> MaxPerPlayer = new()
        {
            { ShipType.L, 4 },
            { ShipType.S, 3 },
            { ShipType.F, 2 },
            { ShipType.C, 1 },
            { ShipType.P, 1 }
        };
    }
}