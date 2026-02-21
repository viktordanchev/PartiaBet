namespace Games.Chess.PieceMovement
{
    public static class Directions
    {
        public static readonly List<(int, int)> King = new()
        {
            (1, 0),
            (1,  1),
            ( 1, -1),
            (-1, 0),
            (-1, 1),
            (-1, -1),
            (0, 1),
            (0, -1)
        };

        public static readonly List<(int, int)> Queen = new()
        {
            (1, 1),
            (1, -1),
            (-1, -1),
            (-1, 1),
            (1, 0),
            (-1, 0),
            (0, 1),
            (0, -1)
        };

        public static readonly List<(int, int)> Rook = new()
        {
            (1, 0),
            (-1, 0),
            (0, 1),
            (0, -1)
        };

        public static readonly List<(int, int)> Knight = new()
        {
            (2, 1),
            (2, -1),
            (-2, 1),
            (-2, -1),
            (1, 2),
            (-1, 2),
            (1, -2),
            (-1, -2)
        };

        public static readonly List<(int, int)> Bishop = new()
        {
            (1, 1),
            (1, -1),
            (-1, -1),
            (-1, 1)
        };
    }
}
