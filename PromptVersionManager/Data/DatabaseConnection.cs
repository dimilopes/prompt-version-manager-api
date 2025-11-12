using System.Data;
using System.Data.SQLite;

namespace PromptVersionManager.Data
{
    /// <summary>
    /// Classe responsável por gerenciar a conexão com o banco de dados SQLite
    /// </summary>
    public class DatabaseConnection
    {
        private readonly string _connectionString;

        public DatabaseConnection(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Obtém uma nova conexão com o banco de dados
        /// </summary>
        /// <returns>Conexão SQLite aberta</returns>
        public IDbConnection GetConnection()
        {
            var connection = new SQLiteConnection(_connectionString);
            connection.Open();
            return connection;
        }

        /// <summary>
        /// Inicializa o banco de dados criando as tabelas necessárias
        /// </summary>
        public void InitializeDatabase()
        {
            using (var connection = GetConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"
                        CREATE TABLE IF NOT EXISTS Prompts (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            Name TEXT NOT NULL,
                            Description TEXT,
                            Content TEXT NOT NULL,
                            Version INTEGER NOT NULL DEFAULT 1,
                            Status TEXT NOT NULL DEFAULT 'Ativo',
                            CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
                            UpdatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
                            CreatedBy TEXT,
                            UpdatedBy TEXT,
                            Tags TEXT,
                            ModelType TEXT
                        );
                    ";
                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Verifica se a conexão com o banco de dados está funcional
        /// </summary>
        /// <returns>True se a conexão é válida, False caso contrário</returns>
        public bool TestConnection()
        {
            try
            {
                using (var connection = GetConnection())
                {
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "SELECT 1";
                        command.ExecuteScalar();
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
