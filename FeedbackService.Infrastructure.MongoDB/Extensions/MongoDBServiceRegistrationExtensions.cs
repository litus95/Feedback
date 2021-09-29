using FeedbackService.Business.Interfaces;
using FeedbackService.Infrastructure.MongoDB.Configuration;
using FeedbackService.Infrastructure.MongoDB.Implementation;
using FeedbackService.Infrastructure.MongoDB.Indices;
using FeedbackService.Infrastructure.MongoDB.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System;

namespace FeedbackService.Infrastructure.MongoDB.Extensions
{
    public static class MongoDBServiceRegistrationExtensions
    {
        public static void ConfigureMongoDbRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            var mongoDbConfiguration = GetMongoDbConfiguration(configuration);

            var mongoClient = CreateMongoClient(mongoDbConfiguration);
            services.AddSingleton<IMongoClient>(mongoClient);

            var database = mongoClient.GetDatabase(mongoDbConfiguration.DatabaseName);
            services.AddSingleton<IMongoDatabase>(database);

            services.AddSingleton<IFeedbackRepository, FeedbackRepository>();

            RegisterCollections(services, database);
            EnsureIndices(database);
        }

        private static void EnsureIndices(IMongoDatabase database)
        {
            new FeedbackIndexProvider(database, "Feedback").ApplyIndices();
        }

        private static MongoDbConfiguration GetMongoDbConfiguration(IConfiguration configuration)
        {
            var mongoDbConfiguration = new MongoDbConfiguration();
            configuration.GetSection("MongoDbConnection").Bind(mongoDbConfiguration);
            return mongoDbConfiguration;
        }

        private static MongoClient CreateMongoClient(MongoDbConfiguration configuration)
        {
            var clientSettings = MongoClientSettings.FromConnectionString(configuration.ConnectionString);
            clientSettings.MaxConnectionIdleTime = TimeSpan.FromMinutes(1);

            return new MongoClient(clientSettings);
        }

        private static void RegisterCollections(IServiceCollection services, IMongoDatabase database)
        {
            RegisterCollection<Feedback>("Feedback");

            void RegisterCollection<T>(string collectionName)
            {
                services.AddSingleton(serviceProvider => database.GetCollection<T>(collectionName));
            }
        }
    }
}
