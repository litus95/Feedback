using FeedbackService.Business.Interfaces;
using FeedbackService.Business.Models;
using FeedbackService.Infrastructure.MongoDB.Implementation;
using MongoDB.Driver;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FeedbackService.Infrastructure.Tests
{
    public class FeedbackDatabaseTests : LocalDbTest
    {
        private IFeedbackRepository feedbackRepository;

        [SetUp]
        public void Setup()
        {
            feedbackRepository = new FeedbackRepository(feedbackCollection, mapper);
        }

        [Test]
        public void ShouldCreateFeedback()
        {
            //Arrange
            var feedback = new Feedback(3, "comment", "session", "user");
            //Act
            feedbackRepository.CreateFeedback(feedback);
            var result = feedbackCollection.Find(f => f.SessionId == "session" && f.UserId == "user").FirstOrDefault();
            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Rating, Is.EqualTo(3));
            Assert.That(result.Comment, Is.EqualTo("comment"));
        }

        [Test]
        public async Task ShouldGetLast15Feedback()
        {
            //Arrange
            for (var i = 0; i < 16; i++)
            {
                var feedback = new Feedback(i%5+1, "comment" + i, "session" + i, "user" + i);
                await feedbackRepository.CreateFeedback(feedback);
            }
            //Act
            var result = await feedbackRepository.GetLastFeedback();
            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(15));
            Assert.That(result.FirstOrDefault(f => f.Comment == "comment0"), Is.Null);
            Assert.That(result.FirstOrDefault(f => f.Comment == "comment15"), Is.Not.Null);
        }

        [Test]
        public async Task ShouldThrowExceptionWhenKeyViolation()
        {
            //Arrange
            var feedback1 = new Feedback(3, "comment1", "session", "user");
            var feedback2 = new Feedback(5, "comment2", "session", "user");
            //Act
            await feedbackRepository.CreateFeedback(feedback1);
            try
            {
                await feedbackRepository.CreateFeedback(feedback2);
                Assert.Fail();
            }
            //Assert
            catch
            {
                Assert.Pass();
            }
        }

        [Test]
        public async Task ShouldGetLast15FeedbackByRating()
        {
            //Arrange
            for (var i = 0; i < 16; i++)
            {
                var feedback = new Feedback(i % 5 + 1, "comment" + i, "session" + i, "user" + i);
                await feedbackRepository.CreateFeedback(feedback);
            }
            //Act
            var result = await feedbackRepository.GetLastFeedbackByRating(1);
            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(4));
        }
    }
}