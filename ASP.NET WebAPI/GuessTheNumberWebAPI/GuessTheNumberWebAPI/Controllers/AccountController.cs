using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using GuessTheNumberWebAPI.Utils;

namespace GuessTheNumberWebAPI.Controllers
{
    [RoutePrefix("account")]
    public class AccountController : ApiController
    {
        [Route("check")]
        [HttpGet]
        public HttpResponseMessage CheckCookie()
        {
            CookieHeaderValue cookie = Request.Headers.GetCookies(Resourses.CookieName).FirstOrDefault();
            return Request.CreateResponse(HttpStatusCode.OK, cookie != null);
        }

        [Route("getname")]
        public HttpResponseMessage GetName()
        {
            CookieHeaderValue cookie = Request.Headers.GetCookies(Resourses.CookieName).FirstOrDefault();
            string playerName = cookie != null ? playerName = cookie[Resourses.CookieName].Value : null;
            var response = Request.CreateResponse(HttpStatusCode.OK, Resourses.MessageGreeting(playerName));
            return response;
        }

        [Route("login/{name}")]
        [HttpPost]
        public HttpResponseMessage Login(string name)
        {
            var response = Request.CreateResponse(HttpStatusCode.OK, "Hello!");
            CookieHeaderValue cookie = Request.Headers.GetCookies(Resourses.CookieName).FirstOrDefault();
            if (cookie != null)
            {
                string playerName = cookie[Resourses.CookieName].Value;
                response = Request.CreateResponse(HttpStatusCode.OK, Resourses.MessageGreeting(playerName));
                return response;
            }
            var newCookie = new CookieHeaderValue(Resourses.CookieName, name)
            {
                Expires = DateTimeOffset.Now.AddDays(1),
                Domain = Request.RequestUri.Host,
                Path = "/"
            };
            response.Headers.AddCookies(new[] { newCookie });
            return response;
        }

        [Route("logout")]
        [HttpPost]
        public HttpResponseMessage Logout()
        {
            var response = Request.CreateResponse(HttpStatusCode.OK);
            string playerName = Request.Headers.GetCookies(Resourses.CookieName).FirstOrDefault()[Resourses.CookieName].Value;
            var newCookie = new CookieHeaderValue(Resourses.CookieName, playerName)
            {
                Expires = DateTimeOffset.Now.AddDays(-1),
                Domain = Request.RequestUri.Host,
                Path = "/"
            };
            return response;
        }
    }
}
