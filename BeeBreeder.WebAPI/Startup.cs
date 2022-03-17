using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Web.Http;
using BeeBreeder.Breeding.Analyzer;
using BeeBreeder.Breeding.Flusher;
using BeeBreeder.Breeding.ProbabilityUtils.Model.Strategy;
using BeeBreeder.Breeding.Simulator;
using BeeBreeder.Breeding.Targeter;
using BeeBreeder.Common.AlleleDatabase.Bee;
using BeeBreeder.Common.Model.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;

namespace BeeBreeder.WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.Converters.Add(new StringEnumConverter());
            }).AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

            services.AddScoped<IBreedAnalyzer, ExtendedNaturalSelectionAnalyzer>();
            services.AddScoped<IBreedFlusher, ExtendedNaturalSelectionFlusher> ();
            services.AddSingleton<MutationTree>(MutationTree.FromSpecieCombinations(BeeGeneticDatabase.SpecieCombinations));
            services.AddScoped<IStrategyUtils, StrategyUtils>();
            services.AddScoped<IBreedingSimulator, BreedingSimulator>();
            services.AddScoped<ISpecieTargeter, BestGenesTargeter>();

            services.AddCors();
            //services.AddSingleton<IBeeBreeder, NaturalSelectionBreeder>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "BeeBreeder.WebAPI", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BeeBreeder.WebAPI v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(builder => builder.AllowAnyOrigin());

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}