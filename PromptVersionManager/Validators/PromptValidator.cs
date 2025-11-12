using PromptVersionManager.Models;

namespace PromptVersionManager.Validators
{
    /// <summary>
    /// Classe responsável por validar dados de Prompt
    /// </summary>
    public class PromptValidator
    {
        private const int MinNameLength = 3;
        private const int MaxNameLength = 255;
        private const int MinContentLength = 10;
        private const int MaxContentLength = 10000;
        private const int MaxDescriptionLength = 1000;
        private const int MaxTagsLength = 500;

        /// <summary>
        /// Valida um objeto Prompt para criação
        /// </summary>
        /// <param name="prompt">Prompt a validar</param>
        /// <returns>Resultado da validação com lista de erros</returns>
        public ValidationResult ValidateForCreation(Prompt prompt)
        {
            var errors = new List<string>();

            if (prompt == null)
            {
                errors.Add("Prompt não pode ser nulo");
                return new ValidationResult { IsValid = false, Errors = errors };
            }

            ValidateName(prompt.Name, errors);
            ValidateContent(prompt.Content, errors);
            ValidateDescription(prompt.Description, errors);
            ValidateTags(prompt.Tags, errors);
            ValidateStatus(prompt.Status, errors);

            return new ValidationResult
            {
                IsValid = errors.Count == 0,
                Errors = errors
            };
        }

        /// <summary>
        /// Valida um objeto Prompt para atualização
        /// </summary>
        /// <param name="prompt">Prompt a validar</param>
        /// <returns>Resultado da validação com lista de erros</returns>
        public ValidationResult ValidateForUpdate(Prompt prompt)
        {
            var errors = new List<string>();

            if (prompt == null)
            {
                errors.Add("Prompt não pode ser nulo");
                return new ValidationResult { IsValid = false, Errors = errors };
            }

            if (prompt.Id <= 0)
            {
                errors.Add("ID do prompt deve ser maior que zero");
            }

            ValidateName(prompt.Name, errors);
            ValidateContent(prompt.Content, errors);
            ValidateDescription(prompt.Description, errors);
            ValidateTags(prompt.Tags, errors);
            ValidateStatus(prompt.Status, errors);

            return new ValidationResult
            {
                IsValid = errors.Count == 0,
                Errors = errors
            };
        }

        /// <summary>
        /// Valida o nome do Prompt
        /// </summary>
        private void ValidateName(string name, List<string> errors)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                errors.Add("Nome do prompt é obrigatório");
                return;
            }

            if (name.Length < MinNameLength)
            {
                errors.Add($"Nome deve ter no mínimo {MinNameLength} caracteres");
            }

            if (name.Length > MaxNameLength)
            {
                errors.Add($"Nome não pode exceder {MaxNameLength} caracteres");
            }
        }

        /// <summary>
        /// Valida o conteúdo do Prompt
        /// </summary>
        private void ValidateContent(string content, List<string> errors)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                errors.Add("Conteúdo do prompt é obrigatório");
                return;
            }

            if (content.Length < MinContentLength)
            {
                errors.Add($"Conteúdo deve ter no mínimo {MinContentLength} caracteres");
            }

            if (content.Length > MaxContentLength)
            {
                errors.Add($"Conteúdo não pode exceder {MaxContentLength} caracteres");
            }
        }

        /// <summary>
        /// Valida a descrição do Prompt
        /// </summary>
        private void ValidateDescription(string description, List<string> errors)
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                return; // Descrição é opcional
            }

            if (description.Length > MaxDescriptionLength)
            {
                errors.Add($"Descrição não pode exceder {MaxDescriptionLength} caracteres");
            }
        }

        /// <summary>
        /// Valida as tags do Prompt
        /// </summary>
        private void ValidateTags(string tags, List<string> errors)
        {
            if (string.IsNullOrWhiteSpace(tags))
            {
                return; // Tags são opcionais
            }

            if (tags.Length > MaxTagsLength)
            {
                errors.Add($"Tags não podem exceder {MaxTagsLength} caracteres");
            }

            // Validar formato de tags (separadas por vírgula)
            var tagList = tags.Split(',', StringSplitOptions.RemoveEmptyEntries);
            if (tagList.Length > 20)
            {
                errors.Add("Máximo de 20 tags permitidas");
            }

            foreach (var tag in tagList)
            {
                if (tag.Trim().Length == 0)
                {
                    errors.Add("Tags não podem estar vazias");
                    break;
                }
            }
        }

        /// <summary>
        /// Valida o status do Prompt
        /// </summary>
        private void ValidateStatus(string status, List<string> errors)
        {
            var validStatuses = new[] { "Ativo", "Inativo", "Arquivado", "Revisão" };

            if (string.IsNullOrWhiteSpace(status))
            {
                errors.Add("Status é obrigatório");
                return;
            }

            if (!validStatuses.Contains(status))
            {
                errors.Add($"Status inválido. Status válidos: {string.Join(", ", validStatuses)}");
            }
        }
    }

    /// <summary>
    /// Classe que representa o resultado de uma validação
    /// </summary>
    public class ValidationResult
    {
        /// <summary>
        /// Indica se a validação foi bem-sucedida
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// Lista de erros encontrados durante a validação
        /// </summary>
        public List<string> Errors { get; set; } = new List<string>();
    }
}
