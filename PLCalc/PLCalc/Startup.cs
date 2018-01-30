using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using PLCalc.Models;

namespace PLCalc
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            //Banco de dados em memória injetado no container do serviço
            services.AddDbContext<FuncionarioContext>(opt => opt.UseInMemoryDatabase("ListaFuncionario"));
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMvc();
        }
    }
}
