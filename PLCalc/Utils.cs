using PLCalc.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace PLCalc
{
    public static class Utils
    {
        public static Participacoes CalculaParticipacao (List<Funcionarios> p_funcionarios, Decimal p_saldo, DateTime p_dataAtual)
        {
            if(p_funcionarios.Count == 0)
                throw new System.Exception("CalculaParticipacao: Não existem funcionarios cadastrados");

            Participacoes participacoesDeLucro = new Participacoes();
            participacoesDeLucro.participacoes = new List<Participante>();
            int totalFuncionarios = p_funcionarios.Count();
            Decimal totalDiponibilizado = p_saldo / totalFuncionarios;
            Decimal totalDistribuido = 0;
            
            //Varre lista de  fucionários
            foreach (var funcionario in p_funcionarios)
            {
                try
                {
                    Participante participante = new Participante();
                    Decimal participacao = 0;
                    Decimal SB = Convert.ToDecimal(funcionario.salario_bruto.Replace("R$", ""));

                    //retorna participação para o funcionario corrente
                    participacao = ParticipacaoIndividual(funcionario, SB, totalDiponibilizado, p_dataAtual);

                    //Insere dados de participante
                    participante.nome = funcionario.nome;
                    participante.matricula = funcionario.matricula;
                    participante.valor_participacao = FormataMoeda(participacao);
                    participacoesDeLucro.participacoes.Add(participante);

                    //Incrementa total distribuido
                    totalDistribuido = totalDistribuido + participacao;

                }catch(Exception error)
                {
                  throw new System.Exception("CalculaParticipacao: Ocorreu um erro ao calcular a participação do funcionário de matricula "+funcionario.matricula+" "+error.Message);
                }
            }

            //Insere dados formatados em participação
            participacoesDeLucro.total_de_funcionarios = totalFuncionarios;
            participacoesDeLucro.total_de_distribuido = FormataMoeda(totalDistribuido);         
            participacoesDeLucro.total_disponibilizado = FormataMoeda(p_saldo);
            participacoesDeLucro.saldo_total_disponibilizado = FormataMoeda(p_saldo - totalDistribuido);

            return participacoesDeLucro;
        }

        public static Decimal ParticipacaoIndividual(Funcionarios p_funcionario, Decimal p_salarioBruto, Decimal p_saldoDisponivel, DateTime p_dataAtual)
        {
            Decimal participacao;
            
            //Retorna pesos de cada critério
            int PTA = PesoTempoAdmissao(Convert.ToDateTime(p_funcionario.data_de_admissao), p_dataAtual);
            int PAA = PesoAreaAtuacao(p_funcionario.area);
            int PFS = PesoFaixaSalarial(p_salarioBruto);

            try
            {
                //Calcula de participação
                participacao = (((p_salarioBruto * PTA) + (p_salarioBruto * PAA)) / (p_salarioBruto * PFS)) * 12;
                participacao = (p_saldoDisponivel * participacao) / 100;
            }
            catch(Exception error)
            {
                throw new System.Exception("ParticipacaoIndividual: Ocorreu um erro ao calcular a participação do funcionário de matricula " + p_funcionario.matricula + " " + error.Message);

            }

            return participacao;
        }

        private static int PesoFaixaSalarial(Decimal p_salaio)
        {
            int peso = 1;
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

        private static int PesoTempoAdmissao(DateTime p_date, DateTime p_DataAtual)
        {           
            TimeSpan date = p_DataAtual.Subtract(p_date);
            
            double tempoAdmissao = date.Days;
            tempoAdmissao = tempoAdmissao / 365;
            int peso = 1;

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

        private static int PesoAreaAtuacao(string p_area)
        {
            int peso = 1;
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
                    peso = 1;
                    break;
            }

            return peso;
        }

        private static string FormataMoeda(Decimal valor)
        {
            string real = "";
            try
            {
                real = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "R$ {0:#,###.##}", valor);
            }
            catch
            {
                throw new System.Exception("FormataMoeda: Ocorreu um erro ao formatar valor: " + valor);
            }

            return real;
            
        }

    }
}
