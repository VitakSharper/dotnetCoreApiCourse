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
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewRepository _repo;
        private readonly IMapper _mapper;
        private readonly IBookRepository _bookRepo;
        private readonly IReviewerRepository _reviewerRepo;
        private readonly IWork _work;

        public ReviewsController(IReviewRepository repo, IMapper mapper, IBookRepository bookRepo,
            IReviewerRepository reviewerRepo, IWork work)
        {
            _repo = repo;
            _mapper = mapper;
            _bookRepo = bookRepo;
            _reviewerRepo = reviewerRepo;
            _work = work;
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

        [HttpGet("{reviewId}", Name = "GetReview")]
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

        [HttpPost]
        public async Task<IActionResult> CreateReview([FromBody] Review review)
        {
            if (review == null) throw new ArgumentNullException(nameof(review));


            if (!await _reviewerRepo.ReviewerExists(review.Reviewer.Id))
            {
                ModelState.AddModelError("", "Reviewer doesn't exist!");
            }

            if (!await _bookRepo.BookExistsById(review.Book.Id))
            {
                ModelState.AddModelError("", "Book doesn't exist!");
            }

            if (!ModelState.IsValid)
                return StatusCode(404, ModelState);


            review.Book = await _bookRepo.GetBookById(review.Book.Id);
            review.Reviewer = await _reviewerRepo.GetReviewer(review.Reviewer.Id);


            if (!await _work.Add(review))
            {
                ModelState.AddModelError("", $"Something went wrong saving the review!!");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetReview", new { reviewId = review.Id }, review);
        }
    }
}