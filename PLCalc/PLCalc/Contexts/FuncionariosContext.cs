using Microsoft.EntityFrameworkCore;
using CalculadoraPL.Models;

namespace CalculadoraPL.Contexts
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
