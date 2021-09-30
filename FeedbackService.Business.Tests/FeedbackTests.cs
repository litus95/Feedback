using FeedbackService.Business.Models;
using NUnit.Framework;
using static FeedbackService.Common.Contracts;

namespace FeedbackService.Business.Tests
{
    public class FeedbackTests
    {

        [Test]
        public void FeedbackThrowsExceptionWhenNotCorrect()
        {
            Assert.Throws<ContractException>(() => new Feedback(6, "random", "session1", "user1"));
            Assert.Throws<ContractException>(() => new Feedback(5, "random", null, "user1"));
            Assert.Throws<ContractException>(() => new Feedback(5, "random", "session1", null));
            Assert.Throws<ContractException>(() => new Feedback(0, "random", "session1", "user1"));
        }
    }
}