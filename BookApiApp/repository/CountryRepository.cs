using BookApiApp.models;
using BookApiApp.services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookApiApp.repository
{
    public class CountryRepository : ICountryRepository
    {
        private readonly BookDbContext _context;
        private readonly IWork _work;

        public CountryRepository(BookDbContext context)
        {
            _context = context;
        }

        public async Task<ICollection<Country>> GetCountries()
        {
            return await _context.Countries.OrderBy(c => c.Name).ToListAsync();
        }

        public async Task<Country> GetCountry(int countryId)
        {
            return await _context.Countries.SingleOrDefaultAsync(c => c.Id == countryId);
        }

        public async Task<Country> GetCountryOfAnAuthor(int authorId)
        {
            return await _context.Authors.Where(a => a.Id == authorId).Select(c => c.Country).SingleOrDefaultAsync();
        }

        public async Task<ICollection<Author>> GetAuthorsFromACountry(int countryId)
        {
            return await _context.Authors.Where(a => a.Country.Id == countryId).ToListAsync();
        }

        public async Task<bool> CountryExist(int countryId)
        {
            return await _context.Countries.AnyAsync(c => c.Id == countryId);
        }

        public async Task<bool> CountryExistByName(string countryName)
        {
            return await _context.Countries.AnyAsync(c => c.Name == countryName.ToUpperInvariant().Trim());
        }

        public async Task<bool> IsCountryDuplicate(string countryName, int countryId)
        {
            return await _context.Countries.AnyAsync(c =>
                string.Equals(c.Name, countryName.Trim(), StringComparison.InvariantCultureIgnoreCase) &&
                c.Id != countryId);
        }
    }
}