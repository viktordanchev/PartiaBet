namespace Games.Chess.PieceMovement
{
    public static class Directions
    {
        public static readonly List<(int Row, int Col)> King = new()
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

        public static readonly List<(int Row, int Col)> Queen = new()
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

        public static readonly List<(int Row, int Col)> Rook = new()
        {
            (1, 0),
            (-1, 0),
            (0, 1),
            (0, -1)
        };

        public static readonly List<(int Row, int Col)> Knight = new()
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

        public static readonly List<(int Row, int Col)> Bishop = new()
        {
            (1, 1),
            (1, -1),
            (-1, -1),
            (-1, 1)
        };
    }
}
