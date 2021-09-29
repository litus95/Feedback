using AutoMapper;

namespace FeedbackService.Infrastructure.MongoDB.Helpers
{
    public class MongoDbMapperProfile : Profile
    {
        public MongoDbMapperProfile()
        {
            CreateMap<Business.Models.Feedback, Models.Feedback>();
            CreateMap<Models.Feedback, Business.Models.Feedback>()
                .ConstructUsing(f => new Business.Models.Feedback(f.Rating, f.Comment, f.SessionId, f.UserId));
        }
    }
}
