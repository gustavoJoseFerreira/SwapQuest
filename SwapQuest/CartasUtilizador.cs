using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapQuest
{
    public class CartasUtilizador
    {
        [Key]
        public int Id_Carta { get; set; }
        public int Quantidade { get; set; }
        public string Qualidade { get; set; }
        public string Idioma { get; set; }
        public int Id_Utilizador { get; set; }
        public int Id_Card { get; set; }
        
    }
}
