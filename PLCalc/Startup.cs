using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using PLCalc.Contexts;
using Swashbuckle.AspNetCore.Swagger;

namespace PLCalc
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
           
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("help", new Info { Title = "CalcPL Help", Version = "v1" });
            });

            //Banco de dados  injetado no container do serviço   
            services.AddEntityFrameworkNpgsql()
           .AddDbContext<FuncionarioContext>(options => options.UseNpgsql(Configuration.GetConnectionString("DesafioStone")));

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options => { options.SwaggerEndpoint("/swagger/help/swagger.json", "CalcPL"); }); 

            app.UseMvc();
            app.UseDeveloperExceptionPage();
        }
    }
}
