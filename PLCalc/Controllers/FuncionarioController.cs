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
    
        [HttpGet]
        public List<Funcionarios> GetAllFuncionarios()
        {
            return _context.Funcionarios.ToList();
        }
       
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
      
        [HttpPost]
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