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

            var countriesToReturn = _mapper.Map<IEnumerable<CountriesToGetDto>>(countries);

            return Ok(countriesToReturn);
        }
    }
}
