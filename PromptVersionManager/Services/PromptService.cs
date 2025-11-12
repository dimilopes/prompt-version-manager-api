using PromptVersionManager.Data;
using PromptVersionManager.Models;

namespace PromptVersionManager.Services
{
    /// <summary>
    /// Implementação do serviço de Prompts com lógica de negócio
    /// </summary>
    public class PromptService : IPromptService
    {
        private readonly IPromptRepository _promptRepository;
        private readonly ILogger<PromptService> _logger;

        public PromptService(IPromptRepository promptRepository, ILogger<PromptService> logger)
        {
            _promptRepository = promptRepository;
            _logger = logger;
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
                throw;
            }
        }

        public async Task<Prompt> GetPromptByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation($"Obtendo prompt com ID: {id}");
                var prompt = await _promptRepository.GetByIdAsync(id);
                
                if (prompt == null)
                {
                    _logger.LogWarning($"Prompt com ID {id} não encontrado");
                    throw new KeyNotFoundException($"Prompt com ID {id} não encontrado");
                }

                return prompt;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao obter prompt com ID {id}");
                throw;
            }
        }

        public async Task<IEnumerable<Prompt>> SearchPromptsByNameAsync(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    throw new ArgumentException("Nome não pode estar vazio", nameof(name));
                }

                _logger.LogInformation($"Buscando prompts por nome: {name}");
                return await _promptRepository.GetByNameAsync(name);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao buscar prompts por nome: {name}");
                throw;
            }
        }

        public async Task<Prompt> CreatePromptAsync(Prompt prompt)
        {
            try
            {
                if (prompt == null)
                {
                    throw new ArgumentNullException(nameof(prompt));
                }

                if (string.IsNullOrWhiteSpace(prompt.Name))
                {
                    throw new ArgumentException("Nome do prompt é obrigatório", nameof(prompt.Name));
                }

                if (string.IsNullOrWhiteSpace(prompt.Content))
                {
                    throw new ArgumentException("Conteúdo do prompt é obrigatório", nameof(prompt.Content));
                }

                prompt.CreatedAt = DateTime.UtcNow;
                prompt.UpdatedAt = DateTime.UtcNow;
                prompt.Version = 1;

                _logger.LogInformation($"Criando novo prompt: {prompt.Name}");
                var id = await _promptRepository.CreateAsync(prompt);
                prompt.Id = id;

                return prompt;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar novo prompt");
                throw;
            }
        }

        public async Task<Prompt> UpdatePromptAsync(int id, Prompt prompt)
        {
            try
            {
                if (prompt == null)
                {
                    throw new ArgumentNullException(nameof(prompt));
                }

                // Verificar se o prompt existe
                var existingPrompt = await _promptRepository.GetByIdAsync(id);
                if (existingPrompt == null)
                {
                    _logger.LogWarning($"Prompt com ID {id} não encontrado para atualização");
                    throw new KeyNotFoundException($"Prompt com ID {id} não encontrado");
                }

                prompt.Id = id;
                prompt.UpdatedAt = DateTime.UtcNow;

                _logger.LogInformation($"Atualizando prompt com ID: {id}");
                var success = await _promptRepository.UpdateAsync(prompt);

                if (!success)
                {
                    throw new InvalidOperationException("Falha ao atualizar o prompt");
                }

                return prompt;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao atualizar prompt com ID {id}");
                throw;
            }
        }

        public async Task DeletePromptAsync(int id)
        {
            try
            {
                // Verificar se o prompt existe
                var prompt = await _promptRepository.GetByIdAsync(id);
                if (prompt == null)
                {
                    _logger.LogWarning($"Prompt com ID {id} não encontrado para deleção");
                    throw new KeyNotFoundException($"Prompt com ID {id} não encontrado");
                }

                _logger.LogInformation($"Deletando prompt com ID: {id}");
                var success = await _promptRepository.DeleteAsync(id);

                if (!success)
                {
                    throw new InvalidOperationException("Falha ao deletar o prompt");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao deletar prompt com ID {id}");
                throw;
            }
        }

        public async Task<IEnumerable<Prompt>> GetPromptsByStatusAsync(string status)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(status))
                {
                    throw new ArgumentException("Status não pode estar vazio", nameof(status));
                }

                _logger.LogInformation($"Obtendo prompts com status: {status}");
                return await _promptRepository.GetByStatusAsync(status);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao obter prompts com status: {status}");
                throw;
            }
        }

        public async Task<IEnumerable<Prompt>> GetPromptsByModelTypeAsync(string modelType)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(modelType))
                {
                    throw new ArgumentException("Tipo de modelo não pode estar vazio", nameof(modelType));
                }

                _logger.LogInformation($"Obtendo prompts para modelo: {modelType}");
                return await _promptRepository.GetByModelTypeAsync(modelType);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao obter prompts para modelo: {modelType}");
                throw;
            }
        }

        public async Task<int> IncrementVersionAsync(int id)
        {
            try
            {
                var prompt = await GetPromptByIdAsync(id);
                prompt.Version++;
                await UpdatePromptAsync(id, prompt);

                _logger.LogInformation($"Versão do prompt {id} incrementada para {prompt.Version}");
                return prompt.Version;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao incrementar versão do prompt {id}");
                throw;
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
                throw;
            }
        }
    }
}
