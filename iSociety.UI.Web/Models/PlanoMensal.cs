using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iSociety.UI.Web.Models
{
    public class PlanoMensal
    {
        public int idPlanoMensal { get; set; }
        public int campoId { get; set; }
        public int reponsavelId { get; set; }
        public string horarioInicio{ get; set; }
        public string horarioFim { get; set; }
        public string diaSemana { get; set; }
    }
}