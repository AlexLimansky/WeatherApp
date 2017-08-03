using System;
using System.Collections.Generic;
using System.Linq;
using GuessTheNumberEF.Data.Utils;
using GuessTheNumberEF.Data.Entities;

namespace GuessTheNumberEF.Logic
{
    public class GameManagerMVC : IGameManager
    {
        private IRepository<Game> repo;

        public GameManagerMVC(IRepository<Game> data)
        {
            repo = data;
        }

        public Game GetCurrentGame()
        {
            return repo.GetAll().FirstOrDefault(g => g.End == null);
        }

        public bool UserInGame(string userName, int gameId)
        {
            bool isAuthor = repo.GetAll().Any(g => g.AuthorName == userName && g.Id == gameId);
            bool isContributor = repo.Get(gameId).Log.Any(l => l.PlayerName == userName);
            return isAuthor || isContributor;
        }

        public string IsActiveMessage()
        {
            return GetCurrentGame() != null ? MessageManager.GameStateActive : MessageManager.GameStateNone;
        }

        public string StartResult(string userName, int number)
        {
            Game currentGame = GetCurrentGame();
            if (currentGame != null)
            {
                currentGame.WinnerName = null;
                currentGame.End = DateTime.Now;
                repo.Update(currentGame);
                currentGame = new Game()
                {
                    Number = number,
                    AuthorName = userName,
                    Start = DateTime.Now,
                    PlayersCount = 1
                };
                repo.Create(currentGame);
                return MessageManager.GameStarted;
            }
            currentGame = new Game()
            {
                Number = number,
                AuthorName = userName,
                Start = DateTime.Now,
                PlayersCount = 1
            };
            repo.Create(currentGame);
            return MessageManager.GameStarted;
        }

        public string GuessResult(string userName, int number)
        {
            Game currentGame = GetCurrentGame();
            if (currentGame != null)
            {
                if (currentGame.Log == null)
                {
                    currentGame.Log = new List<LogEntry>();
                }
                if (!UserInGame(userName, currentGame.Id))
                {
                    currentGame.PlayersCount += 1;
                }                             
                LogEntry entry = new LogEntry()
                {
                    Game = currentGame,
                    GameId = currentGame.Id,
                    Number = number,
                    Time = DateTime.Now,
                    PlayerName = userName,                 
                };
                currentGame.Log.Add(entry);
                if (number < currentGame.Number)
                {
                    return MessageManager.GuessResultLess;
                }
                if (number > currentGame.Number)
                {
                    return MessageManager.GuessResultMore;
                }
                currentGame.End = DateTime.Now;
                currentGame.WinnerName = userName;
                repo.Update(currentGame);
                return MessageManager.GuessResultWin;
            }
            return MessageManager.GameStateNone;
        }

        public IEnumerable<Game> GetAllGames()
        {
            return repo.GetAll().OrderByDescending(g=>g.End);
        }

        public Game GetOneGame(int gameId)
        {
            return repo.Get(gameId);
        }
    }
}
