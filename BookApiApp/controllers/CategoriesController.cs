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
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _repo;
        private readonly IMapper _mapper;
        private readonly IBookRepository _bookRepo;
        private readonly IWork _work;

        public CategoriesController(ICategoryRepository repo, IMapper mapper, IBookRepository bookRepo, IWork work)
        {
            _repo = repo;
            _mapper = mapper;
            _bookRepo = bookRepo;
            _work = work;
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

        [HttpGet("{categoryId}", Name = "GetCategory")]
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
            if (!await _bookRepo.BookExistsById(bookId))
            {
                return NotFound("Book does not exist!");
            }

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

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] Category category)
        {
            if (category == null) throw new ArgumentNullException(nameof(category));


            if (await _repo.CategoryExistsByName(category.Name))
            {
                ModelState.AddModelError("", $"This category {category.Name} already exists!");
                return StatusCode(422, ModelState);
            }


            if (!await _work.Add(category))
            {
                ModelState.AddModelError("", $"Something went wrong saving {category.Name}!!");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetCategory", new { categoryId = category.Id }, category);

        }

        [HttpPut("{categoryId}")]
        public async Task<IActionResult> UpdateCategory(int categoryId, [FromBody] Category category)
        {
            if (category == null) throw new ArgumentNullException(nameof(category));

            if (categoryId != category.Id)
                return Unauthorized();

            if (!await _repo.CategoryExists(categoryId))
                return NotFound($"Country {category.Name} does not exist!!");

            if (await _repo.IsCategoryDuplicate(category.Name, categoryId))
            {
                ModelState.AddModelError("", $"Country {category.Name} already exists!");
                return StatusCode(422, ModelState);
            }

            if (!await _work.Update(category))
            {
                ModelState.AddModelError("", $"Something went wrong saving {category.Name}!!");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{categoryId}")]
        public async Task<IActionResult> DeleteCategory(int categoryId)
        {
            if (!await _repo.CategoryExists(categoryId))
            {
                return NotFound("Category does not exist!");
            }

            var books = await _repo.GetBooksForCategory(categoryId);

            if (books.Count > 0)
            {
                ModelState.AddModelError("", "Category cannot be deleted because it is used by at least one author!");
                return StatusCode(409, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await _work.Delete(await _repo.GetCategory(categoryId)))
            {
                ModelState.AddModelError("", "Something went wrong deleting country!!!");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
