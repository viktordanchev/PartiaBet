using AutoMapper;
using Core.Models.Match;
using RestAPI.Dtos.Match;

namespace RestAPI.Mapper
{
    public class PlayerProfile : Profile
    {
        public PlayerProfile()
        {
            CreateMap<PlayerModel, PlayerDto>()
            .ForMember(dest => dest.TurnTimeLeft, opt =>
                opt.MapFrom(src => GetTurnTimeLeft(src)));
        }

        private double GetTurnTimeLeft(PlayerModel player)
        {
            if (player.IsOnTurn && !player.Timer.IsPaused)
                return Math.Max(0, (player.Timer.TurnExpiresAt - DateTime.UtcNow).TotalSeconds);

            return Math.Max(0, player.Timer.TimeLeft.TotalSeconds);
        }
    }
}
