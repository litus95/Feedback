using FeedbackService.Common;
using System;

namespace FeedbackService.Business.Models
{
    public class Feedback
    {
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime Timestamp { get; set; }
        public string SessionId { get; set; }
        public string UserId { get; set; }

        protected Feedback() { }

        public Feedback(int rating, string comment, string sessionId, string userId) : this()
        {
            Rating = rating;
            Comment = comment;
            SessionId = sessionId;
            UserId = userId;
            Timestamp = DateTime.Now;
            EnsureValidState();
        }

        private void EnsureValidState()
        {
            Contracts.Require(!string.IsNullOrWhiteSpace(SessionId), "SessionId must be informed");
            Contracts.Require(!string.IsNullOrWhiteSpace(UserId), "UserId must be informed");
            Contracts.Require(Rating >= 1 && Rating <= 5, "Rating must be between 1 and 5.");
        }
    }
}
