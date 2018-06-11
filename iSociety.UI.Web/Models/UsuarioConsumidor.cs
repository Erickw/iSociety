using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace iSociety.Models

{
    public class UsuarioConsumidor
    {
        
        public int Id { get; set; }

        [Required(ErrorMessage = "Nome de usuario ser preenchido")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O email deve ser preenchido")]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "O email não e valido")]
        public string email { get; set; }

        [Required(ErrorMessage = "A senha deve ser preenchida")]
        public string Senha { get; set; }
        public string contaBanco { get; set; }
    }
}
