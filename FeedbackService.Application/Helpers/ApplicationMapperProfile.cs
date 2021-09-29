using AutoMapper;
using FeedbackService.Business.Models;
using FeedbackService.DTO;

namespace FeedbackService.Application.Helpers
{
    public class ApplicationMapperProfile : Profile
    {
        public ApplicationMapperProfile()
        {
            CreateMap<Feedback, FeedbackDto>();
            CreateMap<FeedbackDto, Feedback>();
        }
    }
}
