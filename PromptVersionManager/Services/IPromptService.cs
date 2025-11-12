using PromptVersionManager.Models;

namespace PromptVersionManager.Services
{
    /// <summary>
    /// Interface que define os serviços de negócio para Prompts
    /// </summary>
    public interface IPromptService
    {
        /// <summary>
        /// Obtém todos os prompts
        /// </summary>
        /// <returns>Lista de todos os prompts</returns>
        Task<IEnumerable<Prompt>> GetAllPromptsAsync();

        /// <summary>
        /// Obtém um prompt pelo ID
        /// </summary>
        /// <param name="id">ID do prompt</param>
        /// <returns>Prompt encontrado</returns>
        /// <exception cref="KeyNotFoundException">Lançado quando o prompt não é encontrado</exception>
        Task<Prompt> GetPromptByIdAsync(int id);

        /// <summary>
        /// Busca prompts por nome
        /// </summary>
        /// <param name="name">Nome do prompt para buscar</param>
        /// <returns>Lista de prompts encontrados</returns>
        Task<IEnumerable<Prompt>> SearchPromptsByNameAsync(string name);

        /// <summary>
        /// Cria um novo prompt
        /// </summary>
        /// <param name="prompt">Dados do novo prompt</param>
        /// <returns>Prompt criado com ID atribuído</returns>
        Task<Prompt> CreatePromptAsync(Prompt prompt);

        /// <summary>
        /// Atualiza um prompt existente
        /// </summary>
        /// <param name="id">ID do prompt a atualizar</param>
        /// <param name="prompt">Dados atualizados do prompt</param>
        /// <returns>Prompt atualizado</returns>
        /// <exception cref="KeyNotFoundException">Lançado quando o prompt não é encontrado</exception>
        Task<Prompt> UpdatePromptAsync(int id, Prompt prompt);

        /// <summary>
        /// Deleta um prompt
        /// </summary>
        /// <param name="id">ID do prompt a deletar</param>
        /// <exception cref="KeyNotFoundException">Lançado quando o prompt não é encontrado</exception>
        Task DeletePromptAsync(int id);

        /// <summary>
        /// Obtém prompts por status
        /// </summary>
        /// <param name="status">Status dos prompts</param>
        /// <returns>Lista de prompts com o status especificado</returns>
        Task<IEnumerable<Prompt>> GetPromptsByStatusAsync(string status);

        /// <summary>
        /// Obtém prompts por modelo de IA
        /// </summary>
        /// <param name="modelType">Tipo de modelo de IA</param>
        /// <returns>Lista de prompts para o modelo especificado</returns>
        Task<IEnumerable<Prompt>> GetPromptsByModelTypeAsync(string modelType);

        /// <summary>
        /// Incrementa a versão de um prompt
        /// </summary>
        /// <param name="id">ID do prompt</param>
        /// <returns>Versão atualizada</returns>
        Task<int> IncrementVersionAsync(int id);

        /// <summary>
        /// Obtém a contagem total de prompts
        /// </summary>
        /// <returns>Número total de prompts</returns>
        Task<int> GetTotalCountAsync();
    }
}
