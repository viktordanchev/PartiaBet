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
            CreateMap<PlayerModel, PlayerDto>();
        }
    }
}
