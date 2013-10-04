using System.Web.Mvc;

namespace TellToAsk.Areas.LoggedUser
{
    public class LoggedUserAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "LoggedUser";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "LoggedUser_custom",
                "Logged/{action}/{id}",
                new { controller = "LoggedUser", action = "Index", id = UrlParameter.Optional }
            );

            //context.MapRoute(
            //    "LoggedUser_default",
            //    "LoggedUser/{controller}/{action}/{id}",
            //    new { action = "Index", id = UrlParameter.Optional }
            //);
            
        }
    }
}