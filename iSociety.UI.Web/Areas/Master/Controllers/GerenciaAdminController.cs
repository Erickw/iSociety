using CryptSharp;
using iSociety.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iSociety.UI.Web.Areas.Master.Controllers
{
    public class GerenciaAdminController : Controller
    {
        // GET: Master/GerenciaAdmin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Cadastrar()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastrar(UsuarioFornecedor user)
        {
            if (ModelState.IsValid)
            {
                string senhaHash = Crypter.Blowfish.Crypt(user.Senha);
                var usuario = new QueryUsuarioFornecedor();
                user.Senha = senhaHash;
                usuario.Inserir(user);              
                return RedirectToAction("Index");
            }
            return View(user);
        }
    }
}