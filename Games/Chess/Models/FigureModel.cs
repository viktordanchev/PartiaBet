namespace Games.Chess.Models
{
    public class FigureModel
    {
        public Guid PlayerId { get; set; }

        public string Type { get; set; } = string.Empty;

        public int Row { get; set; }

        public int Col { get; set; }
    }
}
