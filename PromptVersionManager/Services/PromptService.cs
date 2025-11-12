using PromptVersionManager.Data;
using PromptVersionManager.Exceptions;
using PromptVersionManager.Models;
using PromptVersionManager.Validators;

namespace PromptVersionManager.Services
{
    /// <summary>
    /// Implementação do serviço de Prompts com lógica de negócio
    /// </summary>
    public class PromptService : IPromptService
    {
        private readonly IPromptRepository _promptRepository;
        private readonly ILogger<PromptService> _logger;
        private readonly PromptValidator _validator;

        public PromptService(IPromptRepository promptRepository, ILogger<PromptService> logger)
        {
            _promptRepository = promptRepository;
            _logger = logger;
            _validator = new PromptValidator();
        }

        public async Task<IEnumerable<Prompt>> GetAllPromptsAsync()
        {
            try
            {
                _logger.LogInformation("Obtendo todos os prompts");
                return await _promptRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter todos os prompts");
                throw new DatabaseException("Erro ao obter prompts do banco de dados", ex);
            }
        }

        public async Task<Prompt> GetPromptByIdAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    throw new ValidationException("ID do prompt deve ser maior que zero", new List<string> { "ID inválido" });
                }

                _logger.LogInformation($"Obtendo prompt com ID: {id}");
                var prompt = await _promptRepository.GetByIdAsync(id);
                
                if (prompt == null)
                {
                    _logger.LogWarning($"Prompt com ID {id} não encontrado");
                    throw new ResourceNotFoundException($"Prompt com ID {id} não encontrado");
                }

