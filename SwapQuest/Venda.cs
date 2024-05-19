using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapQuest
{
    public class Venda
    {
        [Key]
        public int Id_Venda{ get; set; }
        public decimal ValorVenda { get; set; }
        public int QTD_Venda { get; set; }
        public int Id_User { get; set; }
        public int Id_Card { get; set; }
    }
}
