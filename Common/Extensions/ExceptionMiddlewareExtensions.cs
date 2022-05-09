using Freshness.Common.CustomExceptions;
using Freshness.Common.EcxeptionModel;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Net;

namespace Freshness.Common.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    var exception = context.Features.Get<IExceptionHandlerFeature>().Error;

                    if (exception == null)
                    {
                        return;
                    }

                    var exceptionResponseModel = new ExceptionResponseModel();
                    int statusCode;

                    if (exception is CustomException)
                    {
                        statusCode = (int)HttpStatusCode.BadRequest;
                    }
                    else
                    {
                        statusCode = (int)HttpStatusCode.InternalServerError;
                        exceptionResponseModel.StackTrace = env.IsDevelopment() ? exception.StackTrace : null;
                    }

                    exceptionResponseModel.StatusCode = statusCode;

                    exceptionResponseModel.ExceptionDetails = new List<ExceptionDetail>
                    {
                        new ExceptionDetail
                        {
                            Key = "general",
                            Message = exception.Message
                        }
                    };

                    context.Response.StatusCode = statusCode;
                    context.Response.ContentType = "application/json";

                    await context.Response.WriteAsync(exceptionResponseModel.ToString());
                });
            });
        }
    }
}
