using AutoMapper;
using BookApiApp.Dtos;
using BookApiApp.models;
using BookApiApp.repository;
using Microsoft.AspNetCore.Mvc;
using System;
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
        private readonly IWork _work;

        public CountriesController(ICountryRepository repo, IMapper mapper, IWork work)
        {
            _repo = repo;
            _mapper = mapper;
            _work = work;
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

        [HttpGet("{countryId}", Name = "GetCountry")]
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

        [HttpPost]
        public async Task<IActionResult> CreateCountry([FromBody]Country country)
        {
            if (country == null) throw new ArgumentNullException(nameof(country));

            if (await _repo.CountryExistByName(country.Name))
            {
                ModelState.AddModelError("", $"Country {country.Name} already exists!");
                return StatusCode(422, $"Country {country.Name} already exists!");
            }


            if (!await _work.Add(country))
            {
                ModelState.AddModelError("", $"Something went wrong saving {country.Name}!!");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetCountry", new { countryId = country.Id }, country);
        }

        [HttpPut("{countryId}")]
        public async Task<IActionResult> UpdateCountry(int countryId, [FromBody] Country country)
        {
            if (country == null) throw new ArgumentNullException(nameof(country));

            if (countryId != country.Id)
                return Unauthorized();

            if (!await _repo.CountryExist(countryId))
                return NotFound($"Country {country.Name} does not exist!!");

            if (await _repo.IsCountryDuplicate(country.Name, countryId))
            {
                ModelState.AddModelError("", $"Country {country.Name} already exists!");
                return StatusCode(422, $"Country {country.Name} already exists!");
            }

            if (await _work.Update(country)) return NoContent();
            ModelState.AddModelError("", $"Something went wrong saving {country.Name}!!");
            return StatusCode(500, ModelState);
        }

        [HttpDelete("{countryId}")]
        public async Task<IActionResult> DeleteCountry(int countryId)
        {
            if (!await _repo.CountryExist(countryId))
                return NotFound("Country does not exists!");

            var authors = await _repo.GetAuthorsFromACountry(countryId);

            if (authors.Count > 0)
            {
                ModelState.AddModelError("", $"Country cannot be deleted because it is used by at least one author!");
                return StatusCode(409, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await _work.Delete(await _repo.GetCountry(countryId)))
            {
                ModelState.AddModelError("", $"Something went wrong deleting country!!!");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
