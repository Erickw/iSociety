using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace iSociety.UI.Web.Models
{
    public class Aluguel
    {
        public int idAluguel { get; set; }
        public int idCampo { get; set; }
        public int reponsavelId { get; set; }
        public int idPagamento { get; set; }
        [Required(ErrorMessage = "Horário deve ser preenchido")]
        public string horarioInicio{ get; set; }
        public string  horarioFim { get; set; }
        public bool confirmado { get; set; }
        public float valor { get; set; }
    }
}


