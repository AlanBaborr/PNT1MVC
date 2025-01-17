﻿using CarritoMVC.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;

namespace CarritoMVC
{
    public static class Startup
    {   

        public static WebApplication InicializarApp(string[] args)
        {
            //Crear una nueva instancia de nuestro servidor Web
            var builder = WebApplication.CreateBuilder(args);
            ConfigureServices(builder); //Lo configuramos, con sus respectivos servicios

            //Nuevo
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(10);
            });
            var app = builder.Build(); 
            app.UseSession();
            Configure(app); 


            return app; //Retornamos la App ya inicializada.
        }
        private static void ConfigureServices(WebApplicationBuilder builder)
        {
            // Add services to the container.
            builder.Services.AddDbContext<CarritoContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("CarritoDBCS")));
            builder.Services.AddControllersWithViews();
            
        }

        //private static void ConfigureServices(IServiceCollection services)
        //{
        //    services.AddSession();
        //    services.AddMvc();
        //}

        private static void Configure(WebApplication app)
        {
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            
            //app.UseSession();


            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        }
    }
}
