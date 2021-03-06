﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization; 
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Localization; // Adicionado para tratar o localização (País)
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using VendasWebMvc.Models;
using VendasWebMvc.Data;
using VendasWebMvc.Services;


namespace VendasWebMvc
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddDbContext<VendasWebMvcContext>(options =>
                    options.UseMySql(Configuration.GetConnectionString("VendasWebMvcContext"), builder =>
                                      builder.MigrationsAssembly("VendasWebMvc")));  // Alterado para MySQL assim como os paramentros (Vendas ...)

            services.AddScoped<SeedingService>();  // Registo do SeedingService no sistema de injeção de dependencias da Aplicação.
            services.AddScoped<SellerService>();   // Registo do SellerService no sistema de injeção de dependencias da Aplicação.
            services.AddScoped<DepartmentService>();  // services é do método ConfigureServices, acima, e é o parametro passado.
            services.AddScoped<SalesRecordService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, SeedingService seedingService)  // Adicionado SeedingService. Como já está registado em cima na Injeção de Dependencias reconhece automatico.
        {
           // var ptPT = new CultureInfo("pt-PT");
            var enUs = new CultureInfo("en-US");

            var localizationOptions = new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(enUs),
                SupportedCultures = new List<CultureInfo> { enUs },
                SupportedUICultures = new List<CultureInfo> { enUs }
            };

            app.UseRequestLocalization(localizationOptions);   //passa o objeto acima definido em var.
           
            if (env.IsDevelopment())   // Se estiver no perfil de desenvolvimento ...
            {
                app.UseDeveloperExceptionPage();
                seedingService.Seed(); // Chamada metodo Seed em que é criada a Base Dados, caso ainda não esteja.
            }
            else // Se estiver no perfil de produção ...
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
