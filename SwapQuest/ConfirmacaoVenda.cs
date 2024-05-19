using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapQuest
{
    public class ConfirmacaoVenda
    {
        [Key]
        public int Id_ConfVenda { get; set; }
        public int QTD_Comprada { get; set; }
        public int ContactoComprador { get; set; }
        public decimal ValorVenda { get; set; }
        public int Id_Comprador { get; set; }
        public int Id_Vendedor { get; set; }
        public int Id_CardVendida { get; set; }
        public int Id_Sale { get; set; }
    }
}
