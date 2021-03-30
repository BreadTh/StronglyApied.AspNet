using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using BreadTh.StronglyApied.AspNet.Core;

namespace BreadTh.StronglyApied.AspNet
{
    public static class StronglyApiedAspNetExtensions
    {
        public static void UseStronglyApiedParseErrorResponse(
            this IApplicationBuilder applicationBuilder, Func<HttpContext, List<ErrorDescription>, Task> response)
        {
            _ = applicationBuilder.Use(async (HttpContext httpContext, Func<Task> next) =>
            {
                httpContext.Request.EnableBuffering(); 

                try
                {
                    await next();
                }
                catch (BodyParseException exception)
                {
                    await response(httpContext, exception.errors);
                }
            });
        }

        public static void UseStronglyApiedInputParser(this MvcOptions options) =>
            options.InputFormatters.Insert(0, new StronglyApiedInputFormatter());
    }
}
