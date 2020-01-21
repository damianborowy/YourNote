using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Text;
using YourNote.Server.Services;
using YourNote.Server.Services.DatabaseService;
using YourNote.Shared.Models;


namespace YourNote.Server
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().AddNewtonsoftJson();
            services.AddResponseCompression(opts =>
            {
                opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                    new[] { "application/octet-stream" });
            });

            var key = Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("JWTSECRET"));
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });



            services.AddScoped<IUserAuthenticateService, UserAuthenticateService>();
            services.AddSingleton(new MongoClient(GetConnectionData()));
            services.AddScoped<IDatabaseService<User>, MongoDbService<User>>();
            services.AddScoped<IDatabaseService<Note>, MongoDbService<Note>>();

            services.AddScoped<IDatabaseService<Shared.Models.Tag>, MongoDbService<Shared.Models.Tag>>();
            services.AddScoped<IDatabaseService<Shared.Models.Lecture>, MongoDbService<Shared.Models.Lecture>>();

            //-------------------------------Scopy do relacyjnenj bazy ----------------------
            //services.AddTransient<FluentMigratorService>();
            //services.AddScoped<IDatabaseService<Note>, NhibernateService<Note>>();
            //services.AddScoped<IDatabaseService<User>, NhibernateService<User>>();
            //services.AddScoped<IDatabaseService<Tag>, NhibernateService<Tag>>();
            //services.AddScoped<IDatabaseService<Lecture>, NhibernateService<Lecture>>();

            services.AddCors();
            services.AddControllers();


            string GetConnectionData() => Environment.GetEnvironmentVariable("MONGODB_CONNECTION_STRING");
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseResponseCompression();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBlazorDebugging();
            }

            app.UseStaticFiles();
            app.UseClientSideBlazorFiles<Startup>();

            app.UseRouting();

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapDefaultControllerRoute();
                endpoints.MapFallbackToClientSideBlazor<Startup>("index.html");
            });
        }
    }
}