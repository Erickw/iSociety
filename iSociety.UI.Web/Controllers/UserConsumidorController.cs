using iSociety.Models;
using System.Web.Mvc;


namespace iSociety.UI.Web.Controllers
{
    public class UserConsumidorController : Controller
    {
        // GET: Aluno
        public ActionResult Index()
        {

            var users = new QueryUsuarioConsumidor();
            var usersList = users.ListarTodos();
            return View(usersList);
        }

        public ActionResult Cadastrar()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastrar(UsuarioConsumidor user)
        {
            if (ModelState.IsValid)
            {
                var usuario = new QueryUsuarioConsumidor();
                usuario.Inserir(user);
                return RedirectToAction("Index");
            }
            return View(user);
        }

        public ActionResult Editar(int id)
        {
            var users = new QueryUsuarioConsumidor();
            var usersList = users.ListarPorId(id);
            var usuarioSelecionado = usersList[0];
            if(usersList == null) {
                return HttpNotFound();
            }
            return View(usuarioSelecionado);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(UsuarioConsumidor user)
        {
            if (ModelState.IsValid)
            {
                var usuario = new QueryUsuarioConsumidor();
                usuario.Alterar(user);
                return RedirectToAction("Index");
            }
            return View(user);
        }

        public ActionResult Detalhes(int id) {
            var users = new QueryUsuarioConsumidor();
            var usersList = users.ListarPorId(id);
            var usuarioSelecionado = usersList[0];

            return View(usuarioSelecionado);
        }

        public ActionResult Excluir(int id)
        {
            var users = new QueryUsuarioConsumidor();
            var usersList = users.ListarPorId(id);
            var usuarioSelecionado = usersList[0];

            return View(usuarioSelecionado);
        }

        [HttpPost, ActionName("Excluir")]
        [ValidateAntiForgeryToken]
        public ActionResult ExcluirConfirmado(int id)
        {
            var users = new QueryUsuarioConsumidor();
            users.Excluir(id);

            return RedirectToAction("Index");
        }
    }
}