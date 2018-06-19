using iSociety.Models;
using System.Web.Mvc;
using iSociety.Repositorio;
using CryptSharp;

namespace iSociety.UI.Web.Areas.Admin.Controllers
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
                string senhaHash = Crypter.Blowfish.Crypt(user.Senha);
                var usuario = new QueryUsuarioConsumidor();
                if (usuario.VerificaUserName(user))
                {
                    user.Senha = senhaHash;
                    usuario.Inserir(user);
                }
                return View("UsuarioExistente");
            }
            return View(user);
        }
    }
}