using Dapper;
using PromptVersionManager.Models;

namespace PromptVersionManager.Data
{
    /// <summary>
    /// Implementação do repositório de Prompts usando Dapper
    /// </summary>
    public class PromptRepository : IPromptRepository
    {
        private readonly DatabaseConnection _databaseConnection;

        public PromptRepository(DatabaseConnection databaseConnection)
        {
            _databaseConnection = databaseConnection;
        }

        public async Task<IEnumerable<Prompt>> GetAllAsync()
        {
            using (var connection = _databaseConnection.GetConnection())
            {
                const string query = "SELECT * FROM Prompts ORDER BY CreatedAt DESC";
                return await connection.QueryAsync<Prompt>(query);
            }
        }

        public async Task<Prompt?> GetByIdAsync(int id)
        {
            using (var connection = _databaseConnection.GetConnection())
            {
                const string query = "SELECT * FROM Prompts WHERE Id = @Id";
                return await connection.QueryFirstOrDefaultAsync<Prompt>(query, new { Id = id });
            }
        }

        public async Task<IEnumerable<Prompt>> GetByNameAsync(string name)
        {
            using (var connection = _databaseConnection.GetConnection())
            {
                const string query = "SELECT * FROM Prompts WHERE Name LIKE @Name ORDER BY CreatedAt DESC";
                return await connection.QueryAsync<Prompt>(query, new { Name = $"%{name}%" });
            }
        }

        public async Task<int> CreateAsync(Prompt prompt)
        {
            using (var connection = _databaseConnection.GetConnection())
            {
                const string query = @"
                    INSERT INTO Prompts (Name, Description, Content, Version, Status, CreatedAt, UpdatedAt, CreatedBy, UpdatedBy, Tags, ModelType)
                    VALUES (@Name, @Description, @Content, @Version, @Status, @CreatedAt, @UpdatedAt, @CreatedBy, @UpdatedBy, @Tags, @ModelType);
                    SELECT last_insert_rowid();";

                var id = await connection.ExecuteScalarAsync<int>(query, prompt);
                return id;
            }
        }

        public async Task<bool> UpdateAsync(Prompt prompt)
        {
            using (var connection = _databaseConnection.GetConnection())
            {
                prompt.UpdatedAt = DateTime.UtcNow;
                const string query = @"
                    UPDATE Prompts 
                    SET Name = @Name, 
                        Description = @Description, 
                        Content = @Content, 
                        Version = @Version, 
                        Status = @Status, 
                        UpdatedAt = @UpdatedAt, 
                        UpdatedBy = @UpdatedBy, 
                        Tags = @Tags, 
                        ModelType = @ModelType
                    WHERE Id = @Id";

                var rowsAffected = await connection.ExecuteAsync(query, prompt);
                return rowsAffected > 0;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using (var connection = _databaseConnection.GetConnection())
            {
                const string query = "DELETE FROM Prompts WHERE Id = @Id";
                var rowsAffected = await connection.ExecuteAsync(query, new { Id = id });
                return rowsAffected > 0;
            }
        }

        public async Task<IEnumerable<Prompt>> GetByStatusAsync(string status)
        {
            using (var connection = _databaseConnection.GetConnection())
            {
                const string query = "SELECT * FROM Prompts WHERE Status = @Status ORDER BY CreatedAt DESC";
                return await connection.QueryAsync<Prompt>(query, new { Status = status });
            }
        }

        public async Task<IEnumerable<Prompt>> GetByModelTypeAsync(string modelType)
        {
            using (var connection = _databaseConnection.GetConnection())
            {
                const string query = "SELECT * FROM Prompts WHERE ModelType = @ModelType ORDER BY CreatedAt DESC";
                return await connection.QueryAsync<Prompt>(query, new { ModelType = modelType });
            }
        }

        public async Task<int> GetCountAsync()
        {
            using (var connection = _databaseConnection.GetConnection())
            {
                const string query = "SELECT COUNT(*) FROM Prompts";
                return await connection.ExecuteScalarAsync<int>(query);
            }
        }
    }
}
