using FeedbackService.DTO;
using FeedbackService.Infrastructure.MongoDB.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MongoDB.Driver;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FeedbackService.Integration.Tests
{
    public class FeedbackServiceTests
    {
        private readonly HttpClient _client;
        private IMongoDatabase _database;
        public FeedbackServiceTests()
        {
            var appFactory = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        UpdateDatabase(services);
                    });
                });
            _client = appFactory.CreateClient();
        }

        [TearDown]
        public void Teardown()
        {
            _database.DropCollection("Feedback");
        }

        [Test]
        public async Task CreateFeedback_ReturnsSuccessIfInformationIsFilled()
        {
            //Arrange
            var feedbackDto = new FeedbackDto()
            {
                Comment = "Comment1",
                Rating = 1
            };
            var httpContent = CreateJsonContent(feedbackDto);
            httpContent.Headers.Add("Ubi-UserId", "User1");
            //Act
            var response = await _client.PostAsync("/api/feedback/session1", httpContent);
            //Assert
            Assert.That(response.IsSuccessStatusCode, Is.True);
        }

        [Test]
        public async Task CreateFeedback_ReturnsFailureIfInformationIsMissing()
        {
            //Arrange
            var feedbackDto = new FeedbackDto()
            {
                Comment = "Comment1",
                Rating = 1
            };
            var httpContent = CreateJsonContent(feedbackDto);
            //Act
            var response = await _client.PostAsync("/api/feedback/session1", httpContent);
            //Assert
            Assert.That(response.IsSuccessStatusCode, Is.False);
        }

        [Test]
        public async Task CreateFeedback_ReturnsFailureIfRatingIsMising()
        {
            //Arrange
            var feedbackDto = new FeedbackDto()
            {
                Comment = "Comment1"
            };
            var httpContent = CreateJsonContent(feedbackDto);
            httpContent.Headers.Add("Ubi-UserId", "User1");
            //Act
            var response = await _client.PostAsync("/api/feedback/session1", httpContent);
            //Assert
            Assert.That(response.IsSuccessStatusCode, Is.False);
        }

        [Test]
        public async Task GetFeedback_ReturnsEmptyIfNoFeedbackCreated()
        {
            //Arrange
            //Act
            var response = await _client.GetAsync("/api/feedback");
            var feedbacks = JsonConvert.DeserializeObject<List<FeedbackDto>>(await response.Content.ReadAsStringAsync());
            //Assert
            Assert.That(response.IsSuccessStatusCode, Is.True);
            Assert.That(feedbacks.Count, Is.EqualTo(0));
        }

        [Test]
        public async Task GetFeedback_ReturnsIfPreviouslyCreated()
        {
            //Arrange
            var feedbackDto = new FeedbackDto()
            {
                Comment = "Comment1",
                Rating = 1
            };
            var httpContent = CreateJsonContent(feedbackDto);
            httpContent.Headers.Add("Ubi-UserId", "User1");
            //Act
            await _client.PostAsync("/api/feedback/session1", httpContent);
            var response = await _client.GetAsync("/api/feedback");
            var feedbacks = JsonConvert.DeserializeObject<List<FeedbackDto>>(await response.Content.ReadAsStringAsync());
            //Assert
            Assert.That(response.IsSuccessStatusCode, Is.True);
            Assert.That(feedbacks.Count, Is.EqualTo(1));
        }

        private void UpdateDatabase(IServiceCollection services)
        {
            services.RemoveAll(typeof(IMongoClient));
            services.RemoveAll(typeof(IMongoDatabase));
            services.RemoveAll(typeof(IMongoCollection<Feedback>));

            var clientSettings = MongoClientSettings.FromConnectionString("mongodb://localhost:27017");
            clientSettings.MaxConnectionIdleTime = TimeSpan.FromMinutes(1);

            var mongoClient = new MongoClient(clientSettings);
            _database = mongoClient.GetDatabase("FeedbackIntegrationTests");

            services.AddSingleton<IMongoClient>(mongoClient);
            services.AddSingleton(_database);
            services.AddSingleton(serviceProvider => _database.GetCollection<Feedback>("Feedback"));
        }

        private HttpContent CreateJsonContent(string json)
        {
            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        private HttpContent CreateJsonContent(object content)
        {
            return CreateJsonContent(JsonConvert.SerializeObject(content));
        }
    }
}
