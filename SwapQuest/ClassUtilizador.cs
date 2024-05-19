using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SwapQuest
{
    public class Utilizador
    {
        [Key] // Marca a propriedade como chave primária
        public int Id_User { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string UserPassword { get; set; }
        public string UserDistrito { get; set; }
        public DateTime UserDN { get; set; }
    }

}

