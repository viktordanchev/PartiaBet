using AutoMapper;
using Core.Models.Match;
using RestAPI.Dtos.Match;

namespace RestAPI.Mapper
{
    public class MatchProfile : Profile
    {
        public MatchProfile()
        {
            CreateMap<MatchModel, MatchDto>();
            CreateMap<PlayerModel, PlayerDto>()
            .ForMember(dest => dest.TurnTimeLeft, opt => opt.MapFrom(src =>
                CalculateTurnTimeLeft(src)
            ));
        }

        private static double CalculateTurnTimeLeft(PlayerModel player)
        {
            var timeLeft = player.Timer.TimeLeft.TotalSeconds;

            if (player.Timer.TimeLeft > TimeSpan.Zero)
            {
                timeLeft = (player.Timer.TurnExpiresAt - DateTime.UtcNow).TotalSeconds;
            }

            return timeLeft;
        }
    }
}
