using System.Web.Mvc;

namespace iSociety.UI.Web.Areas.Master
{
    public class MasterAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Master";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Master_default",
                "Master/{controller}/{action}/{id}",
                new { controller = "GerenciaAdmin", action = "Cadastrar", id = UrlParameter.Optional }
            );
        }
    }
}