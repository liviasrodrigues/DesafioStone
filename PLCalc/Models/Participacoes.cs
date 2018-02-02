using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PLCalc.Models
{
    public class Participacoes
    {
        public List<Participante> participacoes { get; set; }
        public int total_de_funcionarios { get; set; }
        public string total_de_distribuido { get; set; }
        public string total_disponibilizado { get; set; }
        public string saldo_total_disponibilizado { get; set; }
        
    }
}
