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
            CreateMap<Category, CategoriesToGetDto>();
            CreateMap<Book, BooksToGetDto>();
            CreateMap<Author, AuthorToGetDto>();
        }
    }
}
