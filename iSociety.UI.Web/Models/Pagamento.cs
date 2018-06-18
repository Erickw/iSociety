using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace iSociety.UI.Web.Models
{
    public class Pagamento
    {

        public int idPagamento { get; set; }
        public int idConsumidor { get; set; }
        public int idAdministrador { get; set; }
        public string horarioReservado { get; set; }        
        public string formaPagamento { get; set; }

    }
}


