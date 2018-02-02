using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using CalculadoraPL.Contexts;

namespace CalculadoraPL
{
    public class Startup
    {

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
      
        }

        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            //Banco de dados  injetado no container do serviço   
            services.AddMvc();

            services.AddEntityFrameworkNpgsql()
           .AddDbContext<FuncionarioContext>(options => options.UseNpgsql(Configuration.GetConnectionString("DesafioStone")));
          
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMvc();
        }
    }
}
