using System.Collections.Generic;
using System.Threading.Tasks;
using FeedbackService.Business.Models;

namespace FeedbackService.Business.Interfaces
{
    public interface IFeedbackRepository
    {
        Task CreateFeedback(Feedback feedback);
        Task<List<Feedback>> GetLastFeedbackByRating(int rating);
        Task<List<Feedback>> GetLastFeedback();
    }
}
