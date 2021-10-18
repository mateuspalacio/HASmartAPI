using HASmart.Core;
using HASmart.Core.Repositories;
using HASmart.Core.Services;
using HASmart.Infrastructure.EFDataAccess;
using HASmart.Infrastructure.EFDataAccess.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HASmart.WebApi
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
            
            services.AddDbContext<AppDBContext>(opt => opt.UseMySql(Configuration.GetConnectionString("HASmartContext"), ServerVersion.AutoDetect(Configuration.GetConnectionString("HASmartContext"))));
            services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);
            services.AddTransient<ICidadaoRepository, CidadaoRepository>();
            services.AddTransient<IFarmaciaRepository, FarmaciaRepository>();
            services.AddTransient<IMedicoRepository, MedicoRepository>();
            services.AddTransient<CidadaoService>();
            services.AddTransient<FarmaciaService>();
            services.AddTransient<MedicoService>();

            IsoDateTimeConverter converter = new IsoDateTimeConverter
            {
                Culture = System.Globalization.CultureInfo.InvariantCulture,
                DateTimeFormat = "dd/MM/yyyy",
                DateTimeStyles = System.Globalization.DateTimeStyles.AssumeLocal
            };
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                options.SerializerSettings.Converters.Add(converter);
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "HASmart.WebApi", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "HASmart.WebApi v1"));
            }
            app.UseCors(x => x
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
