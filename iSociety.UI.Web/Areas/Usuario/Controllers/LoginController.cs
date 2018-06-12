using iSociety.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iSociety.UI.Web.Areas.Usuario.Controllers
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
                TempData["u"] = user;
                return RedirectToAction("Painel");
            //"~/Areas/Usuario/Views/Painel/Index.cshtml"
            //return RedirectToRoute(new { controller = "Login", action = "MeuPerfil", id = UrlParameter.Optional });
            return View("Alert");
           
        }

        public ActionResult Painel()
        {
            if (TempData["u"] != null)
            {
                var user = TempData["u"] as UsuarioConsumidor;
                var users = new QueryUsuarioConsumidor();
                var usersList = users.ListarPorId(user.Id);
                var usuarioSelecionado = usersList[0];
                if (usersList == null)
                {
                    return HttpNotFound();
                }
                return View(usuarioSelecionado);
            }
            return View();
        }
        
        public ActionResult Alert()
        {
            return View();
        }

    }


}