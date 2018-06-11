using iSociety.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iSociety.UI.Web.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(UsuarioConsumidor user)
        {
            var usuario = new QueryUsuarioConsumidor();
            if (usuario.ValidaUser(user))
                return RedirectToRoute(new { controller = "Home", action = "About", id = UrlParameter.Optional });
            return View("Alert");
        }

        public ActionResult Alert()
        {
            return View();
        }

    }


}