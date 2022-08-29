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
            CreateMap<User, RegisterUserDto>().ReverseMap();
        }
    }
}
