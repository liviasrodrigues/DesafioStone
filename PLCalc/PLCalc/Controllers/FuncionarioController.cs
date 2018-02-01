using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using PLCalc.Models;
using System.Linq;
using System.IO;
using System.Text;
using System.Runtime.Serialization.Json;
using PLCalc.Contexts;

namespace PLCalc.Controllers
{
    [Route("PLCalc/funcionarios")]
    public class FuncionarioController : Controller
    {
        private readonly FuncionarioContext _context;

        //usando  Injeção de Dependência para injetar o contexto de banco de dados no controlador
        public FuncionarioController(FuncionarioContext context)
        {
            _context = context;

            //Primeira carga de dados na tabela Funcionarios
            if (_context.Funcionarios.Count() == 0)
            {
                var json = Utils.DeserializarDataContractJsonSerializer();
                foreach (var item in json)
                    _context.Funcionarios.Add(item);
                _context.SaveChanges();
            }
        }

        [HttpGet]
        public IEnumerable<Funcionarios> GetAllFuncionarios()
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

        //Retorna valor da participação para todos os funcionários
        [HttpGet("participacoes/{saldo}")]
        public string GetAllParticipacoes(float saldo)
        {
            return "participações";
        }


        //Retorna valor da participação para todos um funcionário selecionado
        [HttpGet("{matricula}/participacoes/{saldo}")]
        public string GetParticipacaoByFuncionario(string matricula, float saldo)
        {
            return "participaçoes por funcionario";
        }

    }
}