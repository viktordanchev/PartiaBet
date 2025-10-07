namespace Core.Games.Dtos
{
    public class TeamDto
    {
        public TeamDto()
        {
            Players = new List<PlayerDto>();
        }

        public Guid Id { get; set; }

        public List<PlayerDto> Players { get; set; }
    }
}
