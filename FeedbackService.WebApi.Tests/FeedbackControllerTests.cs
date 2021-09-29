using FeedbackService.Application.Interfaces;
using FeedbackService.WebApi.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace FeedbackService.WebApi.Tests
{
    public class FeedbackControllerTests
    {
        private Mock<IFeedbackApplicationService> feedbackApplicationServiceMock;

        [SetUp]
        public void Setup()
        {
            feedbackApplicationServiceMock = new Mock<IFeedbackApplicationService>();
        }

        [Test]
        public async Task ShouldCallCreateFeedback()
        {
            //Arrange
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Ubi-UserId"] = "user1";
            var controller = new FeedbackController(feedbackApplicationServiceMock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext
                }
            };
            var feedbackDto = new DTO.FeedbackDto()
            {
                Rating = 1,
                Comment = "test"
            };
            //Act
            await controller.CreateFeedback("session1", feedbackDto);
            //Assert
            feedbackApplicationServiceMock.Verify(s => s.CreateFeedback("session1", "user1", feedbackDto), Times.Once);
        }

        [Test]
        public void ShouldThrowMissingUserId()
        {
            //Arrange
            var feedbackDto = new DTO.FeedbackDto()
            {
                Rating = 1,
                Comment = "test"
            };
            var httpContext = new DefaultHttpContext();
            var controller = new FeedbackController(feedbackApplicationServiceMock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext
                }
            };
            //Act
            //Assert
            Assert.ThrowsAsync<Exception>(async () => await controller.CreateFeedback("session1", feedbackDto));
        }

        [Test]
        public void ShouldThrowRatingOutOfRange()
        {
            //Arrange
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Ubi-UserId"] = "user1";
            var controller = new FeedbackController(feedbackApplicationServiceMock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext
                }
            };
            //Act
            //Assert
            Assert.ThrowsAsync<Exception>(async () => await controller.GetFeedback(0));
            Assert.ThrowsAsync<Exception>(async () => await controller.GetFeedback(6));
        }

        [Test]
        public async Task ShouldCallGetFeedback()
        {
            //Arrange
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Ubi-UserId"] = "user1";
            var controller = new FeedbackController(feedbackApplicationServiceMock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext
                }
            };
            //Act
            await controller.GetFeedback(1);
            //Assert
            feedbackApplicationServiceMock.Verify(s => s.GetFeedback(1), Times.Once);
        }
    }
}