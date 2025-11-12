namespace PromptVersionManager.Exceptions
{
    /// <summary>
    /// Exceção base para erros da API
    /// </summary>
    public class ApiException : Exception
    {
        /// <summary>
        /// Código HTTP associado à exceção
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Detalhes adicionais sobre o erro
        /// </summary>
        public string? Details { get; set; }

        public ApiException(string message, int statusCode = 500, string? details = null)
            : base(message)
        {
            StatusCode = statusCode;
            Details = details;
        }

        public ApiException(string message, Exception innerException, int statusCode = 500, string? details = null)
            : base(message, innerException)
        {
            StatusCode = statusCode;
            Details = details;
        }
    }

    /// <summary>
    /// Exceção para erros de validação
    /// </summary>
    public class ValidationException : ApiException
    {
        /// <summary>
        /// Lista de erros de validação
        /// </summary>
        public List<string> ValidationErrors { get; set; }

        public ValidationException(string message, List<string> validationErrors)
            : base(message, 400)
        {
            ValidationErrors = validationErrors;
        }
    }

    /// <summary>
    /// Exceção para recursos não encontrados
    /// </summary>
    public class ResourceNotFoundException : ApiException
    {
        public ResourceNotFoundException(string message)
            : base(message, 404)
        {
        }
    }

    /// <summary>
    /// Exceção para operações não permitidas
    /// </summary>
    public class UnauthorizedException : ApiException
    {
        public UnauthorizedException(string message)
            : base(message, 401)
        {
        }
    }

    /// <summary>
    /// Exceção para conflitos de dados
    /// </summary>
    public class ConflictException : ApiException
    {
        public ConflictException(string message)
            : base(message, 409)
        {
        }
    }

    /// <summary>
    /// Exceção para erros de banco de dados
    /// </summary>
    public class DatabaseException : ApiException
    {
        public DatabaseException(string message, Exception innerException)
            : base(message, innerException, 500, "Erro ao acessar o banco de dados")
        {
        }
    }
}
