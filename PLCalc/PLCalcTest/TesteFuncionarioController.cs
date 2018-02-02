using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CalculadoraPL.Contexts;
using CalculadoraPL.Models;
using CalculadoraPL.Controllers;
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;

namespace CalculadoraPL.Test
{
    public class TesteFuncionarioController
    {

        private FuncionarioContext _context;
        private DbContextOptionsBuilder<FuncionarioContext> OptionBuilder;

        public TesteFuncionarioController()
        {            
            OptionBuilder = new DbContextOptionsBuilder<FuncionarioContext>();
            OptionBuilder.UseNpgsql("Server=baasu.db.elephantsql.com;Port=5432;Pooling=true;Database=flfuaelt;User Id=flfuaelt;Password=Ih81sEGYZcvEjsFlsqHfqmmD8INQhGzD;");

            _context = new FuncionarioContext(OptionBuilder.Options);
       
        }
      
        [Fact]
        public void  TestGetAllFuncionarios()
        {
            //Arranjo          
            var controller = new FuncionarioController(_context);
            var totalFuncionarios = _context.Funcionarios.Count();

            //Ação
            var result = controller.GetAllFuncionarios();

            //Assert          
            var okResult = result.Should().BeOfType<List<Funcionarios>>().Subject;
            result.Should().HaveCount(totalFuncionarios); 
        }

        [Fact]
        public void TestGetByMatricula()
        {
            // Arranjo
            var controller = new FuncionarioController(_context);
            var matricula = _context.Funcionarios.FirstOrDefault().matricula;

            // Ação
            var result = controller.GetFuncionariosByMatricula(matricula);

            // Assert
            var okResult = result.Should().BeOfType<ObjectResult>().Subject;
            var funcionario = okResult.Value.Should().BeAssignableTo<Funcionarios>().Subject;
            funcionario.matricula.Should().Be(matricula);
        }

        [Fact]
        public void TestCreateFuncionario()
        {
            // Arranjo
            var serviceMock = new Mock<FuncionarioContext>(OptionBuilder.Options);
            serviceMock.Setup(x => x.Funcionarios).Returns(_context.Funcionarios);
        
            var controller = new FuncionarioController(serviceMock.Object);

            var funcionario = new Funcionarios
            {
                nome = "Funcionario Teste 4",
                matricula = "0001238",
                area = "Relacionamento com o Cliente",
                cargo = "Auxiliar de Ouvidoria",
                salario_bruto = "R$ 1.500,00",
                data_de_admissao = "2016-03-05"
            };

            //Ação
            var result = controller.CreateFuncionarios(funcionario);

            // Assert
            var okResult = result.Should().BeOfType<CreatedAtRouteResult>().Subject;
            var funcionarioResult = okResult.Value.Should().BeAssignableTo<Funcionarios>().Subject;
            
        }

        [Fact]
        public void TestCreateFuncionarioList()
        {
            // Arranjo
            var serviceMock = new Mock<FuncionarioContext>(OptionBuilder.Options);
            serviceMock.Setup(x => x.Funcionarios).Returns(_context.Funcionarios);

            var controller = new FuncionarioController(serviceMock.Object);

            var funcionariosList = new List<Funcionarios>
            {
               new Funcionarios { nome = "Funcionario Teste 1",
                matricula = "0001238",
                area = "Relacionamento com o Cliente",
                cargo = "Auxiliar de Ouvidoria",
                salario_bruto = "R$ 1.500,00",
                data_de_admissao = "2016-03-05" },

                 new Funcionarios { nome = "Funcionario Teste 2",
                matricula = "0001235",
                area = "Diretoria",
                cargo = "Diretor Financeiro",
                salario_bruto = "R$ 15.000,00",
                data_de_admissao = "2010-05-02" }

            };

            //Ação
            var result = controller.CreateFuncionarios(funcionariosList);

            // Assert
            var okResult = result.Should().BeOfType<ObjectResult>().Subject;          
        }

        [Fact]
        public void TestDeleteFuncionario()
        {          
            // Arranjo
            var serviceMock = new Mock<FuncionarioContext>(OptionBuilder.Options);
            serviceMock.Setup(x => x.Funcionarios).Returns(_context.Funcionarios);

            var controller = new FuncionarioController(serviceMock.Object);
            var matricula = serviceMock.Object.Funcionarios.FirstOrDefault().matricula;

            // Ação
            var result = controller.DeleteFuncionarios(matricula);

            // Assert
            var okResult = result.Should().BeOfType<NoContentResult>().Subject;

        }

        [Fact]
        public void TestUpdateFuncionario()
        {
            // Arranjo
            var serviceMock = new Mock<FuncionarioContext>(OptionBuilder.Options);
            serviceMock.Setup(x => x.Funcionarios).Returns(_context.Funcionarios);

            var controller = new FuncionarioController(serviceMock.Object);
            var matricula = serviceMock.Object.Funcionarios.FirstOrDefault().matricula; ;
            var funcionario = serviceMock.Object.Funcionarios.FirstOrDefault();
            funcionario.nome = "Nome Atualizado";
            // Ação
            var result = controller.UpdateFuncionarios(matricula,funcionario);

            // Assert
            var okResult = result.Should().BeOfType<ObjectResult>().Subject;
            var funcionarioResult = okResult.Value.Should().BeAssignableTo<Funcionarios>().Subject;
            funcionarioResult.nome.Should().Be(funcionario.nome);
        }

        [Fact]
        public void TestGetAllParticipacoes()
        {
            //Arranjo          
            var controller = new FuncionarioController(_context);

            //Ação
            var result = controller.GetAllParticipacoes(70000);

            //Assert          
            var okResult = result.Should().BeOfType<ObjectResult>().Subject;
            var participacaoResult = okResult.Value.Should().BeAssignableTo<Participacoes>().Subject;
            participacaoResult.participacoes.Should().NotBeNull();
        }

        [Fact]
        public void TestGetParticipacaoDeUmFuncionario()
        {
            //Arranjo          
            var controller = new FuncionarioController(_context);
            var matricula = _context.Funcionarios.FirstOrDefault().matricula;

            //Ação
            var result = controller.GetParticipacaoByFuncionario(matricula,70000);

            //Assert          
            var okResult = result.Should().BeOfType<ObjectResult>().Subject;
            var participacaoResult = okResult.Value.Should().BeAssignableTo<Participacoes>().Subject;
            participacaoResult.participacoes.First().matricula.Should().Be(matricula);
            participacaoResult.participacoes.Count().Should().Be(1);
        }

        [Fact]
        public void ParticipacaoIndividualResultadoOk()
        {
            Funcionarios funcionario = new Funcionarios()
            {
                matricula = "000000",
                nome = "Funcionario Teste",
                area = "Financeiro",
                cargo = "Contador Pleno",
                salario_bruto = "R$ 3.199,82",
                data_de_admissao = "2016-08-17"
            };

            Decimal SB = Convert.ToDecimal(funcionario.salario_bruto.Replace("R$", ""));
            Decimal disponivel = 100000 / 22;
            String dataAtual = "2018-01-02";
            var result = Utils.ParticipacaoIndividual(funcionario, SB, disponivel, Convert.ToDateTime(dataAtual));

            Assert.True(result.ToString() == "1090,8");
        }


    }
}
