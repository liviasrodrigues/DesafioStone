using PLCalc.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace PLCalc
{
    public static class Utils
    {
        //Retorna Participações
        public static Participacoes calculaPL (List<Funcionarios> p_funcionarios, Decimal p_saldo, String p_matricula = null)
        {
            Participacoes participacoesDeLucro = new Participacoes();
            int totalFuncionarios = 0;
            Decimal totalDistribuido = 0;
            Decimal totalDiponibilizado = 0;

           totalFuncionarios = p_funcionarios.Count();
           totalDiponibilizado = p_saldo/totalFuncionarios;

            //Se a matricula for passada deixo na List apenas o funcionario correspondente;
            if (p_matricula != null)            
                p_funcionarios = p_funcionarios.ToList().FindAll(x => x.matricula == p_matricula);
            
          
            foreach (var funcionario in p_funcionarios)
            {
                Participante participante = new Participante();
               
                Decimal SB = Convert.ToDecimal(funcionario.salario_bruto.Replace("R$", "")); //Sarario Bruto
                Decimal porcentagem = 0;
                Decimal participacao = 0;
             
                //Pego valores do peso de cada critério
                int PTA = PesoTempoAdmissao(Convert.ToDateTime(funcionario.data_de_admissao)); //Peso por tempo de Admissão
                int PAA = PesoAreaAtuacao(funcionario.area); //Peso por area de Atuação
                int PFS = PesoFaixaSalarial(SB); //Peso por faixa salarial
              
                porcentagem = (((SB * PTA) + (SB * PAA)) / (SB * PFS)) * 12;
                participacao = (totalDiponibilizado * porcentagem) / 100;

                participante.nome = funcionario.nome;
                participante.matricula = funcionario.matricula;
                participante.valor_participacao = participacao.ToString();

                totalDistribuido =+ participacao;

                participacoesDeLucro.participacoes.Add(participante);

            }

            participacoesDeLucro.total_de_distribuido = totalDistribuido.ToString();
            participacoesDeLucro.total_de_funcionarios = totalFuncionarios;
            participacoesDeLucro.total_disponibilizado = totalDiponibilizado.ToString();
            participacoesDeLucro.saldo_total_disponibilizado = (totalDiponibilizado - totalDistribuido).ToString();


            return participacoesDeLucro;
        }
       

        private static int PesoFaixaSalarial(Decimal p_salaio)
        {
            int peso = 0;
            Decimal salarioMinimo = 937;
            Decimal quantidadeSalarios = p_salaio / salarioMinimo;

            if (quantidadeSalarios <= 3)
                peso = 1;
            if (quantidadeSalarios > 3 && quantidadeSalarios <= 5)
                peso = 2;
            if (quantidadeSalarios > 5 && quantidadeSalarios <= 8)
                peso = 3;
            if (quantidadeSalarios > 8)
                peso = 5;

            return peso;
        }

        private static int PesoAreaAtuacao(string p_area)
        {
            int peso = 0;
            switch (p_area)
            {
                case "Diretoria":
                    peso = 1;
                    break;
                case "Contabilidade":
                    peso = 2;
                    break;
                case "Financeiro":
                    peso = 2;
                    break;
                case "Tecnologia":
                    peso = 2;
                    break;
                case "Serviços Gerais":
                    peso = 3;
                    break;
                case "Relacionamento com o Cliente":
                    peso = 5;
                    break;
                default:
                    peso = 0;
                    break;

            }

            return peso;
        }
        private static int PesoTempoAdmissao(DateTime p_date)
        {           
            TimeSpan date = DateTime.Now.Subtract(p_date);
            
            double tempoAdmissao = date.Days;
            tempoAdmissao = tempoAdmissao / 365;
            int peso = 0;

            if (tempoAdmissao < 1)
                peso = 1;
            if (tempoAdmissao >= 1 && tempoAdmissao < 3)
                peso = 2;
            if (tempoAdmissao >= 3 && tempoAdmissao < 8)
                peso = 3;
            if (tempoAdmissao > 8)
                peso = 5;

            return peso;           
        }
    }
}
