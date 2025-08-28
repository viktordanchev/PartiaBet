namespace Core.Games.Dtos
{
    public class User
    {
        public Guid Id { get; set; }

        public bool IsPlayer { get; set; }

        public bool IsActivePlayer { get; set; }
    }
}
