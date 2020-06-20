using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Data;
using Infrastructure;
using Infrastructure.EFCore;
using Infrastructure.Repositories;
using Logic;
using Logic.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Application
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
            services.AddDbContext<InventarioDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddControllers();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });

            services.AddScoped<IProductUseCases, ProductUseCases>();
            services.AddScoped<IPriceUseCases, PriceUseCases>();
            services.AddScoped<IClientUseCases, ClientUseCases>();
            services.AddScoped<IVendorUseCases, VendorUseCases>();
            services.AddScoped<ICategoryUseCases, Logic.CategoryUseCases>();


            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IPriceRepository, PriceRepository>();
            services.AddScoped<IClientRepository, ClientRepository>();
            services.AddScoped<IVendorRepository, VendorRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("CorsPolicy");

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
