﻿using Microsoft.EntityFrameworkCore;

using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Pessoas_API.a_Repository;
using Pessoas_API.Context;
using System.Reflection;

namespace Pessoas_API
{
    /// <summary>
    /// 
    /// </summary>
    public class Startup
    {
        private string _connectionPostgreSQL = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            /*
             DB_HOST=localhost
                DB_PASS=123456
             */
            var host = configuration["DB_HOST"];
            var pass = configuration["DB_PASS"];
            
        }

        /// <summary>
        /// 
        /// </summary>
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            /*
             "ConnectionStrings": {
    "MySqlConnection":
             */
            
            if (Configuration != null && !string.IsNullOrEmpty(Configuration["CONNECT_POSTGRES"]))
            {
                _connectionPostgreSQL = Configuration["CONNECT_POSTGRES"];
            }
            else
            {
                _connectionPostgreSQL = Configuration["ConnectionStrings:PostgresConnection"];
            }
            _connectionPostgreSQL = Configuration["ConnectionStrings:PostgresConnection"];

            services.AddDbContext<PessoaContext>(
                context => context.UseNpgsql(_connectionPostgreSQL)
            );


            //services.AddDbContext<PessoaContext>(
            //    context => context.UseMySql(
            //        connectionString, ServerVersion.AutoDetect(connectionString)
            //    )
            //);

            //services.AddDbContext<PessoaContext>(
            //    context => context.UseMySql(Configuration.GetConnectionString("MySqlConnection"))
            //);


            services.AddControllers(f =>
            {
                //f.Filters.Add<MyExceptionFilter>();
            });

            services.AddControllers()
                .AddNewtonsoftJson(
                opt => opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

            services.AddScoped<IRepository, Repository>();
            

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API Pessoas e contatos", Version = "v1" });

                var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);
                if (xmlCommentsFullPath != null)
                    c.IncludeXmlComments(xmlCommentsFullPath);
            });

            services.AddCors();
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseSwagger();
                //app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "APICourse v1"));
            }

            // Verifica e aplica migrações ao iniciar o aplicativo
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<PessoaContext>();
                dbContext.Database.MigrateAsync();
            }

            //app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "APIPessoasContatos v1"));


            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
