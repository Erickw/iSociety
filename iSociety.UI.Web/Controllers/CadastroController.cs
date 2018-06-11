using iSociety.Models;
using System.Web.Mvc;
using iSociety.Repositorio;

namespace iSociety.UI.Web.Controllers
{
    public class CadastroController : Controller
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
            if (ModelState.IsValid)
            {
                var cripto = new Cripto();
                var usuario = new QueryUsuarioConsumidor();
                var senhaHash = cripto.Hash(user.Senha);
                user.Senha = senhaHash;
                usuario.Inserir(user);
                return RedirectToAction("Index");
                //return RedirectToRoute(new{ controller = "Home", action = "About", id = UrlParameter.Optional });
            }
            return View(user);
        }
    }
}