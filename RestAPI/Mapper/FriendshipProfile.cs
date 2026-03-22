using AutoMapper;
using Core.Models.Friendship;
using RestAPI.Dtos.Friendship;

namespace RestAPI.Mapper
{
    public class FriendshipProfile : Profile
    {
        public FriendshipProfile()
        {
            CreateMap<FriendModel, FriendDto>();
            CreateMap<PlayerDataModel, PlayerDataDto>();
            CreateMap<GameStatsModel, GameStatsDto>();
        }
    }
}
