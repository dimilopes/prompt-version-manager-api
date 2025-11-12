# Prompt Version Manager API

Sistema de API para versionar, rastrear e gerenciar diferentes versões de prompts usados no treinamento de modelos de IA.

## Visão Geral

O **Prompt Version Manager API** é uma solução robusta construída em **ASP.NET Core 8.0** que permite que empresas de IA gerenciem, versionem e rastreiem prompts utilizados em seus modelos de treinamento. A API utiliza **Dapper** como ORM para acesso eficiente aos dados armazenados em **SQLite**.

## Tecnologias Utilizadas

- **Framework**: ASP.NET Core 8.0
- **Banco de Dados**: SQLite 3
- **ORM**: Dapper 2.1.66
- **Linguagem**: C# 12.0
- **Documentação API**: Swagger/OpenAPI

## Estrutura do Projeto

```
PromptVersionManager/
├── Models/                 # Classes de domínio
│   └── Prompt.cs          # Entidade Prompt
├── Data/                  # Camada de acesso a dados
│   └── DatabaseConnection.cs  # Gerenciador de conexão SQLite
├── Services/              # Camada de lógica de negócio (em desenvolvimento)
├── Controllers/           # Controladores da API (em desenvolvimento)
├── Program.cs            # Configuração da aplicação
├── appsettings.json      # Configurações da aplicação
└── PromptVersionManager.csproj  # Arquivo de projeto
```

## Fases de Desenvolvimento

### ✅ Fase 1: Modelagem do Domínio (Branch: `feature/modelagem-dominio`)

**Status**: Concluída

**Implementações**:
- ✅ Criação da classe `Prompt` com todas as propriedades necessárias
- ✅ Implementação da classe `DatabaseConnection` para gerenciar conexões SQLite
- ✅ Configuração do banco de dados com inicialização automática de tabelas
- ✅ Integração com injeção de dependência do ASP.NET Core
- ✅ Configuração de CORS para permitir requisições de diferentes origens
- ✅ Compilação bem-sucedida do projeto

**Propriedades da Entidade Prompt**:

| Propriedade | Tipo | Descrição |
|-------------|------|-----------|
| Id | int | Identificador único do Prompt |
| Name | string | Nome descritivo do Prompt |
| Description | string | Descrição detalhada do Prompt |
| Content | string | Conteúdo do Prompt |
| Version | int | Versão do Prompt |
| Status | string | Status (Ativo, Inativo, Arquivado) |
| CreatedAt | DateTime | Data de criação |
| UpdatedAt | DateTime | Data da última atualização |
| CreatedBy | string | Usuário que criou |
| UpdatedBy | string | Usuário que atualizou |
| Tags | string | Tags para categorização |
| ModelType | string | Modelo de IA associado |

**Banco de Dados**:
- Arquivo: `promptmanager.db`
- Tabela: `Prompts`
- Inicialização automática ao iniciar a aplicação

### ⏳ Fase 2: Implementação do Core (Branch: `feature/implementacao-core`)

**Status**: Pendente

**Implementações Planejadas**:
- Criar a controller `PromptsController`
- Implementar endpoints CRUD (Create, Read, Update, Delete)
- Criar a camada `PromptService` com lógica de negócio
- Registrar serviços na injeção de dependência
- Implementar validações básicas

### ⏳ Fase 3: Validações e Melhorias (Branch: `feature/validacoes-melhorias`)

**Status**: Pendente

**Implementações Planejadas**:
- Tratamento robusto de exceções
- Validações de entrada de dados
- Logging estruturado
- Documentação completa do README

## Como Executar

### Pré-requisitos

- .NET 8.0 SDK ou superior
- SQLite 3 (incluído no pacote System.Data.SQLite)

### Instalação e Execução

1. Clone o repositório:
```bash
git clone https://github.com/dimilopes/prompt-version-manager-api.git
cd prompt-version-manager-api/PromptVersionManager
```

2. Restaure as dependências:
```bash
dotnet restore
```

3. Compile o projeto:
```bash
dotnet build
```

4. Execute a aplicação:
```bash
dotnet run
```

A API estará disponível em `https://localhost:7000` (HTTPS) ou `http://localhost:5000` (HTTP).

### Acessar a Documentação Swagger

Após iniciar a aplicação, acesse a documentação interativa em:
```
https://localhost:7000/swagger
```

## Configuração do Banco de Dados

A conexão com o banco de dados SQLite é configurada em `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=promptmanager.db;Version=3;"
  }
}
```

O banco de dados é inicializado automaticamente ao iniciar a aplicação, criando a tabela `Prompts` se ela não existir.

## Teste de Conexão

Para verificar se a conexão com o banco de dados está funcionando, a classe `DatabaseConnection` fornece o método `TestConnection()`:

```csharp
var db = new DatabaseConnection(connectionString);
bool isConnected = db.TestConnection();
```

## Próximos Passos

1. Implementar a camada de serviços (`PromptService`)
2. Criar a controller com endpoints CRUD
3. Adicionar validações e tratamento de exceções
4. Implementar testes unitários
5. Adicionar autenticação e autorização
6. Implementar paginação e filtros avançados

## Contribuindo

Para contribuir com o projeto, siga o fluxo de branches:

1. Crie uma branch a partir de `develop`
2. Faça suas alterações
3. Envie um pull request com uma descrição clara

## Licença

Este projeto está sob a licença MIT.

## Autor

Desenvolvido por **Manus AI** em colaboração com **dimilopes**.

---

**Última atualização**: 12 de Novembro de 2025
