using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iSociety.UI.Web.Models
{
    public class CampoAluguel
    {
        public int aluguelId { get; set; }
        public int responsavelId { get; set; }
        public string nomeCampo { get; set; }       
        public string rua { get; set; }
        public string horarioInicio { get; set; }
        public string horarioFim{ get; set; }
        public double valor { get; set; }
        public string cep { get; set; }
        public int numero { get; set; }
        public string cidade { get; set; }
        public bool bar { get; set; }
    }
}