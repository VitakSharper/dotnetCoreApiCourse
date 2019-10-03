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
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorRepository _repo;
        private readonly IMapper _mapper;
        private readonly IBookRepository _bookRepo;

        public AuthorController(IAuthorRepository repo, IMapper mapper, IBookRepository bookRepo)
        {
            _repo = repo;
            _mapper = mapper;
            _bookRepo = bookRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAuthors()
        {
            var authors = await _repo.GetAuthors();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var authorsToReturn = _mapper.Map<ICollection<AuthorToGetDto>>(authors);

            return Ok(authorsToReturn);
        }

        [HttpGet("{authorId}")]
        public async Task<IActionResult> GetAuthorById(int authorId)
        {
            if (!await _repo.AuthorExists(authorId))
            {
                return NotFound("Author does not exists!");
            }

            var author = await _repo.GetAuthor(authorId);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var authorToReturn = _mapper.Map<AuthorToGetDto>(author);

            return Ok(authorToReturn);
        }

        [HttpGet("book/{bookId}")]
        public async Task<IActionResult> GetAuthorsOfABook(int bookId)
        {
            if (!await _bookRepo.BookExistsById(bookId))
            {
                return NotFound("Book does not exist!");
            }

            var authors = await _repo.GetAuthorsOfABook(bookId);

            if (!ModelState.IsValid)
            {
                BadRequest(ModelState);
            }

            var authorsToReturn = _mapper.Map<ICollection<AuthorToGetDto>>(authors);

            return Ok(authorsToReturn);
        }

        [HttpGet("books/{authorId}")]
        public async Task<IActionResult> GetBooksByAuthor(int authorId)
        {
            if (!await _repo.AuthorExists(authorId))
            {
                return NotFound("Author does not exist!");
            }

            var books = await _repo.GetBooksByAuthor(authorId);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var booksToReturn = _mapper.Map<ICollection<BooksToGetDto>>(books);

            return Ok(booksToReturn);

        }
    }
}
