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
    public class BookController : ControllerBase
    {
        private readonly IBookRepository _repo;
        private readonly IMapper _mapper;

        public BookController(IBookRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetBooks()
        {
            var books = await _repo.GetBooks();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var booksToReturn = _mapper.Map<ICollection<BooksToGetDto>>(books);

            return Ok(booksToReturn);
        }

        [HttpGet("{bookId}")]
        public async Task<IActionResult> GetBookById(int bookId)
        {
            if (!await _repo.BookExistsById(bookId))
            {
                NotFound("Book does not exist!");
            }

            var book = await _repo.GetBookById(bookId);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var bookToReturn = _mapper.Map<BooksToGetDto>(book);

            return Ok(bookToReturn);
        }

        [HttpGet("isbn/{isbn}")]
        public async Task<IActionResult> GetBookByIsbn(string isbn)
        {
            if (!await _repo.BookExistsByIsbn(isbn))
            {
                return NotFound("Book by isbn does not exist!");
            }

            var book = await _repo.GetBookByIsbn(isbn);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var bookToReturn = _mapper.Map<BooksToGetDto>(book);

            return Ok(bookToReturn);
        }

        [HttpGet("rating/{bookId}")]
        public async Task<IActionResult> GetBookRating(int bookId)
        {
            if (!await _repo.BookExistsById(bookId))
            {
                return NotFound("Book does not exist!");
            }

            var bookRating = await _repo.GetBookRating(bookId);


            return Ok(bookRating);
        }

    }

}
