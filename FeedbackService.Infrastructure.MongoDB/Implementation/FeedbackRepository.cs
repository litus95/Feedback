using AutoMapper;
using FeedbackService.Business.Interfaces;
using FeedbackService.Business.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InfrastructureFeedback = FeedbackService.Infrastructure.MongoDB.Models.Feedback;

namespace FeedbackService.Infrastructure.MongoDB.Implementation
{
    public class FeedbackRepository : IFeedbackRepository
    {
        private readonly IMongoCollection<InfrastructureFeedback> feedbackCollection;
        private readonly IMapper mapper;

        public FeedbackRepository(
            IMongoCollection<InfrastructureFeedback> feedbackCollection,
            IMapper mapper)
        {
            this.feedbackCollection = feedbackCollection;
            this.mapper = mapper;
        }
        public async Task CreateFeedback(Feedback feedback)
        {
            try
            {
                await feedbackCollection.InsertOneAsync(mapper.Map<InfrastructureFeedback>(feedback));
            }
            catch (MongoWriteException ex)
            {
                if (ex.WriteError.Category == ServerErrorCategory.DuplicateKey)
                    throw new Exception("Players can only leave one feedback per session.");
                else
                    throw;
            }
        }

        public async Task<List<Feedback>> GetLastFeedback()
        {
            var feedback = await FindFeedback(Builders<InfrastructureFeedback>.Filter.Empty);
            return mapper.Map<List<Feedback>>(feedback);
        }

        public async Task<List<Feedback>> GetLastFeedbackByRating(int rating)
        {
            var feedback = await FindFeedback(Builders<InfrastructureFeedback>.Filter.Eq(f => f.Rating, rating));
            return mapper.Map<List<Feedback>>(feedback);
        }

        private async Task<List<InfrastructureFeedback>> FindFeedback(FilterDefinition<InfrastructureFeedback> definition)
        {
            return await feedbackCollection
                .Find(definition)
                .Sort(SortByTimestamp())
                .Limit(15)
                .ToListAsync()
                .ConfigureAwait(false);
        }

        private static SortDefinition<InfrastructureFeedback> SortByTimestamp() 
            => Builders<InfrastructureFeedback>.Sort.Descending("Timestamp");
    }
}
