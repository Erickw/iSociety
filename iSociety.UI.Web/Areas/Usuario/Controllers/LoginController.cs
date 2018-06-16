using CryptSharp;
using iSociety.Areas.Admin.Models;
using iSociety.Models;
using iSociety.UI.Web.Models;
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
            var logado = new UsuarioConsumidor
            {
                Id = user.Id,
                Nome = user.Nome,
                email = user.email,
                Senha = user.Senha,
                contaBanco = user.contaBanco
            };
            var usuario = new QueryUsuarioConsumidor();
            if (usuario.ValidaUser(user))
            {
                TempData["UsuarioConsumidor"] = logado;
                return RedirectToAction("Painel");
            }
            //"~/Areas/Usuario/Views/Painel/Index.cshtml"
            //return RedirectToRoute(new { controller = "Login", action = "MeuPerfil", id = UrlParameter.Optional });
            return View("Alert");
        }

        [HttpGet]
        public ActionResult Painel()
        {
            if (TempData["UsuarioConsumidor"] != null)
            {
                var logado = TempData["UsuarioConsumidor"] as UsuarioConsumidor;
                TempData.Keep();
                var users = new QueryUsuarioConsumidor();
                List<UsuarioConsumidor> usersList = users.ListarPorNome(logado.Nome);                
                if (usersList.Count() == 0)
                {
                    return View("Alert");
                }
                UsuarioConsumidor usuarioSelecionado = usersList[0];
                //ViewBag.Nome = usuarioSelecionado.Id;
                return View(usuarioSelecionado);
                //return View();
            }
            return View();

        }

        public ActionResult Detalhes(int id)
        {
            var users = new QueryUsuarioConsumidor();
            var usersList = users.ListarPorId(id);
            var usuarioSelecionado = usersList[0];

            return View(usuarioSelecionado);
        }
        
        public ActionResult Editar(int id)
        {
            var users = new QueryUsuarioConsumidor();
            var usersList = users.ListarPorId(id);
            //var usuarioSelecionado = usersList[0];
            if (usersList == null)
            {
                return HttpNotFound();
            }
            return View(usersList.First());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(UsuarioConsumidor user)
        {
            if (ModelState.IsValid)
            {
                string senhaHash = Crypter.Blowfish.Crypt(user.Senha);
                var usuario = new QueryUsuarioConsumidor();
                user.Senha = senhaHash;
                usuario.Alterar(user);
                return RedirectToAction("Index");
            }
            return View(user);
        }

        public ActionResult ListarCampoAluguel(int id)
        {           
            var field = new QueryUsuarioConsumidor();
            var fieldList = field.ListarCamposAlugueis();
            var logged = new UsuarioConsumidor {Id = id};
            TempData["UsuarioConsumidor"] = logged;
            return View(fieldList);
        }

        public ActionResult Reservar(int id)
        {
            var logged = TempData["UsuarioConsumidor"] as UsuarioConsumidor;
            TempData.Keep();
            var field = new QueryUsuarioConsumidor();
            var fieldList = field.ListarCamposAlugueisPorId(id);
            fieldList.First().responsavelId = logged.Id;                 

            return View(fieldList.First()); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Reservar(CampoAluguel campAluguel)
        {
            if (ModelState.IsValid)
            {
                var queryAluguel = new QueryUsuarioFornecedor();
                var aluguel = queryAluguel.ListarAluguelPorId(campAluguel.aluguelId);
                aluguel.First().reponsavelId = campAluguel.responsavelId;
                queryAluguel.AlterarAluguel(aluguel.First());

                return View("SucessoReserva");
            }
            return View("Alert");
        }

        public ActionResult Alert()
        {
            return View();
        }

    }


}