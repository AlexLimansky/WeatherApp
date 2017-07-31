using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using GuessTheNumberWebAPI.Utils;
using GuessTheNumberWebAPI.Models;
using GuessTheNumberWebAPI.Hubs;

namespace GuessTheNumberWebAPI.Controllers
{
    [RoutePrefix("game")]
    public class GameController : ApiController
    {
        [Route("start/{number}")]
        [HttpPost]
        public IHttpActionResult StartGame(int number)
        {
            Game.Number = number;
            string name = GetPlayerName();
            Game.Author = name;
            SendStartMessage(Resourses.MessageNewGame(name));
            return Ok();
        }

        [Route("guess/{number}")]
        [HttpPost]
        public IHttpActionResult Guess(int number)
        {
            int currentNumber = (int)Game.Number;
            LogEntry newEntry = new LogEntry
            {
                playerProposed = Game.Author,
                playerGuessed = GetPlayerName(),
                yourNumber = number,
                realNumber = (int)Game.Number,
                time = DateTime.Now
            };
            Game.Log.Add(newEntry);
            if (currentNumber < number)
            {
                return Ok(Resourses.MoveResultMore);
            }
            else if (currentNumber > number)
            {
                return Ok(Resourses.MoveResultLess);
            }
            else
            {
                Game.Number = null;
                SendEndMessage(Resourses.MessageGameEnded(GetPlayerName()));
                return Ok("Win");
            }
        }

        [Route("log")]
        [HttpGet]
        public IHttpActionResult GetLog()
        {
            return Ok(Game.Log);
        }

        [NonAction]
        private void SendStartMessage(string message)
        {
            var context = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<GameHub>();
            context.Clients.All.gameStarted(message);
        }

        [NonAction]
        private void SendEndMessage(string message)
        {
            var context = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<GameHub>();
            context.Clients.All.gameFinished(message);
        }

        private string GetPlayerName()
        {
            CookieHeaderValue cookie = Request.Headers.GetCookies(Resourses.CookieName).FirstOrDefault();
            string playerName = cookie[Resourses.CookieName].Value;
            return playerName;
        }
    }
}
