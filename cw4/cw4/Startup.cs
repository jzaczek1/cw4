using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cw4.DAL;
using cw4.Middlewares;
using cw4.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Web;
using Microsoft.AspNetCore.Authentication;
using cw4.Handlers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace cw4
{
    public class Startup
    {
        private object encoding;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddTransient<SqlServerStudentDbService, SqlServerStudentDbService>;
            services.AddScoped<IStudentsDbService, SqlServerStudentDbService>();
            services.AddControllers();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidIssuer = "Zaku",
                        ValidAudience = "Jan ¯",
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["SecretKey"]))
                    };
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IStudentsDbService serv)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            // odt¹d
            //app.UseHttpsRedirection();
            app.UseMiddleware<LoggingMiddleware>();

            app.Use(async (context, next) =>
            {
                if (!context.Request.Headers.ContainsKey("Index"))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Nie podano indeksu w nag³ówku");
                    return;
                }

                //to najpierw
                // plik z generowym plikiem middleware byl w folderze middleware albo bezposrednio do katalgu projektu (oprocz
                // debug katalogu ) - musi byc dopisywany a nie tworzony
                //var bodyStream = string.Empty;
                //using (var reader = new StreamReader(HttpContext.Request.Body, Encoding.UTF8, true, 1024, true))
                //{
                //    bodyStream = await reader.ReadToEndAsync();
                //}

                //HttpContext.Request.EnableBuffering(); /*(na pocz¹tku)*/

                //    HttpContext.Request.Body.Seek(0, SeekOrigin.Begin);
                //    //(na koñcu przed await _next...)

                var index = context.Request.Headers["Index"].ToString();

                if (!serv.CheckIndex(index)) // sprawdzenie czy student wystepuje w bazie danych
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Nie ma indeksu w bazie");
                    return;
                }

                await next();
            });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
