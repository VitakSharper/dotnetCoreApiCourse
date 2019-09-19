﻿using BookApiApp.models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookApiApp.repository
{
    interface ICountryRepository
    {
        Task<ICollection<Country>> GetCountries();
        Task<Country> GetCountry(int countryId);
        Task<Country> GetCountryOfAnAuthor(int authorId);
        Task<ICollection<Author>> GetAuthorsFromACountry(int countryId);
        Task<bool> CountryExist(int countryId);
    }
}
