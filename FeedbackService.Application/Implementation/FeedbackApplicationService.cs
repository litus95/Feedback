using AutoMapper;
using FeedbackService.Application.Interfaces;
using FeedbackService.Business.Interfaces;
using FeedbackService.Business.Models;
using FeedbackService.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FeedbackService.Application.Implementation
{
    public class FeedbackApplicationService : IFeedbackApplicationService
    {
        private readonly IFeedbackRepository feedbackRepository;
        private readonly IMapper mapper;

        public FeedbackApplicationService(
            IFeedbackRepository feedbackRepository,
            IMapper mapper)
        {
            this.feedbackRepository = feedbackRepository;
            this.mapper = mapper;
        }
        public async Task CreateFeedback(string sessionId, string userId, FeedbackDto feedback)
        {
            var businessFeedback = new Feedback(feedback.Rating, feedback.Comment, sessionId, userId);
            await feedbackRepository.CreateFeedback(businessFeedback);
        }

        public async Task<List<FeedbackDto>> GetFeedback(int? rating)
        {
            var feedback = rating.HasValue
                ? await feedbackRepository.GetLastFeedbackByRating(rating.Value)
                : await feedbackRepository.GetLastFeedback();
            return mapper.Map<List<FeedbackDto>>(feedback);

        }
    }
}
