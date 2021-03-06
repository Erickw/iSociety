﻿using CryptSharp;
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
                return View(usuarioSelecionado);
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
            var campoAluguel = fieldList.First();
            return View(campoAluguel); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Reservar(CampoAluguel campAluguel)
        {
            if (ModelState.IsValid)
            {

                var logged = TempData["UsuarioConsumidor"] as UsuarioConsumidor;
                TempData.Keep();
                ViewBag.userID = logged.Id;

                var queryAluguel = new QueryUsuarioFornecedor();
                var aluguel = queryAluguel.ListarAluguelPorId(campAluguel.aluguelId);
                aluguel.First().reponsavelId = campAluguel.responsavelId;
                queryAluguel.AlterarAluguel(aluguel.First());

                return View("SucessoReserva");
            }
            return View("Painel");
        }

        public ActionResult VerReservas(int id)
        {
            var field = new QueryUsuarioConsumidor();
            var fieldList = field.ListarCamposAlugueisPorIdUsuario(id);
            var logged = new UsuarioConsumidor { Id = id };
            TempData["UsuarioConsumidor"] = logged;
            foreach (var item in fieldList)
            {
                item.responsavelId = id;
            }
            return View(fieldList);
        }

        public ActionResult ConfirmaReserva(int id)
        {
            var logged = TempData["UsuarioConsumidor"] as UsuarioConsumidor;
            TempData.Keep();

            var queryReserva = new QueryUsuarioConsumidor();
            var reservaList = queryReserva.ListarCamposAlugueisPorId(id);                                   
            reservaList.First().responsavelId = logged.Id;

            var campoAluguelTD = new CampoAluguel { aluguelId = id};
            TempData["CampoAluguel"] = campoAluguelTD;

            var pagamento = new Pagamento {
                idConsumidor = reservaList.First().responsavelId,
                horarioReservado = reservaList.First().horarioInicio,
                idAdministrador = queryReserva.ListarIDAdmNPorNomeCampo(reservaList.First().nomeCampo)                
            };
            return View(pagamento);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmaReserva(Pagamento pgto)
        {
            var campoAluguelTD = TempData["CampoAluguel"] as CampoAluguel;
            TempData.Keep();
            var queryPagamento = new QueryUsuarioConsumidor();
            if (string.IsNullOrEmpty(pgto.formaPagamento))
            {
                ViewBag.AluguelID = campoAluguelTD.aluguelId;
                return View("ValidacaoPagamento");
            }         

            if (ModelState.IsValid)
            {
                var aluguelConfirmado = new Aluguel {
                    idAluguel = campoAluguelTD.aluguelId,
                    idPagamento = (queryPagamento.ListarIdPagamento() + 1),
                    confirmado = true
            };
                queryPagamento.InserirPagamento(pgto);
                queryPagamento.ConfirmarAluguel(aluguelConfirmado);                

                return View("SucessoReserva");
            }
            
            return View();
        }
        
        public ActionResult CancelaReserva(int id)
        {
            var field = new QueryUsuarioConsumidor();
            var fieldList = field.ListarCamposAlugueisPorId(id);
            var campoAluguel = fieldList.First();
            campoAluguel.aluguelId = id;
            return View(campoAluguel);
        }
        
        [HttpPost, ActionName("CancelaReserva")]
        [ValidateAntiForgeryToken]
        public ActionResult CancelaReservaConfirma(int id)
        {
            var queryCancelamento = new QueryUsuarioConsumidor();
            if (ModelState.IsValid)
            {
                var logged = TempData["UsuarioConsumidor"] as UsuarioConsumidor;
                TempData.Keep();
                ViewBag.userID = logged.Id;
                queryCancelamento.CancelarAluguel(id);

                return View("SucessoCancelamento");
            }
            return View();
        }

        public ActionResult VerPeladas(int id) {

            var field = new QueryUsuarioConsumidor();
            var fieldList = field.ListarPeladas(id);
            foreach (var item in fieldList)
            {
                item.responsavelId = id;
            }
            return View(fieldList);
        }
        public ActionResult Alert()
        {
            return View();
        }
    }


}