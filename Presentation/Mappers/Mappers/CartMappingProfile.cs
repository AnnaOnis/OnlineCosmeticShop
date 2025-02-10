using AutoMapper;
using CosmeticShop.Domain.Entities;
using HttpModels.Requests;
using HttpModels.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mappers
{
    public class CartMappingProfile : Profile
    {
        public CartMappingProfile() 
        { 
            CreateMap<Cart, CartResponseDto>();
            CreateMap<CartItem, CartItemResponseDto>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.ProductPrice, opt => opt.MapFrom(src => src.Product.Price))
                .ForMember(dest => dest.ProductImageUrl, opt => opt.MapFrom(src => src.Product.ImageUrl)); ;
            CreateMap<CartItemRequestDto, CartItem>();
        }
    }
}
