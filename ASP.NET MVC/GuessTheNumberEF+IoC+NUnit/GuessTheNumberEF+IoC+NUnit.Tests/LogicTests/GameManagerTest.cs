using NUnit.Framework;
using Moq;

using GuessTheNumberEF.Logic;
using GuessTheNumberEF.Data.Utils;
using GuessTheNumberEF.Data.Entities;
using System.Collections.Generic;
using System;

namespace GuessTheNumberEF_IoC_NUnit.Tests.LogicTests
{
    [TestFixture]
    public class GameManagerTest
    {
        [Test]
        public void GetCurrentGame_GetGame_ReturnsGame()
        {
            var mock = new Mock<IRepository<Game>>();
            IGameManager manager = new GameManagerMVC(mock.Object);
            mock.Setup(a => a.GetAll()).Returns(new List<Game>() { new Game {Id=1 } });
            var result = manager.GetCurrentGame();
            Assert.IsInstanceOf<Game>(result);
        }
        [Test]
        public void GetCurrentGame_GetGame_ReturnsNull()
        {
            var mock = new Mock<IRepository<Game>>();
            IGameManager manager = new GameManagerMVC(mock.Object);
            mock.Setup(a => a.GetAll()).Returns(new List<Game>() { new Game { End = DateTime.Now } });
            var result = manager.GetCurrentGame();
            Assert.IsNull(result);
        }
        [Test]
        public void UserInGame_CheckUser_ReturnsFalse()
        {
            var mock = new Mock<IRepository<Game>>();
            IGameManager manager = new GameManagerMVC(mock.Object);
            mock.Setup(a => a.GetAll()).Returns(new List<Game>() { new Game { Id = 1, AuthorName = "test" } });
            mock.Setup(a=>a.Get(0).Log).Returns(new List<LogEntry>(){ new LogEntry { GameId = 1, PlayerName = "test" } });
            var result = manager.UserInGame("name", 0);
            Assert.IsFalse(result);
        }
        [Test]
        public void UserInGame_CheckUser_ReturnsTrue()
        {
            var mock = new Mock<IRepository<Game>>();
            IGameManager manager = new GameManagerMVC(mock.Object);
            mock.Setup(a => a.GetAll()).Returns(new List<Game>() { new Game { Id = 1, AuthorName = "test" } });
            mock.Setup(a => a.Get(0).Log).Returns(new List<LogEntry>() { new LogEntry { GameId = 1, PlayerName = "test" } });
            var result = manager.UserInGame("test", 0);
            Assert.IsTrue(result);
        }
        [Test]
        public void StartResult_GameStarted_ReturnsMessage()
        {
            var mock = new Mock<IRepository<Game>>();
            IGameManager manager = new GameManagerMVC(mock.Object);
            mock.Setup(a => a.GetAll()).Returns(new List<Game>());
            var result = manager.StartResult("test", 1);
            Assert.AreEqual(MessageManager.GameStarted, result);
        }
        [Test]
        public void GetAllGames_Get_ReturnsCollection()
        {
            var mock = new Mock<IRepository<Game>>();
            IGameManager manager = new GameManagerMVC(mock.Object);
            mock.Setup(a => a.GetAll()).Returns(new List<Game>());
            var result = manager.GetAllGames();
            Assert.IsInstanceOf<IEnumerable<Game>>(result);
        }
        [Test]
        public void GetOneGame_Get_ReturnsInstance()
        {
            var mock = new Mock<IRepository<Game>>();
            IGameManager manager = new GameManagerMVC(mock.Object);
            mock.Setup(a => a.Get(1)).Returns(new Game());
            var nullResult = manager.GetOneGame(2);
            var gameResult = manager.GetOneGame(1);
            Assert.IsNull(nullResult);
            Assert.IsInstanceOf<Game>(gameResult);
        }
        [Test]
        public void IsActiveMessage_Check_ReturnsMessageActive()
        {
            var mock = new Mock<IRepository<Game>>();
            IGameManager manager = new GameManagerMVC(mock.Object);
            mock.Setup(a => a.GetAll()).Returns(new List<Game>() { new Game { Id = 1, AuthorName = "test" } });
            var result = manager.IsActiveMessage();
            Assert.AreEqual(MessageManager.GameStateActive, result);
        }
        [Test]
        public void IsActiveMessage_Check_ReturnsMessageNone()
        {
            var mock = new Mock<IRepository<Game>>();
            IGameManager manager = new GameManagerMVC(mock.Object);
            mock.Setup(a => a.GetAll()).Returns(new List<Game>() { new Game { End = DateTime.Now } });
            var result = manager.IsActiveMessage();
            Assert.AreEqual(MessageManager.GameStateNone, result);
        }
        [Test]
        public void GuessResult_Guess_Less()
        {
            var mock = new Mock<IRepository<Game>>();
            IGameManager manager = new GameManagerMVC(mock.Object);
            mock.Setup(a => a.GetAll()).Returns(new List<Game>() { new Game { Id = 1, AuthorName = "test", Number = 7 } });
            mock.Setup(a => a.Get(0).Log).Returns(new List<LogEntry>() { new LogEntry { GameId = 1, PlayerName = "test" } });
            var result = manager.GuessResult("test", 6);
            Assert.AreEqual(MessageManager.GuessResultLess, result);      
        }
        [Test]
        public void GuessResult_Guess_More()
        {
            var mock = new Mock<IRepository<Game>>();
            IGameManager manager = new GameManagerMVC(mock.Object);
            mock.Setup(a => a.GetAll()).Returns(new List<Game>() { new Game { Id = 1, AuthorName = "test", Number = 7 } });
            mock.Setup(a => a.Get(0).Log).Returns(new List<LogEntry>() { new LogEntry { GameId = 1, PlayerName = "test" } });
            var result = manager.GuessResult("test", 8);
            Assert.AreEqual(MessageManager.GuessResultMore, result);
        }
        [Test]
        public void GuessResult_Guess_Win()
        {
            var mock = new Mock<IRepository<Game>>();
            IGameManager manager = new GameManagerMVC(mock.Object);
            mock.Setup(a => a.GetAll()).Returns(new List<Game>() { new Game { Id = 1, AuthorName = "test", Number = 7 } });
            mock.Setup(a => a.Get(0).Log).Returns(new List<LogEntry>() { new LogEntry { GameId = 1, PlayerName = "test" } });
            var result = manager.GuessResult("test", 7);
            Assert.AreEqual(MessageManager.GuessResultWin, result);
        }
        [Test]
        public void GuessResult_Guess_NoGame()
        {
            var mock = new Mock<IRepository<Game>>();
            IGameManager manager = new GameManagerMVC(mock.Object);
            mock.Setup(a => a.GetAll()).Returns(new List<Game>() { new Game { Id = 1, AuthorName = "test", Number = 7, End = DateTime.Now } });
            mock.Setup(a => a.Get(0).Log).Returns(new List<LogEntry>() { new LogEntry { GameId = 1, PlayerName = "test" } });
            var result = manager.GuessResult("test", 7);
            Assert.AreEqual(MessageManager.GameStateNone, result);
        }

    }
}
