using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CosmeticShop.Domain.Entities;
using HttpModels.Responses;

namespace Mappers
{
    public class FavoriteMapperProfile : Profile
    {
        public FavoriteMapperProfile() 
        {
            CreateMap<Favorite, FavoriteResponseDto>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.ProductDescription, opt => opt.MapFrom(src => src.Product.Description))
                .ForMember(dest => dest.ProductPrice, opt => opt.MapFrom(src => src.Product.Price))
                .ForMember(dest => dest.ProductRating, opt => opt.MapFrom(src => src.Product.Rating))
                .ForMember(dest => dest.ProductImageUrl, opt => opt.MapFrom(src => src.Product.ImageUrl));
        }
    }
}
