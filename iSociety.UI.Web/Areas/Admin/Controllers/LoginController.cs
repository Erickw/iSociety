using CryptSharp;
using iSociety.Areas.Admin.Models;
using iSociety.Models;
using iSociety.UI.Web.Areas.Admin.Models;
using iSociety.UI.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iSociety.UI.Web.Areas.Admin.Controllers
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
            if (usuario.ValidaUser(user))
            {
                TempData["UsuarioFornecedor"] = logado;
                return RedirectToAction("Painel");
            }
            //"~/Areas/Usuario/Views/Painel/Index.cshtml"
            //return RedirectToRoute(new { controller = "Login", action = "MeuPerfil", id = UrlParameter.Optional });
            return View("Alert");
        }

        [HttpGet]
        public ActionResult Painel()
        {
            if (TempData["UsuarioFornecedor"] != null)
            {
                var logado = TempData["UsuarioFornecedor"] as UsuarioFornecedor;          
                TempData.Keep();
                var users = new QueryUsuarioFornecedor();
                List<UsuarioFornecedor> usersList = users.ListarPorNome(logado.Nome);                
                if (usersList.Count() == 0)
                {
                    return View("Alert");
                }
                UsuarioFornecedor usuarioSelecionado = usersList[0];
                //ViewBag.Nome = usuarioSelecionado.Id;
                return View(usuarioSelecionado);
                //return View();
            }
            return View();
        }

        public ActionResult Detalhes(int id)
        {
            var users = new QueryUsuarioFornecedor();
            var usersList = users.ListarPorId(id);
            var usuarioSelecionado = usersList[0];
            var usuarioSelecionado2 = usersList[0];

            return View(usuarioSelecionado);
        }
        
        public ActionResult Editar(int id)
        {
            var users = new QueryUsuarioFornecedor();
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
        public ActionResult Editar(UsuarioFornecedor user)
        {
            if (ModelState.IsValid)
            {
                string senhaHash = Crypter.Blowfish.Crypt(user.Senha);
                var usuario = new QueryUsuarioFornecedor();
                user.Senha = senhaHash;
                usuario.Alterar(user);
                return RedirectToAction("Index");
            }
            return View(user);
        }

        public ActionResult AdicionarCampo(int id) {

            var campo = new Campo();
            campo.idAdministrador = id;
            
            return View(campo);  
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdicionarCampo(Campo campo)
        {
            if (ModelState.IsValid)
            {
                var field = new QueryUsuarioFornecedor();
                field.InserirCampo(campo);               
                return RedirectToAction("CampoAdicionado");
            }
            return View();
         }
          
        public ActionResult EditarCampo(int id) {
            var field = new QueryUsuarioFornecedor();
            var fieldList = field.ListarCamposPorId(id);
            return View(fieldList.AsEnumerable());
        }
        
        public ActionResult EditCampo(int id)
        {
            var field = new QueryUsuarioFornecedor();
            var fieldList = field.SelecionaCampo(id);          
            if (fieldList == null)
            {
                return HttpNotFound();
            }
            return View(fieldList);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCampo(Campo campo)
        {
            if (ModelState.IsValid)
            {
                var field = new QueryUsuarioFornecedor();
                field.AlterarCampo(campo);
                return RedirectToAction("Painel");
            }
            return View(campo);
        }

        public ActionResult AdicionarHorario(int id)
        {
            var campo = new QueryUsuarioFornecedor();
            var campoEscolhido = campo.SelecionaCampo(id);
            Horario hora = new Horario();
            hora.idCampo = campoEscolhido.id;
            return View(hora);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdicionarHorario(Horario hora)
        {
            int i = hora.idCampo;
            var h = hora.horarios;
            if (ModelState.IsValid)
            {                
                var horario = new QueryUsuarioFornecedor();
                if (horario.VerificaHorario(hora)) {
                    horario.AdicionarHorario(hora);
                    return RedirectToAction("HorarioAdicionado");
                }
            }
            return RedirectToAction("HorarioExistente");
        }

        public ActionResult AdicionarAluguel(int id) {
            var campo = new QueryUsuarioFornecedor();
            var campoEscolhido = campo.SelecionaCampo(id);
            Aluguel aluguel = new Aluguel();
            aluguel.idCampo = campoEscolhido.id;
            return View(aluguel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdicionarAluguel(Aluguel aluguel)
        {
            if (ModelState.IsValid)
            {
                var queryAluguel = new QueryUsuarioFornecedor();
                var hora = new Horario {
                    idCampo = aluguel.idCampo,
                    horarios = aluguel.horarioInicio                    
                };
                
                if (!queryAluguel.VerificaHorario(hora))
                {   
                    queryAluguel.CriarAluguel(aluguel);
                    queryAluguel.ExcluirHorario(hora);
                    return View("SucessoAluguel");
                }
            }
            return RedirectToAction("HorarioExistente");
        }

        public ActionResult ListarAlugueis(int id)
        {
            var queryCamposAlugueis = new QueryUsuarioFornecedor();
            var CamposAlugueisList = queryCamposAlugueis.ListarCamposAlugueisPorId(id);
            if(CamposAlugueisList.Count() == 0)
            {
                return View("NaoHaDados");
            }
            var campoAluguel = CamposAlugueisList.First();
            return View(CamposAlugueisList);
        }
        
        public ActionResult RemoverAluguel(int id)
        {
            
            var queryAluguel = new QueryUsuarioFornecedor();
            var aluguelList = queryAluguel.ListarAluguelPorId(id);
            var aluguelTD = new Aluguel {
                idAluguel = aluguelList.First().idAluguel,
                idCampo = aluguelList.First().idCampo,
                horarioInicio = aluguelList.First().horarioInicio,
                horarioFim = aluguelList.First().horarioFim,
                valor = aluguelList.First().valor
            };
            TempData["Aluguel"] = aluguelTD;

            return View(aluguelTD);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoverAluguel(Aluguel rent)
        {
            var aluguel = TempData["aluguel"] as Aluguel;
            TempData.Keep();
            
            if (ModelState.IsValid)
            {
                var queryAluguel = new QueryUsuarioFornecedor();
                var hora = new Horario
                {
                    idCampo = aluguel.idCampo,
                    horarios = aluguel.horarioInicio
                };

                if (queryAluguel.VerificaHorario(hora))
                {
                    queryAluguel.RemoverAluguel(aluguel);
                    queryAluguel.AdicionarHorario(hora);
                    return View("SucessoAluguel");
                }
            }
            return View();
        }

        //public ActionResult RemoverAluguel(int id)
        //{
        //    var queryAluguel = new QueryUsuarioFornecedor();
        //    var aluguelList = queryAluguel.SelecionaCampo(id);
        //    Aluguel aluguel = new Aluguel();
        //    aluguel.idCampo = id;
        //    return View(aluguel);
        //}

        //[HttpPost, ActionName("RemoverAluguel")]
        //[ValidateAntiForgeryToken]
        //public ActionResult RemoveAluguel(int id)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var queryAluguel = new QueryUsuarioFornecedor();
        //        queryAluguel.RemoverAluguel(id);

        //    }
        //    return RedirectToAction("HorarioExistente");
        //}

        public ActionResult ExcluirHorario(int id)
        {
            var campo = new QueryUsuarioFornecedor();
            var campoEscolhido = campo.SelecionaCampo(id);
            Horario hora = new Horario();
            hora.idCampo = campoEscolhido.id;
            return View(hora);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ExcluirHorario(Horario hora)
        {
            if (ModelState.IsValid)
            {
                var horario = new QueryUsuarioFornecedor();
                if (!horario.VerificaHorario(hora))
                {
                    horario.ExcluirHorario(hora);
                    return RedirectToAction("HorarioExcluido");
                }
            }
            return RedirectToAction("HorarioNaoExistente");
        }

        public ActionResult Excluir(int id)
        {
            var field = new QueryUsuarioFornecedor();
            var campoSelecionado = field.SelecionaCampo(id);

            return View(campoSelecionado);
        }

        [HttpPost, ActionName("Excluir")]
        [ValidateAntiForgeryToken]
        public ActionResult ExcluirConfirmado(int id)
        {
            var field = new QueryUsuarioFornecedor();
            field.ExcluirCampo(id);

            return RedirectToAction("CampoExcluido");
        }

        public ActionResult VerPagamentos(int id)
        {
            var pgto = new QueryUsuarioFornecedor();
            var pgtoList = pgto.ListarPagamentos(id);
            //var logged = new UsuarioConsumidor { Id = id };
            //TempData["UsuarioConsumidor"] = logged;
            foreach (var item in pgtoList)
            {
                item.responsavelId = id;
            }
            if (pgtoList.Count() == 0)
            {
                return View("NaoHaDados");
            }
            return View(pgtoList);
        }
        
        public ActionResult AdicionarPlanoMensal(int id) {
            var queryPlanoMensal = new QueryUsuarioFornecedor();
            var campoEscolhido = queryPlanoMensal.SelecionaCampo(id);
            PlanoMensal planoMensal = new PlanoMensal();
            planoMensal.campoId = campoEscolhido.id;
            return View(planoMensal);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdicionarPlanoMensal(PlanoMensal planoMensal)
        {
            if (ModelState.IsValid)
            {
                var queryPlanoMensal = new QueryUsuarioFornecedor();
                var hora = new Horario
                {
                    idCampo = planoMensal.campoId,
                    horarios = planoMensal.horarioInicio
                };

                if (!queryPlanoMensal.VerificaHorario(hora))
                {
                    queryPlanoMensal.CriarPlanoMensal(planoMensal);
                    queryPlanoMensal.ExcluirHorario(hora);
                    return View("SucessoPlanoMensal");
                }
            }
            return RedirectToAction("HorarioExistente");
        }


        public ActionResult CampoExcluido() {
            return View();
        }
   
        public ActionResult CampoAdicionado(){
            return View();
        }

        public ActionResult HorarioExistente()
        {
            return View();
        }

        public ActionResult HorarioAdicionado()
        {
            return View();
        }

        public ActionResult HorarioExcluido()
        {
            return View();
        }

        public ActionResult HorarioNaoExistente()
        {
            return View();
        }

        public ActionResult Alert()
        {
            return View();
        }


    }


}