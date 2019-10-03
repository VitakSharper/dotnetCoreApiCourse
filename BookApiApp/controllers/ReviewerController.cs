using AutoMapper;
using BookApiApp.Dtos;
using BookApiApp.repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookApiApp.controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewerController : ControllerBase
    {
        private readonly IReviewerRepository _repo;
        private readonly IReviewRepository _rRepo;
        private readonly IMapper _mapper;

        public ReviewerController(IReviewerRepository repo, IReviewRepository rRepo, IMapper mapper)
        {
            _repo = repo;
            _rRepo = rRepo;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<IActionResult> GetReviewers()
        {
            var reviewers = await _repo.GetReviewers();

            var reviewersToReturn = _mapper.Map<ICollection<ReviewerToGetDto>>(reviewers);

            return Ok(reviewersToReturn);
        }

        [HttpGet("{reviewerId}")]
        public async Task<IActionResult> GetReviewer(int reviewerId)
        {
            if (reviewerId <= 0) throw new ArgumentOutOfRangeException(nameof(reviewerId));

            if (!await _repo.ReviewerExists(reviewerId))
                return NotFound("Reviewer does not exist!");

            var reviewer = await _repo.GetReviewer(reviewerId);

            var reviewerToReturn = _mapper.Map<ReviewerToGetDto>(reviewer);

            return Ok(reviewerToReturn);
        }

        [HttpGet("{reviewerId}/reviews")]
        public async Task<IActionResult> GetReviewsByReviewer(int reviewerId)
        {
            if (reviewerId <= 0) throw new ArgumentOutOfRangeException(nameof(reviewerId));

            if (!await _repo.ReviewerExists(reviewerId))
                return NotFound("Reviewer does not exist!");

            var reviewsToReturn =
                _mapper.Map<ICollection<ReviewToGetDto>>(await _repo.GetReviewsByReviewer(reviewerId));

            return Ok(reviewsToReturn);
        }

        [HttpGet("{reviewId}/reviewer")]
        public async Task<IActionResult> GetReviewerOfAReview(int reviewId)
        {
            if (reviewId <= 0) throw new ArgumentOutOfRangeException(nameof(reviewId));

            if (!await _rRepo.ReviewExists(reviewId))
                return NotFound("The review does not exists!");

            var reviewerToReturn = _mapper.Map<ReviewerToGetDto>(await _repo.GetReviewerOfAReview(reviewId));

            if (!await _repo.ReviewerExists(reviewerToReturn.Id))
                return NotFound("Reviewer Not Found!");

            return Ok(reviewerToReturn);
        }
    }
}