using AutoMapper;
using RestaurantAPI.Entities;
using RestaurantAPI.Models;

namespace RestaurantAPI
{
    public class RestaurantMappingProfile : Profile
    {
        public RestaurantMappingProfile()
        {
            CreateMap<Restaurant, RestaurantDto>()
                .ForMember(r => r.Street, 
                c => c.MapFrom(s => s.Address.Street))
                .ForMember(r => r.City,
                c => c.MapFrom(s => s.Address.City))
                .ForMember(r => r.PostalCode,
                c => c.MapFrom(s => s.Address.PostalCode))
                .ReverseMap();

            CreateMap<Dish, DishDto>().ReverseMap();

            //CreateMap<RestaurantDto, Restaurant>()
            //    .ForMember(
            //    r => r.Address.Street, 
            //    c => c.MapFrom(
            //        dto => new Address() 
            //        { 
            //            City = dto.City, 
            //            Street = dto.Street, 
            //            PostalCode = dto.PostalCode
            //        }));

            //CreateMap<DishDto, Dish>();
        }
    }
}
