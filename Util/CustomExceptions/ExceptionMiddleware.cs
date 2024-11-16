﻿using BackendLaboratory.Util.CustomExceptions.Exceptions;
using System.Net;
using System.Text.Json;

namespace BackendLaboratory.Util.CustomExceptions
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                await HandleExceptionAsync(context, exception);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            var statusCode = (int)HttpStatusCode.InternalServerError;
            string message = "Internal Server Error";

            if (exception is BadRequestException)
            {
                statusCode = (int)HttpStatusCode.BadRequest;
                message = !string.IsNullOrEmpty(exception.Message) ? exception.Message : "Плохой запрос";
            }
            else if (exception is UnauthorizedException)
            {
                statusCode = (int)HttpStatusCode.Unauthorized;
                message = !string.IsNullOrEmpty(exception.Message) ? exception.Message : "Данный пользователь не авторизован";
            }
            else if (exception is InternalServerErrorException)
            {
                statusCode = (int)HttpStatusCode.InternalServerError;
                message = !string.IsNullOrEmpty(exception.Message) ? exception.Message : "Внутренняя ошибка сервера";
            }
            else if (exception is ForbiddenException)
            {
                statusCode = (int)HttpStatusCode.Forbidden;
                message = !string.IsNullOrEmpty(exception.Message) ? exception.Message : "Вход запрещен";
            }
            else if (exception is NotFoundException)
            {
                statusCode = (int)HttpStatusCode.NotFound;
                message = !string.IsNullOrEmpty(exception.Message) ? exception.Message : "Не найдено";
            }

            response.StatusCode = statusCode;

            var error = new HttpException((HttpStatusCode)statusCode, message);

            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(error, options);

            await response.WriteAsync(json);
        }
    }
}