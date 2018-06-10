using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iSociety.Dominio
{
    public class UsuarioConsumidor
    {
        public int Id { get; set; }
        public string nome { get; set; }
        public string email { get; set; }
        public string senha { get; set; }
        public string contaBanco { get; set; }
    }
}
