using AutoMapper;
using FeedbackService.Infrastructure.MongoDB.Helpers;
using FeedbackService.Infrastructure.MongoDB.Models;
using MongoDB.Driver;
using NUnit.Framework;
using System.Threading.Tasks;

namespace FeedbackService.Infrastructure.Tests
{
    public class LocalDbTest
    {
        private const string databaseName = "FeedbackTest";
        private IMongoDatabase database;
        protected IMapper mapper;

        protected IMongoCollection<Feedback> feedbackCollection;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            var client = new MongoClient("mongodb://localhost:27017");
            database = client.GetDatabase(databaseName);
            mapper = CreateMapper();
        }

        private IMapper CreateMapper()
        {
            var profile = new MongoDbMapperProfile();
            var mapperConfiguration = new MapperConfiguration(c => c.AddProfile(profile));
            return mapperConfiguration.CreateMapper();
        }

        [SetUp]
        public async Task SetupBase()
        {
            await database.DropCollectionAsync("Feedback");

            feedbackCollection = database.GetCollection<Feedback>("Feedback");
        }
    }
}
