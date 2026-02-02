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
                opt.MapFrom(src =>
                    src.IsOnTurn
                        ? (src.Timer.TurnExpiresAt - DateTime.UtcNow).TotalSeconds
                        : src.Timer.TimeLeft.TotalSeconds
                ));
        }
    }
}
