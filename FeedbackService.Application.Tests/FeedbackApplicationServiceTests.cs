using AutoMapper;
using FeedbackService.Application.Implementation;
using FeedbackService.Business.Interfaces;
using FeedbackService.Business.Models;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace FeedbackService.Application.Tests
{
    public class FeedbackApplicationServiceTests
    {
        Mock<IFeedbackRepository> feedbackRepository;
        Mock<IMapper> mapper;

        [SetUp]
        public void Setup()
        {
            feedbackRepository = new Mock<IFeedbackRepository>();
            mapper = new Mock<IMapper>();
        }

        [Test]
        public async Task ShouldCallCreateFeedback()
        {
            // Arrange
            var service = new FeedbackApplicationService(feedbackRepository.Object, mapper.Object);
            // Act
            await service.CreateFeedback("session1", "user1", new DTO.FeedbackDto()
            {
                Rating = 1,
                Comment = "test"
            });
            // Assert
            feedbackRepository.Verify(s => s.CreateFeedback(It.IsAny<Feedback>()), Times.Once);
        }

        [Test]
        public async Task ShouldCallGetFeedback()
        {
            // Arrange
            var service = new FeedbackApplicationService(feedbackRepository.Object, mapper.Object);
            // Act
            await service.GetFeedback(null);
            // Assert
            feedbackRepository.Verify(s => s.GetLastFeedback(), Times.Once);
        }

        [Test]
        public async Task ShouldCallGetFeedbackByRating()
        {
            // Arrange
            var service = new FeedbackApplicationService(feedbackRepository.Object, mapper.Object);
            // Act
            await service.GetFeedback(1);
            // Assert
            feedbackRepository.Verify(s => s.GetLastFeedbackByRating(1), Times.Once);
        }
    }
}