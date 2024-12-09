using AutoMapper;
using CosmeticShop.Domain.Entities;
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
            CreateMap<CartItem, CartItemResponseDto>();
        }
    }
}
