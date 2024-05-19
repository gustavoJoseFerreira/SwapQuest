using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapQuest
{
    public class AdicionarCartasViewModel
    {
        public int Id_Carta { get; set; }
        public int NumeroCarta { get; set; }
        public string PersonagemCarta { get; set; }
        public string NomeSerie { get; set; }
        public int AnoLancamento { get; set; }
        public decimal PrecoRef { get; set; }
        public string Imagem { get; set; }
    }
}
