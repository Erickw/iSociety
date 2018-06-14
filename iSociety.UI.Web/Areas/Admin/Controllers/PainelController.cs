using iSociety.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iSociety.UI.Web.Areas.Admin.Controllers
{
    public class PainelController : Controller
    {
       
        // GET: Usuario/Painel
        public ActionResult Index(String id)
        {
            return View();
        }

        //public ActionResult MeuPerfil(int id)
        //{
        //    var users = new QueryUsuarioConsumidor();
        //    var usersList = users.ListarPorId(id);
        //    var usuarioSelecionado = usersList[0];

        //    return View(usuarioSelecionado);
        //}

    }
}