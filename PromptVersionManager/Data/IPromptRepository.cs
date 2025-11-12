using PromptVersionManager.Models;

namespace PromptVersionManager.Data
{
    /// <summary>
    /// Interface que define as operações de acesso a dados para Prompts
    /// </summary>
    public interface IPromptRepository
    {
        /// <summary>
        /// Obtém todos os prompts
        /// </summary>
        /// <returns>Lista de todos os prompts</returns>
        Task<IEnumerable<Prompt>> GetAllAsync();

        /// <summary>
        /// Obtém um prompt pelo ID
        /// </summary>
        /// <param name="id">ID do prompt</param>
        /// <returns>Prompt encontrado ou null</returns>
        Task<Prompt?> GetByIdAsync(int id);

        /// <summary>
        /// Obtém prompts por nome (busca parcial)
        /// </summary>
        /// <param name="name">Nome do prompt para buscar</param>
        /// <returns>Lista de prompts encontrados</returns>
        Task<IEnumerable<Prompt>> GetByNameAsync(string name);

        /// <summary>
        /// Cria um novo prompt
        /// </summary>
        /// <param name="prompt">Objeto Prompt a ser criado</param>
        /// <returns>ID do prompt criado</returns>
        Task<int> CreateAsync(Prompt prompt);

        /// <summary>
        /// Atualiza um prompt existente
        /// </summary>
        /// <param name="prompt">Objeto Prompt com dados atualizados</param>
        /// <returns>True se atualizado com sucesso, False caso contrário</returns>
        Task<bool> UpdateAsync(Prompt prompt);

        /// <summary>
        /// Deleta um prompt pelo ID
        /// </summary>
        /// <param name="id">ID do prompt a deletar</param>
        /// <returns>True se deletado com sucesso, False caso contrário</returns>
        Task<bool> DeleteAsync(int id);

        /// <summary>
        /// Obtém prompts por status
        /// </summary>
        /// <param name="status">Status dos prompts a buscar</param>
        /// <returns>Lista de prompts com o status especificado</returns>
        Task<IEnumerable<Prompt>> GetByStatusAsync(string status);

        /// <summary>
        /// Obtém prompts por modelo de IA
        /// </summary>
        /// <param name="modelType">Tipo de modelo de IA</param>
        /// <returns>Lista de prompts para o modelo especificado</returns>
        Task<IEnumerable<Prompt>> GetByModelTypeAsync(string modelType);

        /// <summary>
        /// Obtém a contagem total de prompts
        /// </summary>
        /// <returns>Número total de prompts</returns>
        Task<int> GetCountAsync();
    }
}
