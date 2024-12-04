using AutoMapper;
using CosmeticShop.Domain.Entities;
using HttpModels.Responses;

namespace Mappers
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile() 
        {
            CreateMap<User, UserResponseDto>();
        }
    }
}
