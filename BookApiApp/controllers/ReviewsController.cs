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
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewRepository _repo;
        private readonly IMapper _mapper;
        private readonly IBookRepository _bookRepo;

        public ReviewsController(IReviewRepository repo, IMapper mapper, IBookRepository bookRepo)
        {
            _repo = repo;
            _mapper = mapper;
            _bookRepo = bookRepo;
        }


        [HttpGet]
        public async Task<IActionResult> GetReviews()
        {
            var reviews = await _repo.GetReviews();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var reviewsToReturn = _mapper.Map<ICollection<ReviewToGetDto>>(reviews);

            return Ok(reviewsToReturn);

        }

        [HttpGet("{reviewId}")]
        public async Task<IActionResult> GetReview(int reviewId)
        {
            if (!await _repo.ReviewExists(reviewId))
            {
                return NotFound("Review does not exist!");
            }

            var review = await _repo.GetReview(reviewId);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var reviewToReturn = _mapper.Map<ReviewToGetDto>(review);

            return Ok(reviewToReturn);
        }

        [HttpGet("reviews/{bookId}")]
        public async Task<IActionResult> GetReviewsOfABook(int bookId)
        {
            if (!await _bookRepo.BookExistsById(bookId))
            {
                return NotFound("Book is not found!");
            }

            var reviews = await _repo.GetReviewsOfABook(bookId);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var reviewToReturn = _mapper.Map<ICollection<ReviewToGetDto>>(reviews);

            return Ok(reviewToReturn);

        }

        [HttpGet("book/{reviewId}")]
        public async Task<IActionResult> GetBookOfAReview(int reviewId)
        {
            if (!await _repo.ReviewExists(reviewId))
            {
                return NotFound("Reviews with this ID does not exist!");
            }

            var book = await _repo.GetBookOfAReview(reviewId);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var bookToReturn = _mapper.Map<BooksToGetDto>(book);

            return Ok(bookToReturn);
        }

    }
}
