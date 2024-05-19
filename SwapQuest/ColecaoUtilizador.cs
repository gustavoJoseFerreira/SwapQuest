using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapQuest
{
    public class ColecaoUtilizador
    {
        [Key]
        public int Id_ColecaoUtilizador { get; set; }
        public int Id_Utilizador { get; set; }
        public int Id_Colecao {  get; set; }
    }
}
