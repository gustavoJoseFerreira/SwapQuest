using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapQuest
{
    public class ClassColecao
    {
        [Key]
        public int Id_Colecao { get; set; }
        public string DescricaoColecao { get; set; }
    }
}
