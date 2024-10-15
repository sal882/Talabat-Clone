using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Talabat.Apis.Errors;
using Talabat.Apis.Extensions;
using Talabat.Apis.Helpers;
using Talabat.Apis.Middlewares;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Repositories;
using Talabat.Repository;
using Talabat.Repository.Data;
using Talabat.Repository.Identity;

namespace Talabat.Apis
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            #region Congigure Service
            builder.Services.AddControllers();

            //Call Swigger extension method to add there services
            builder.Services.AddSwaggerServices();

            //Add StoreContext Class Service by Dependancy Injection
            builder.Services.AddDbContext<StoreContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            //Add ApplicationIdentity Class Service by Dependancy Injection
            builder.Services.AddDbContext<ApplicationIdentityDbContext>(option =>
            {
                option.UseSqlServer(builder.Configuration.GetConnectionString("Identity"));
            });

            //Register the service for redis 
            builder.Services.AddSingleton<IConnectionMultiplexer>(S =>
            {
                var connectionString = builder.Configuration.GetConnectionString("Redis");

                return ConnectionMultiplexer.Connect(connectionString);
            });

            //REgister the IBasketRepository Service
            builder.Services.AddScoped(typeof(IBasketRepository), typeof(BasketRepository));

            builder.Services.AddApplictionServices();

            builder.Services.AddIdentityServices(builder.Configuration);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("MyPolicy", options =>
                {
                    options.AllowAnyOrigin();
                    options.AllowAnyMethod();
                    options.AllowAnyHeader();
                });
            });

            #endregion

            var app = builder.Build();

            //fetch all Scoped Services from app and put it in scope container
            // should use using keyword to dispose all objs that will be created from this services
            using var scope = app.Services.CreateScope();
            // fetch service itself 
            var services = scope.ServiceProvider;
            //create service form class that implement ILoggerFactory Interface to
            //log exceprion or Errors in consle in special way to be understanded
            var loggerFacory = services.GetRequiredService<ILoggerFactory>();

            try
            {
                //try to ask from CLR to Create Obj from StoreContext Service
                var dbContext = services.GetRequiredService<StoreContext>();
                //try to applay all migration that not applied to database
                await dbContext.Database.MigrateAsync();
               
                //try to ask from CLR to Create Obj from StoreContext 
                var identityDbContext = services.GetRequiredService<ApplicationIdentityDbContext>();
                //try to applay all migration that not applied to database
                await identityDbContext.Database.MigrateAsync();

                //Asking CLR to Create opj from userManager Explicitly
                var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                //Try to seed all User data for just first time
                await ApplicationIdentitySeed.SeedUsers(userManager);

                //Try to seed all data for just first time
                await StoreContextSeed.SeedAsync(dbContext);
            }
            catch (Exception ex)
            {
                //Create Obj from ILooger to log at Console
                var logger = loggerFacory.CreateLogger<Program>();
                //Log error At console withe specific Messege
                logger.LogError(ex, "An Error Occure during Apply Migrations on Database");

            }



            #region Configure Pilplines (Middlewares)
            // Configure the HTTP request pipeline.
            app.UseMiddleware<ExceptionMiddleware>();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerMiddlewares();
            }

            app.UseStatusCodePagesWithReExecute("/errors/{0}");

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseCors("MyPolicy");

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            #endregion

            app.Run();
        }
    }
}