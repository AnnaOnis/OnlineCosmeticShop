using CosmeticShop.Domain.Entities;
using HttpModels.Responses;
using AutoMapper;
using HttpModels.Requests;

namespace Mappers
{
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            CreateMap<Product, ProductResponseDto>();
            CreateMap<ProductRequestDto, Product>();
        }
    }
}
