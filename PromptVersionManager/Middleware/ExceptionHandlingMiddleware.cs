using System.Net;
using System.Text.Json;
using PromptVersionManager.Exceptions;

namespace PromptVersionManager.Middleware
{
    /// <summary>
    /// Middleware para tratamento global de exceções
    /// </summary>
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exceção não tratada");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var response = new ErrorResponse();

            switch (exception)
            {
                case ValidationException validationEx:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response.StatusCode = 400;
                    response.Message = validationEx.Message;
                    response.Errors = validationEx.ValidationErrors;
                    break;

                case ResourceNotFoundException notFoundEx:
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    response.StatusCode = 404;
                    response.Message = notFoundEx.Message;
                    break;

                case UnauthorizedException unauthorizedEx:
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    response.StatusCode = 401;
                    response.Message = unauthorizedEx.Message;
                    break;

                case ConflictException conflictEx:
                    context.Response.StatusCode = (int)HttpStatusCode.Conflict;
                    response.StatusCode = 409;
                    response.Message = conflictEx.Message;
                    break;

                case DatabaseException dbEx:
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    response.StatusCode = 500;
                    response.Message = dbEx.Message;
                    response.Details = dbEx.Details;
                    break;

                case ApiException apiEx:
                    context.Response.StatusCode = apiEx.StatusCode;
                    response.StatusCode = apiEx.StatusCode;
                    response.Message = apiEx.Message;
                    response.Details = apiEx.Details;
                    break;

                default:
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    response.StatusCode = 500;
                    response.Message = "Erro interno do servidor";
                    response.Details = "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde.";
                    break;
            }

            response.Timestamp = DateTime.UtcNow;
            var json = JsonSerializer.Serialize(response);
            return context.Response.WriteAsync(json);
        }
    }

    /// <summary>
    /// Classe que representa a resposta de erro padronizada
    /// </summary>
    public class ErrorResponse
    {
        /// <summary>
        /// Código HTTP do erro
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Mensagem de erro
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Detalhes adicionais do erro
        /// </summary>
        public string? Details { get; set; }

        /// <summary>
        /// Data e hora do erro
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Lista de erros de validação (se aplicável)
        /// </summary>
        public List<string>? Errors { get; set; }
    }
}
