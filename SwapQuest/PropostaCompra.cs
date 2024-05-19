using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapQuest
{
    public class PropostaCompra
    {
        [Key]
        public int Id_PrCompra { get; set; }
        public string ContactoComprador { get; set; }
        public int Id_User { get; set; }
        public int Id_Sale { get; set; }
        public int Id_Comprador { get; set; }
    }
}
