using System.Web.Mvc;

namespace Knowwaste.Areas.New
{
    public class NewAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get { return "New"; }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "New_default",
                "New/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
