using AutoMapper;
using CatalogApi.Models;

namespace CatalogApi.DTOs.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDTO>().ReverseMap();
            CreateMap<Category, CategoryDTO>()
                .ForMember(dto => dto.Product, opt => opt.MapFrom(c => c.Products))
                .ReverseMap();
        }
    }
}
