using FeedbackService.Infrastructure.MongoDB.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace FeedbackService.Infrastructure.MongoDB.Indices
{
    class FeedbackIndexProvider : IndexProvider<Feedback>
    {
        public FeedbackIndexProvider(IMongoDatabase database, string collectionName) : base(database, collectionName)
        {

        }

        public override IEnumerable<CreateIndexModel<Feedback>> GetIndices()
        {
            yield return CreateUniquePlayerPerSessionIndex();
        }

        private CreateIndexModel<Feedback> CreateUniquePlayerPerSessionIndex()
        {
            var unique = new IndexKeysDefinitionBuilder<Feedback>()
                .Ascending(f => f.UserId)
                .Ascending(f => f.SessionId);
            return new CreateIndexModel<Feedback>(unique, new CreateIndexOptions { Background = true, Name = "Unique_UserId_SessionId", Unique = true });
        }
    }
}
