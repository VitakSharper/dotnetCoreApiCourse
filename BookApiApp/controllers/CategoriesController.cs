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
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _repo;
        private readonly IMapper _mapper;

        public CategoriesController(ICategoryRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _repo.GetCategories();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var categoriesToReturn = _mapper.Map<ICollection<CategoriesToGetDto>>(categories);

            return Ok(categoriesToReturn);
        }

        [HttpGet("{categoryId}")]

        public async Task<IActionResult> GetCategory(int categoryId)
        {
            if (!await _repo.CategoryExists(categoryId))
            {
                return NotFound("Category don't exist!");
            }

            var category = await _repo.GetCategory(categoryId);

            var categoryToReturn = _mapper.Map<CategoriesToGetDto>(category);

            return Ok(categoryToReturn);

        }

        [HttpGet("book/{bookId}")]
        public async Task<IActionResult> GetCategoriesOfABook(int bookId)
        {

            var categoriesOfABook = await _repo.GetCategoriesOfABook(bookId);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var categoriesToReturn = _mapper.Map<ICollection<CategoriesToGetDto>>(categoriesOfABook);

            return Ok(categoriesToReturn);

        }

        [HttpGet("{categoryId}/books")]
        public async Task<IActionResult> GetBooksForCategory(int categoryId)
        {
            if (!await _repo.CategoryExists(categoryId))
                return NotFound("Category not found!");

            var books = await _repo.GetBooksForCategory(categoryId);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var booksToReturn = _mapper.Map<ICollection<BooksToGetDto>>(books);

            return Ok(booksToReturn);

        }

    }
}
