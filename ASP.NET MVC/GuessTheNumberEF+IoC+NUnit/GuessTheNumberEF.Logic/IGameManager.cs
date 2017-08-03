using System.Collections.Generic;
using GuessTheNumberEF.Data.Entities;

namespace GuessTheNumberEF.Logic
{
    public interface IGameManager
    {
        Game GetCurrentGame();
        bool UserInGame(string userName, int gameId);
        string IsActiveMessage();
        string StartResult(string userName, int number);
        string GuessResult(string userName, int number);
        IEnumerable<Game> GetAllGames();
        Game GetOneGame(int gameId);             
    }
}
