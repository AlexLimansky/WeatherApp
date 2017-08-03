using NUnit.Framework;
using Moq;
using GuessTheNumberEF.MVC.Controllers;
using System.Web.Mvc;
using GuessTheNumberEF.Logic;

namespace GuessTheNumberEF_IoC_NUnit.Tests.LogicTests
{
    [TestFixture]
    public class ArchiveControllerTest
    {
        [Test]
        public void Index_GetViewResult_ReturnsNotNull()
        {
            var gameMock = new Mock<IGameManager>();
            var authMock = new Mock<IAuthManager>();
            ArchiveController controller = new ArchiveController(gameMock.Object, authMock.Object);
            ViewResult result = controller.Games() as ViewResult;
            Assert.IsNotNull(result);
        }
        [Test]
        public void GameDetails_GetViewResult_ReturnsNotNull()
        {
            var gameMock = new Mock<IGameManager>();
            var authMock = new Mock<IAuthManager>();
            ArchiveController controller = new ArchiveController(gameMock.Object, authMock.Object);
            ViewResult result = controller.GameDetails(3) as ViewResult;
            Assert.IsNotNull(result);
        }
    }
}
