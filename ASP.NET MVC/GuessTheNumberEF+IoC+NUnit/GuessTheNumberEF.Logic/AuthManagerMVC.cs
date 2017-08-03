using System.Web.Mvc;
using System.Security.Principal;

namespace GuessTheNumberEF.Logic
{
    public class AuthManagerMVC : IAuthManager
    {
        private IPrincipal currentUser;
        public AuthManagerMVC(IPrincipal user)
        {
            currentUser = user;
        }

        public string GetUserName()
        {
            return currentUser.Identity.Name;
        }
    }
}
