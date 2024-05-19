using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapQuest
{
    public class PropostaTroca
    {
        [Key]
        public int Id_PrTroca { get; set; }
        public string ContactoComprador { get; set; }
        public int Id_Vendedor { get; set; }
        public int Id_Troca { get; set; }
        public int Id_Comprador { get; set; }
    }
}
