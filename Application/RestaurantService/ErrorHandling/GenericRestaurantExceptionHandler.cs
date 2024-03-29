﻿using Common.ErrorModels;
using Microsoft.AspNetCore.Diagnostics;
using RestaurantService.Repository;
using System.ComponentModel;
using RestaurantService.Model;
using Serilog;
using Serilog.Events;

namespace RestaurantService.ErrorHandling
{
    public static class GenericRestaurantExceptionHandler
    {

        

        public static void ConfigureExceptionHandler(this IApplicationBuilder app, IDbLogger logger
        )
        {
            
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    var error = contextFeature.Error;

                    if (contextFeature != null)
                    {
                        if (error is HttpStatusException)
                        {
                            var errorType = (HttpStatusException)error;
                            // TODO Maybe do error logs to db
                            //dbLogger.Error(errorType.Message, errorType.StatusCode);
                            context.Response.StatusCode = errorType.StatusCode;

                            logger.Error(errorType.Message, errorType.StatusCode);
                            await context.Response.WriteAsync(new ExceptionDto()
                            {
                                StatusCode = errorType.StatusCode,
                                Message = errorType.Message
                            }.ToString());

                        }
                        else
                        {
                            //dbLogger.Error(error.Message, StatusCodes.Status500InternalServerError);
                            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                            logger.Error("Internal Server Error", StatusCodes.Status500InternalServerError);

                            await context.Response.WriteAsync(new ExceptionDto()
                            {
                                StatusCode = StatusCodes.Status500InternalServerError,
                                Message = "Internal Server Error."
                            }.ToString());
                        }
                    }
                });
            });
        }
    }
}
