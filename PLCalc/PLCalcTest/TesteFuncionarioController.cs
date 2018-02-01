using Microsoft.EntityFrameworkCore;
using PLCalc.Contexts;
using PLCalc.Controllers;
using PLCalc.Models;
using Xunit;


namespace PLCalc.Test
{
    public class TesteFuncionarioController
    {

        private FuncionarioContext _context;


        public TesteFuncionarioController()

        {
             
            DbContextOptionsBuilder<FuncionarioContext> OptionBuilder = new DbContextOptionsBuilder<FuncionarioContext>();
            OptionBuilder.UseNpgsql("Server=baasu.db.elephantsql.com;Port=5432;Pooling=true;Database=flfuaelt;User Id=flfuaelt;Password=Ih81sEGYZcvEjsFlsqHfqmmD8INQhGzD;");
            _context = new FuncionarioContext(OptionBuilder.Options);
        }
 

        [Fact]
        public void GetAll_retornaTodosFuncionarios()
        {
            FuncionarioController controller = new FuncionarioController(_context);
            var retorno = controller.GetAll();

          
            
        }
    }
}