                return prompt;
            }
            catch (Exception ex) when (!(ex is ApiException))
            {
                _logger.LogError(ex, $"Erro ao obter prompt com ID {id}");
                throw new DatabaseException($"Erro ao obter prompt com ID {id}", ex);
            }
        }

        public async Task<IEnumerable<Prompt>> SearchPromptsByNameAsync(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    throw new ValidationException("Nome não pode estar vazio", new List<string> { "Nome é obrigatório" });
                }

                if (name.Length > 255)
                {
                    throw new ValidationException("Nome muito longo", new List<string> { "Nome não pode exceder 255 caracteres" });
                }

                _logger.LogInformation($"Buscando prompts por nome: {name}");
                return await _promptRepository.GetByNameAsync(name);
            }
            catch (Exception ex) when (!(ex is ApiException))
            {
                _logger.LogError(ex, $"Erro ao buscar prompts por nome: {name}");
                throw new DatabaseException($"Erro ao buscar prompts por nome", ex);
            }
        }

        public async Task<Prompt> CreatePromptAsync(Prompt prompt)
        {
            try
            {
                if (prompt == null)
                {
                    throw new ValidationException("Dados do prompt são obrigatórios", new List<string> { "Prompt não pode ser nulo" });
                }

                // Validar usando o validador
                var validationResult = _validator.ValidateForCreation(prompt);
                if (!validationResult.IsValid)
                {
                    throw new ValidationException("Erro na validação do prompt", validationResult.Errors);
                }

                prompt.CreatedAt = DateTime.UtcNow;
                prompt.UpdatedAt = DateTime.UtcNow;
                prompt.Version = 1;

                _logger.LogInformation($"Criando novo prompt: {prompt.Name}");
                var id = await _promptRepository.CreateAsync(prompt);
                prompt.Id = id;

                _logger.LogInformation($"Prompt criado com sucesso. ID: {id}");
                return prompt;
            }
            catch (Exception ex) when (!(ex is ApiException))
            {
                _logger.LogError(ex, "Erro ao criar novo prompt");
                throw new DatabaseException("Erro ao criar prompt no banco de dados", ex);
            }
        }

        public async Task<Prompt> UpdatePromptAsync(int id, Prompt prompt)
        {
            try
            {
                if (prompt == null)
                {
                    throw new ValidationException("Dados do prompt são obrigatórios", new List<string> { "Prompt não pode ser nulo" });
                }

                if (id <= 0)
                {
                    throw new ValidationException("ID inválido", new List<string> { "ID do prompt deve ser maior que zero" });
                }

                // Verificar se o prompt existe
                var existingPrompt = await _promptRepository.GetByIdAsync(id);
                if (existingPrompt == null)
                {
                    _logger.LogWarning($"Prompt com ID {id} não encontrado para atualização");
                    throw new ResourceNotFoundException($"Prompt com ID {id} não encontrado");
                }

                // Validar usando o validador
                prompt.Id = id;
                var validationResult = _validator.ValidateForUpdate(prompt);
                if (!validationResult.IsValid)
                {
                    throw new ValidationException("Erro na validação do prompt", validationResult.Errors);
                }

                prompt.UpdatedAt = DateTime.UtcNow;
                // Preservar a versão anterior se não foi alterada
                if (prompt.Version == 0)
                {
                    prompt.Version = existingPrompt.Version;
                }

                _logger.LogInformation($"Atualizando prompt com ID: {id}");
                var success = await _promptRepository.UpdateAsync(prompt);

                if (!success)
                {
                    throw new ConflictException("Falha ao atualizar o prompt. Tente novamente.");
                }

                _logger.LogInformation($"Prompt {id} atualizado com sucesso");
                return prompt;
            }
            catch (Exception ex) when (!(ex is ApiException))
            {
                _logger.LogError(ex, $"Erro ao atualizar prompt com ID {id}");
                throw new DatabaseException($"Erro ao atualizar prompt no banco de dados", ex);
            }
        }

        public async Task DeletePromptAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    throw new ValidationException("ID inválido", new List<string> { "ID do prompt deve ser maior que zero" });
                }

                // Verificar se o prompt existe
                var prompt = await _promptRepository.GetByIdAsync(id);
                if (prompt == null)
                {
                    _logger.LogWarning($"Prompt com ID {id} não encontrado para deleção");
                    throw new ResourceNotFoundException($"Prompt com ID {id} não encontrado");
                }

                _logger.LogInformation($"Deletando prompt com ID: {id}");
                var success = await _promptRepository.DeleteAsync(id);

                if (!success)
                {
                    throw new ConflictException("Falha ao deletar o prompt. Tente novamente.");
                }

                _logger.LogInformation($"Prompt {id} deletado com sucesso");
            }
            catch (Exception ex) when (!(ex is ApiException))
            {
                _logger.LogError(ex, $"Erro ao deletar prompt com ID {id}");
                throw new DatabaseException($"Erro ao deletar prompt do banco de dados", ex);
            }
        }

        public async Task<IEnumerable<Prompt>> GetPromptsByStatusAsync(string status)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(status))
                {
                    throw new ValidationException("Status não pode estar vazio", new List<string> { "Status é obrigatório" });
                }

                var validStatuses = new[] { "Ativo", "Inativo", "Arquivado", "Revisão" };
                if (!validStatuses.Contains(status))
                {
                    throw new ValidationException("Status inválido", new List<string> { $"Status válidos: {string.Join(", ", validStatuses)}" });
                }

                _logger.LogInformation($"Obtendo prompts com status: {status}");
                return await _promptRepository.GetByStatusAsync(status);
            }
            catch (Exception ex) when (!(ex is ApiException))
            {
                _logger.LogError(ex, $"Erro ao obter prompts com status: {status}");
                throw new DatabaseException($"Erro ao obter prompts do banco de dados", ex);
            }
        }

        public async Task<IEnumerable<Prompt>> GetPromptsByModelTypeAsync(string modelType)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(modelType))
                {
                    throw new ValidationException("Tipo de modelo não pode estar vazio", new List<string> { "Tipo de modelo é obrigatório" });
                }

                _logger.LogInformation($"Obtendo prompts para modelo: {modelType}");
                return await _promptRepository.GetByModelTypeAsync(modelType);
            }
            catch (Exception ex) when (!(ex is ApiException))
            {
                _logger.LogError(ex, $"Erro ao obter prompts para modelo: {modelType}");
                throw new DatabaseException($"Erro ao obter prompts do banco de dados", ex);
            }
        }

        public async Task<int> IncrementVersionAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    throw new ValidationException("ID inválido", new List<string> { "ID do prompt deve ser maior que zero" });
                }

                var prompt = await GetPromptByIdAsync(id);
                prompt.Version++;
                await UpdatePromptAsync(id, prompt);

                _logger.LogInformation($"Versão do prompt {id} incrementada para {prompt.Version}");
                return prompt.Version;
            }
            catch (Exception ex) when (!(ex is ApiException))
            {
                _logger.LogError(ex, $"Erro ao incrementar versão do prompt {id}");
                throw new DatabaseException($"Erro ao incrementar versão do prompt", ex);
            }
        }

        public async Task<int> GetTotalCountAsync()
        {
            try
            {
                _logger.LogInformation("Obtendo contagem total de prompts");
                return await _promptRepository.GetCountAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter contagem total de prompts");
                throw new DatabaseException("Erro ao obter contagem de prompts", ex);
            }
        }
    }
}
