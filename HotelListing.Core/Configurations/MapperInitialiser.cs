using HotelListing.Data;
using HotelListing.Core.DTOs;
using AutoMapper;

namespace HotelListing.Core.Configurations
{
    public class MapperInitialiser : Profile
    {
        public MapperInitialiser()
        {
            CreateMap<Country, CountryDTO>().ReverseMap();
            CreateMap<Country, CreateCountryDTO>().ReverseMap();
            CreateMap<Hotel, HotelDTO>().ReverseMap();
            CreateMap<Hotel, CreateHotelDTO>().ReverseMap();

            // Identity
            CreateMap<ApiUser, LoginUserDTO>().ReverseMap();
            CreateMap<ApiUser, UserDTO>().ReverseMap();
        }
    }
}
