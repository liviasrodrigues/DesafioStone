using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using PLCalc.Models;
using System.Linq;
using System.IO;
using System.Text;
using System.Runtime.Serialization.Json;

namespace PLCalc.Controllers
{
    [Route("api/funcionario")]
    public class FuncionarioController : Controller
    {
        private readonly FuncionarioContext _context;

        private static List<Funcionario> DeserializarDataContractJsonSerializer()
        {
            
                string json = System.IO.File.ReadAllText("funcionarios.json");
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(List<Funcionario>));
                MemoryStream ms = new MemoryStream(Encoding.GetEncoding("iso-8859-1").GetBytes(json));
                return (List<Funcionario>)serializer.ReadObject(ms);

        }

        //usando  Injeção de Dependência para injetar o contexto de banco de dados no controlador
        public FuncionarioController(FuncionarioContext context)
        {
            _context = context;

            //Insere dados do arquivo json em memória
            if (_context.Funcionarios.Count() == 0)
            {
                var json = DeserializarDataContractJsonSerializer();
                foreach (var item in json)
                    _context.Funcionarios.Add(item);

                _context.SaveChanges();
            }
        }


        [HttpGet]
        public IEnumerable<Funcionario> GetAll()
        {
            return _context.Funcionarios.ToList();
        }

        [HttpGet("{matricula}", Name = "GetFuncionario")]
        public IActionResult GetByMatricula(string matricula)
        {
            var item = _context.Funcionarios.FirstOrDefault(t => t.matricula == matricula);
            if (item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }


        [HttpPost]
        public IActionResult Create([FromBody] List<Funcionario> item)
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
    }
}