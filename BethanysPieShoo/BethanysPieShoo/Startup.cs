using BethanysPieShoo.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BethanysPieShoo
{
    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddScoped<IPieRepository, PieRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();

            // Why is this line very important?
            // When the user now comes to my site, I'm going to create a scoped shopping cart using the GetCart methos.
            // In other words, the GetCart method is going to be invoked when the user sends a request.
            // That gives me the ability to check if the cart ID is already in the session, if not, I pass it into this seesion
            // and I return the ShoppingCart itself down here.
            // This way, I'm sure that when a user comes to the site, a shopping cart will be associated with the request.
            // And since it's scoper, it means it all interacts with that same shopping cart, withing that same request, we'll use that
            // same ShoppingCart.
            services.AddScoped<ShoppingCart>(sp => ShoppingCart.GetCart(sp));
            services.AddSession();
            services.AddHttpContextAccessor();

            // This will bring in support for working with MVC in our application.
            // It replace services.AddMVC() service call
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseSession();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
