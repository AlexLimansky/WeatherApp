using NUnit.Framework;
using Moq;
using GuessTheNumberEF.MVC.Controllers;
using System.Web.Mvc;
using GuessTheNumberEF.Logic;

namespace GuessTheNumberEF_IoC_NUnit.Tests.MVCTests
{
    [TestFixture]
    public class GameControllerTest
    {
        [Test]
        public void Index_GetViewResult_ReturnsNotNull()
        {
            var gameMock = new Mock<IGameManager>();
            var authMock = new Mock<IAuthManager>();
            GameController controller = new GameController(gameMock.Object, authMock.Object);
            ViewResult result = controller.Index() as ViewResult;
            Assert.IsNotNull(result);
        }
        [Test]
        public void SetNumber_Try_ReturnsStart()
        {
            var gameMock = new Mock<IGameManager>();
            var authMock = new Mock<IAuthManager>();
            GameController controller = new GameController(gameMock.Object, authMock.Object);
            gameMock.Setup(a => a.StartResult(authMock.Object.GetUserName(), 7)).Returns(MessageManager.GameStarted);
            JsonResult result = controller.SetNumber(7, "start") as JsonResult;
            Assert.AreEqual(MessageManager.GameStarted, result.Data.ToString());
        }
        [Test]
        public void SetNumber_Try_ReturnsGuess()
        {
            var gameMock = new Mock<IGameManager>();
            var authMock = new Mock<IAuthManager>();
            GameController controller = new GameController(gameMock.Object, authMock.Object);
            gameMock.Setup(a => a.GuessResult(authMock.Object.GetUserName(), 7)).Returns(MessageManager.GuessResultLess);
            JsonResult result = controller.SetNumber(7, "guess") as JsonResult;
            Assert.AreEqual(MessageManager.GuessResultLess, result.Data.ToString());
        }
    }
}
