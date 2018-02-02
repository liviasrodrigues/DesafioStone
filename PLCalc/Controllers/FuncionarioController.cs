using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using PLCalc.Models;
using System.Linq;
using PLCalc.Contexts;
using System;

namespace PLCalc.Controllers
{
    [Route("plcalc/funcionarios")]
    public class FuncionarioController : Controller
    {
        private readonly FuncionarioContext _context;
      
        public FuncionarioController(FuncionarioContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retorna todos os funcionarios cadastrados
        /// </summary>
        /// <response code="400">Bad Request</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet]
        public List<Funcionarios> GetAllFuncionarios()
        {
            return _context.Funcionarios.ToList();
        }

        /// <summary>
        /// Retorna um usuario pela matricula
        /// </summary>
        /// <param name="matricula">Matricula do funcionário</param>
        /// <response code="400">Bad Request</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet("{matricula}", Name = "GetByMatricula")]
        public IActionResult GetFuncionariosByMatricula(string matricula)
        {
            var item = _context.Funcionarios.FirstOrDefault(t => t.matricula == matricula);
            if (item == null)
            {
                return NotFound();
            }

            return new ObjectResult(item);
        }

        /// <summary>
        /// Insere novo funcionário
        /// </summary>
        /// <param name="Funcionario">Novo Funcionario</param>
        /// <response code="400">Bad Request</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost]
        public IActionResult CreateFuncionarios([FromBody] List<Funcionarios> item)
        {
            if (item == null)
            {
                return BadRequest();
            }

            foreach(var c_item in item)
            {
                _context.Funcionarios.Add(c_item);
                _context.SaveChanges();

            }

            return new ObjectResult(_context.Funcionarios);
        }

        /// <summary>
        /// Insere lista de novos funcionarios
        /// </summary>
        /// <param name="Funcionarios">Novos fucionarios</param>
        /// <response code="400">Bad Request</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost("Lista")]
        public IActionResult CreateFuncionarios([FromBody] Funcionarios item)
        {
            if (item == null)
            {
                return BadRequest();
            }

            _context.Funcionarios.Add(item);
            _context.SaveChanges();

            return CreatedAtRoute("GetFuncionarios", new { matricula = item.matricula }, item);
        }

        /// <summary>
        /// Atualiza funcionario
        /// </summary>
        /// <param name="matricula">matricula</param>
        /// <param name="funcionario">Dados atualizados do fucionario</param>
        /// <response code="400">Bad Request</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPut("{matricula}")]
        public IActionResult UpdateFuncionarios(string matricula, [FromBody] Funcionarios item)
        {
            if (string.IsNullOrEmpty(matricula))
            {
                return BadRequest();
            }

            var funcionario = _context.Funcionarios.FirstOrDefault(x => x.matricula == matricula);
            if (funcionario == null)
            {
                return NotFound();
            }

            funcionario.nome = item.nome;
            funcionario.area = item.area;
            funcionario.cargo = item.cargo;
            funcionario.salario_bruto = item.salario_bruto;
            funcionario.data_de_admissao = item.data_de_admissao;

            _context.Funcionarios.Update(funcionario);
            _context.SaveChanges();


            return new ObjectResult(_context.Funcionarios.Where(t=>t.matricula == matricula).FirstOrDefault());
        }

        /// <summary>
        /// Deleta funcionario
        /// </summary>
        /// <param name="matricula">matricula</param>      
        /// <response code="400">Bad Request</response>
        /// <response code="500">Internal Server Error</response>
        [HttpDelete("{matricula}")]
        public IActionResult DeleteFuncionarios(string matricula)
        {
            if (string.IsNullOrEmpty(matricula))
            {
                return BadRequest();
            }

            var funcionario = _context.Funcionarios.FirstOrDefault(x => x.matricula == matricula);
            if (funcionario == null)
            {
                return NotFound();
            }
         
            _context.Funcionarios.Remove(funcionario);
            _context.SaveChanges();


            return new NoContentResult();
        }

        /// <summary>
        /// Retorna o valor de participação nos lucros para todos os funcionarios cadastrados
        /// </summary>
        /// <param name="saldo">Saldo Disponivel</param>
        /// <response code="400">Bad Request</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet("participacoes/{saldo}")]
        public IActionResult GetAllParticipacoes(Decimal saldo)
        {
            if (saldo == 0)
            {
                return BadRequest();
            }

            Participacoes result = Utils.CalculaParticipacao(_context.Funcionarios.ToList(), saldo, DateTime.Now);
            return new ObjectResult(result);
        }


        /// <summary>
        /// Retorna o valor de participação nos lucros para um unico funcionario selecionado
        /// </summary>
        /// <param name="matricula">Saldo Disponivel</param>
        /// <param name="saldo">Saldo Disponivel</param>
        /// <response code="400">Bad Request</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet("{matricula}/participacoes/{saldo}")]
        public IActionResult GetParticipacaoByFuncionario(string matricula, decimal saldo)
        {
            if(saldo == 0)
            {
                return BadRequest();
            }

            Participacoes result = Utils.CalculaParticipacao(_context.Funcionarios.ToList(), saldo, DateTime.Now);
            result.participacoes = result.participacoes.FindAll(t => t.matricula == matricula);

            if (result == null)
            {
                return NotFound();
            }

            return new ObjectResult(result);
        }

    }
}