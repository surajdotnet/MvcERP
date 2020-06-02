using System.Web.Mvc;

namespace ERP.Web.Areas.WebSpace
{
    public class WebSpaceAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "WebSpace";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "WebSpace_default",
                "WebSpace/{controller}/{action}/{id}",
                new { Controller= "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}