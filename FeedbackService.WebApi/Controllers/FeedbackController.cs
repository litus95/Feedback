using FeedbackService.Application.Interfaces;
using FeedbackService.DTO;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FeedbackService.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackController : Controller
    {
        const string userIdHeaderName = "Ubi-UserId";
        private readonly IFeedbackApplicationService feedbackApplicationService;

        public FeedbackController(IFeedbackApplicationService feedbackApplicationService)
        {
            this.feedbackApplicationService = feedbackApplicationService;
        }

        /// <summary>
        /// Get the last 15 feedbacks left by users.
        /// </summary>
        /// <param name="rating"> Filters the rating of the feedback </param>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<FeedbackDto>> GetFeedback(int? rating)
        {
            if (rating.HasValue && (rating < 1 || rating > 5))
                throw new Exception("Rating must be a number between 1 and 5");
            return await feedbackApplicationService.GetFeedback(rating);
        }

        /// <summary>
        /// Creates a new feedback for a session.
        /// </summary>
        /// <param name="sessionId"> Id of the session that we want to rate</param>
        /// <param name="feedback"> Rating and comment</param>
        /// <returns></returns>
        [HttpPost("{sessionId}")]
        public async Task CreateFeedback(string sessionId, [FromBody] FeedbackDto feedback)
        {
            if (Request.Headers.TryGetValue(userIdHeaderName, out var userId))
            {
                await feedbackApplicationService.CreateFeedback(sessionId, userId, feedback);
            }
            else
                throw new Exception("Missing User Id from headers");
        }
    }
}
