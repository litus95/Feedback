using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace FeedbackService.Infrastructure.MongoDB.Models
{
    public class Feedback
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime Timestamp { get; set; }
        public string SessionId { get; set; }
        public string UserId { get; set; }
    }
}
