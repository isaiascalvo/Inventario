using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Application.Middlewares
{
    public class ErrorHandling
    {
        private readonly RequestDelegate next;
        public ErrorHandling(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context /* other dependencies */)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var code = HttpStatusCode.InternalServerError;
            var message = ex.Message;
            var title = "";

            if (ex is KeyNotFoundException)
            {
                code = HttpStatusCode.NotFound;
                title = ((KeyNotFoundException)ex).Message;
            }
            else if (ex is InvalidOperationException)
            {
                code = HttpStatusCode.Conflict;
                title = ((InvalidOperationException)ex).Message;
            }
            else if (ex is ApplicationException)
            {
                code = HttpStatusCode.BadRequest;
                title = ((ApplicationException)ex).Message;
            }
            else
            {
                code = HttpStatusCode.NotFound;
                message = "Le pedimos disculpas, ha ocurrido un error inesperado, por favor contáctese con el administrador.";
                title = "Error";
            }

            Log.Error(ex, string.Empty);

            var result = JsonConvert.SerializeObject(new { message = message, title = title });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            await context.Response.WriteAsync(result);
            return;
        }
    }
}
