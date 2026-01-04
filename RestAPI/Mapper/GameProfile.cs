using AutoMapper;
using Core.Models.Games;
using RestAPI.Dtos.Games;

namespace RestAPI.Mapper
{
    public class GameProfile : Profile
    {
        public GameProfile()
        {
            CreateMap<GameModel, GameDto>();
        }
    }
}
