using System.Diagnostics;
using System.Net;
using Newtonsoft.Json;
using WebApi.Services;

namespace WebApi.Middlewares
{
    public class CustomExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILoggerService _logService;

        public CustomExceptionMiddleware(RequestDelegate next, ILoggerService logService)
        {
            _next = next;
            _logService = logService;
        }

        public async Task Invoke(HttpContext context)
        {
            var watch = Stopwatch.StartNew();            
            try
            {
                string message = "[Request] HTTP " + context.Request.Method + " - " + context.Request.Path;
                _logService.Log(message);
                await _next(context);
                watch.Stop();
                message = "[Response] HTTP " + context.Request.Method + " - " + context.Request.Path + " responded " + 
                    context.Response.StatusCode + " in " + watch.ElapsedMilliseconds + "ms";
                _logService.Log(message);
            }
            catch(Exception ex)
            {
                watch.Stop();
                await HandleException(context, ex, watch);
            }
        }

        private Task HandleException(HttpContext context, Exception ex, Stopwatch watch)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";            
            var result = JsonConvert.SerializeObject(new {error = ex.Message}, Formatting.None);
            string message = "[Error] HTTP " + context.Request.Method + " - " + context.Response.StatusCode + " Error Message " + 
                ex.Message + " in " + watch.ElapsedMilliseconds + "ms";
            _logService.Log(message);    
            return context.Response.WriteAsync(result);
        }
    }

    public static class CustomExceptionMiddlewareExtension
    {
        public static IApplicationBuilder UseCustomExceptionMiddleware(this IApplicationBuilder builder) => 
            builder.UseMiddleware<CustomExceptionMiddleware>();
    }
}