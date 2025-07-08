using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;

namespace RoverMissionPlanner.API.Middlewares
{
	public class ExceptionHandlingMiddleware
	{
		private readonly RequestDelegate _next;

		public ExceptionHandlingMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			try
			{
				await _next(context);
			}
			catch (InvalidOperationException ex)
			{
				await HandleExceptionAsync(context, ex, HttpStatusCode.BadRequest);
			}
			catch (Exception ex)
			{
				await HandleExceptionAsync(context, ex, HttpStatusCode.InternalServerError);
			}
		}

		private Task HandleExceptionAsync(HttpContext context, Exception exception, HttpStatusCode statusCode)
		{
			context.Response.ContentType = "application/json";
			context.Response.StatusCode = (int)statusCode;

			var result = JsonSerializer.Serialize(new
			{
				error = exception.Message
			});

			return context.Response.WriteAsync(result);
		}
	}
}
