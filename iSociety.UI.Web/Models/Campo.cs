using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace iSociety.UI.Web.Models
{
    public class Campo
    {
        public int id { get; set; }
        public int idAdministrador { get; set; }

        [Required(ErrorMessage = "Nome do campo ser preenchido")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Nome da rua deve ser preenchida")]
        public string rua { get; set; }
        
        public string cep{ get; set; }

        [Required(ErrorMessage = "Número deve ser preenchido")]
        public int numero { get; set; }

        [Required(ErrorMessage = "Cidade deve ser preenchida")]
        public string cidade{ get; set; }
        public bool bar{ get; set; }

    }
}