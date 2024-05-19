using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapQuest
{
    public class Troca
    {
        [Key]
        public int Id_Troca { get; set; }
        public int QTD_Troca { get; set; }
        public string OpcaoTroca { get; set; }
        public int Id_User { get; set; }
        public int Id_Card { get; set; }
    }
}
