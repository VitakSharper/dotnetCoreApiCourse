using AutoMapper;
using BookApiApp.Dtos;
using BookApiApp.models;

namespace BookApiApp.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Country, CountriesToGetDto>();
        }
    }
}
