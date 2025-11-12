namespace PromptVersionManager.Models
{
    /// <summary>
    /// Classe que representa um Prompt no sistema de versionamento de IA
    /// </summary>
    public class Prompt
    {
        /// <summary>
        /// Identificador único do Prompt
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nome descritivo do Prompt
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Descrição detalhada do Prompt
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Conteúdo do Prompt
        /// </summary>
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// Versão do Prompt
        /// </summary>
        public int Version { get; set; } = 1;

        /// <summary>
        /// Status do Prompt (Ativo, Inativo, Arquivado, etc)
        /// </summary>
        public string Status { get; set; } = "Ativo";

        /// <summary>
        /// Data de criação do Prompt
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Data da última atualização do Prompt
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Usuário que criou o Prompt
        /// </summary>
        public string CreatedBy { get; set; } = string.Empty;

        /// <summary>
        /// Usuário que atualizou o Prompt pela última vez
        /// </summary>
        public string UpdatedBy { get; set; } = string.Empty;

        /// <summary>
        /// Tags para categorização e busca
        /// </summary>
        public string Tags { get; set; } = string.Empty;

        /// <summary>
        /// Modelo de IA associado ao Prompt
        /// </summary>
        public string ModelType { get; set; } = string.Empty;
    }
}
