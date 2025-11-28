using Games.Dtos;

namespace Games.Chess.Models
{
    public class NewMoveDto : BaseDto
    {

        public int OldRow { get; set; }

        public int OldCol { get; set; }

        public int NewRow { get; set; }

        public int NewCol { get; set; }
    }
}
