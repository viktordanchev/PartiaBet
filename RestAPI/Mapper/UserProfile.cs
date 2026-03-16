using AutoMapper;
using Core.Models.Friendship;
using Core.Models.User;
using RestAPI.Dtos.User;

namespace RestAPI.Mapper
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<RegisterUserDto, RegisterUserModel>();
            CreateMap<LoginUserDto, LoginUserModel>();
            CreateMap<UserDataModel, UserDataDto>();
            CreateMap<FriendModel, FriendDto>();
        }
    }
}
