using FeedbackService.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FeedbackService.Application.Interfaces
{
    public interface IFeedbackApplicationService
    {
        Task CreateFeedback(string sessionId, string userId, FeedbackDto feedback);
        Task<List<FeedbackDto>> GetFeedback(int? rating);
    }
}
