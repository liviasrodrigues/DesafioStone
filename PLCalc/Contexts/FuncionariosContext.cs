using Microsoft.EntityFrameworkCore;
using PLCalc.Models;

namespace PLCalc.Contexts
{
    public class FuncionarioContext : DbContext
    {
       
        public FuncionarioContext(DbContextOptions<FuncionarioContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Funcionarios> Funcionarios { get; set; }
    }
}
