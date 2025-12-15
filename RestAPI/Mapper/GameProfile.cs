using AutoMapper;
using Core.Models.Games;
using RestAPI.Dtos.Game;

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
