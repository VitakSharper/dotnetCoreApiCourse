using AutoMapper;
using BookApiApp.Dtos;
using BookApiApp.repository;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookApiApp.controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly ICountryRepository _repo;
        private readonly IMapper _mapper;

        public CountriesController(ICountryRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<IActionResult> GetCountries()
        {
            var countries = await _repo.GetCountries();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var countriesToReturn = _mapper.Map<ICollection<CountriesToGetDto>>(countries);

            return Ok(countriesToReturn);
        }

        [HttpGet("{countryId}")]
        public async Task<IActionResult> GetCountry(int countryId)
        {
            if (!await _repo.CountryExist(countryId))
                return NotFound("This country is not found!");

            var country = await _repo.GetCountry(countryId);

            var countryToReturn = _mapper.Map<CountriesToGetDto>(country);

            return Ok(countryToReturn);

        }

        [HttpGet("authors/{authorId}")]
        public async Task<IActionResult> GetCountryOfAnAuthor(int authorId)
        {
            //TO DO Validate the author exists

            var country = await _repo.GetCountryOfAnAuthor(authorId);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var countryToReturn = _mapper.Map<CountriesToGetDto>(country);

            return Ok(countryToReturn);

        }

        // TO DO GetAuthorsFromACountry

        [HttpGet("{countryId}/authors")]
        public async Task<IActionResult> GetAuthorsFromACountry(int countryId)
        {
            if (!await _repo.CountryExist(countryId))
                return NotFound("Country not exists!");
            var authors = await _repo.GetAuthorsFromACountry(countryId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var authorsToReturn = _mapper.Map<ICollection<AuthorToGetDto>>(authors);

            return Ok(authorsToReturn);
        }
    }
}
