using System;
using System.Collections.Generic;
using GuessTheNumberEF.Data.Utils;
using GuessTheNumberEF.Data.Entities;

namespace GuessTheNumberEF.Logic
{
    public static class GameManager
    {
        static GameUnitOfWork gm = new GameUnitOfWork();

        public static string IsActiveMessage()
        {
            return gm.Games.GetCurrent() != null ? MessageManager.GameStateActive : MessageManager.GameStateNone;
        }

        public static string StartResult(string userName, int number)
        {
            Game currentGame = gm.Games.GetCurrent();
            if (currentGame != null)
            {
                currentGame.WinnerName = null;
                currentGame.End = DateTime.Now;
                gm.Games.Update(currentGame);
                gm.Save();
                currentGame = new Game()
                {
                    Number = number,
                    AuthorName = userName,
                    Start = DateTime.Now,
                    PlayersCount = 1
                };
                gm.Games.Create(currentGame);
                gm.Save();
                return MessageManager.GameStarted;
            }
            currentGame = new Game()
            {
                Number = number,
                AuthorName = userName,
                Start = DateTime.Now,
                PlayersCount = 1
            };
            gm.Games.Create(currentGame);
            gm.Save();
            return MessageManager.GameStarted;
        }

        public static string GuessResult(string userName, int number)
        {
            Game currentGame = gm.Games.GetCurrent();
            if (currentGame != null)
            {
                if (!gm.Games.UserInGame(userName, currentGame.Id))
                {
                    currentGame.PlayersCount += 1;
                }
                if(currentGame.Log == null)
                {
                    currentGame.Log = new List<LogEntry>();
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
                gm.Save();
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
                gm.Games.Update(currentGame);
                gm.Save();
                return MessageManager.GuessResultWin;
            }
            return MessageManager.GameStateNone;
        }

    }
}
