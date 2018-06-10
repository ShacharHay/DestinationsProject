using System.Web;
using ApplicationsProject.Models;

namespace ApplicationsProject.Controllers
{
    public static class AuthorizationAdmin
    {
        public static bool AdminAuthorized(HttpSessionStateBase session)
        {
            return Authorized(session) && ((Client)session["Client"]).IsAdmin;
        }

        public static bool Authorized(HttpSessionStateBase session)
        {
            return (Client)session["Client"] != null;
        }
    }
}