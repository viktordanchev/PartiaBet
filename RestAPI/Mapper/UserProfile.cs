using AutoMapper;
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
        }
    }
}
