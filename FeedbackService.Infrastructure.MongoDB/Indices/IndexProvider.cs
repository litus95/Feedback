using MongoDB.Driver;
using System.Collections.Generic;

namespace FeedbackService.Infrastructure.MongoDB.Indices
{
    internal abstract class IndexProvider<T>
    {
        private readonly IMongoCollection<T> collection;

        protected IndexProvider(IMongoDatabase database, string collectionName)
        {
            collection = database.GetCollection<T>(collectionName);
        }

        public void ApplyIndices()
        {
            foreach (var index in GetIndices())
            {
                ApplyIndex(index);
            }
        }

        private void ApplyIndex(CreateIndexModel<T> index)
        {
            collection.Indexes.CreateOne(index);
        }

        public abstract IEnumerable<CreateIndexModel<T>> GetIndices();
    }
}
