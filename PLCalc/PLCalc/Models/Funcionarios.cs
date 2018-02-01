using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PLCalc.Models
{
    [Table("Funcionario")]
    public class Funcionarios
    {
        [Key]
        public string matricula { get; set; }

        public string nome { get; set; }
        public string area { get; set; }
        public string cargo { get; set; }
        public string salario_bruto { get; set; }
        public string data_de_admissao { get; set; }
      
    }
}
