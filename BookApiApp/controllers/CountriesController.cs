using BookApiApp.repository;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BookApiApp.controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly ICountryRepository _repo;

        public CountriesController(ICountryRepository repo)
        {
            _repo = repo;
        }


        [HttpGet]
        public async Task<IActionResult> GetCountries()
        {
            var countries = await _repo.GetCountries();



            return Ok(countries);
        }
    }
}
