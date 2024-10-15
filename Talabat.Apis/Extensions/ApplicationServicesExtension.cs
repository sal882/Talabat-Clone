using Microsoft.AspNetCore.Mvc;
using Talabat.Apis.Helpers;
using Talabat.Core.Repositories;
using Talabat.Repository.Data;
using Talabat.Repository;
using Talabat.Apis.Errors;
using Talabat.Core;
using Talabat.Core.Services;
using Talabat.Service;

namespace Talabat.Apis.Extensions
{
    public static class ApplicationServicesExtension
    {
        public static IServiceCollection AddApplictionServices(this IServiceCollection services)
        {

            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IOrderSeervice, OrderService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            //Add DP for IGenericRepository to ask clr return obj from GenericRepositoty class
            //services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            //Register the AutoMaaper Service
            services.AddAutoMapper(typeof(MappingProfile));

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var errors = actionContext.ModelState.Where(P => P.Value.Errors.Count() > 0)
                                                            .SelectMany(P => P.Value.Errors)
                                                            .Select(E => E.ErrorMessage)
                                                            .ToArray();
                    var validationErrorResponse = new ApiValidationErrorResponse()
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(validationErrorResponse);
                };
            });
            return services;
        }
    }
}
