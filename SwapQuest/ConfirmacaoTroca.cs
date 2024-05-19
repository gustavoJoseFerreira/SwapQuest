using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapQuest
{
    public class ConfirmacaoTroca
    {
        [Key]
        public int Id_ConfTroca { get; set; }
        public int QTD_Trocada { get; set; }
        public string ContactoComprador { get; set; }
        public string OpcaoTroca { get; set; }
        public int Id_Comprador { get; set; }
        public int Id_Vendedor { get; set; }
        public int Id_CardTrocado { get; set; }
        public int Id_Troca { get; set; }
    }
}
