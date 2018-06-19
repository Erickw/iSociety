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

        // GET: Login
        public ActionResult Index()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(UsuarioFornecedor user)
        {
            var logado = new UsuarioFornecedor
            {
                Id = user.Id,
                Nome = user.Nome,
                email = user.email,
                Senha = user.Senha,
                contaBanco = user.contaBanco
            };
            var usuario = new QueryUsuarioFornecedor();
            if (usuario.ValidaUserMaster(user))
            {
                return RedirectToAction("Cadastrar");
            }
            return View("Alert");
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
                return View("SucessoAdm");
            }
            return View(user);
        }
    }
}