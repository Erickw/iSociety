using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iSociety.UI.Web.Models
{
    public class CampoHorario
    {
        public int idCampo { get; set; }
        public int idAdministrador { get; set; }
        public string nomeCampo { get; set; }
        public string horarioDisponivel { get; set; }
    }
}