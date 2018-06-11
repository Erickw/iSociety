using iSociety.Models;
using System.Web.Mvc;

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
                var usuario = new QueryUsuarioConsumidor();
                usuario.Inserir(user);
                return RedirectToAction("Index");
            }
            return View(user);
        }
    }
}