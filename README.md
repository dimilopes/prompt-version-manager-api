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
│   ├── DatabaseConnection.cs  # Gerenciador de conexão SQLite
│   ├── IPromptRepository.cs   # Interface do repositório
│   └── PromptRepository.cs    # Implementação do repositório com Dapper
├── Services/              # Camada de lógica de negócio
│   ├── IPromptService.cs      # Interface do serviço
│   └── PromptService.cs       # Implementação do serviço
├── Controllers/           # Controladores da API
│   └── PromptsController.cs   # Controller CRUD de Prompts
├── Validators/            # Validadores de dados
│   └── PromptValidator.cs     # Validador de Prompts
├── Exceptions/            # Exceções customizadas
│   └── ApiException.cs        # Exceções da API
├── Middleware/            # Middlewares da aplicação
│   └── ExceptionHandlingMiddleware.cs  # Tratamento global de exceções
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

### ✅ Fase 2: Implementação do Core (Branch: `feature/implementacao-core`)

**Status**: Concluída

**Implementações**:
- ✅ Criação da interface `IPromptRepository` para abstração de dados
- ✅ Implementação de `PromptRepository` com Dapper para operações CRUD
- ✅ Criação da interface `IPromptService` para lógica de negócio
- ✅ Implementação de `PromptService` com validações e logging
- ✅ Criação de `PromptsController` com endpoints REST
- ✅ Registro de serviços na injeção de dependência
- ✅ Compilação bem-sucedida do projeto

**Endpoints Implementados**:

| Método | Rota | Descrição |
|--------|------|----------|
| GET | `/api/prompts` | Obtém todos os prompts |
| GET | `/api/prompts/{id}` | Obtém um prompt pelo ID |
| GET | `/api/prompts/search/by-name?name=...` | Busca prompts por nome |
| GET | `/api/prompts/filter/by-status?status=...` | Filtra prompts por status |
| GET | `/api/prompts/filter/by-model?modelType=...` | Filtra prompts por modelo de IA |
| GET | `/api/prompts/stats/count` | Obtém contagem total de prompts |
| POST | `/api/prompts` | Cria um novo prompt |
| PUT | `/api/prompts/{id}` | Atualiza um prompt existente |
| DELETE | `/api/prompts/{id}` | Deleta um prompt |
| POST | `/api/prompts/{id}/increment-version` | Incrementa a versão de um prompt |

**Padrões Implementados**:
- Repository Pattern para abstração de dados
- Service Layer para lógica de negócio
- Dependency Injection para inversão de controle
- Logging estruturado em todas as operações
- Tratamento de exceções com códigos HTTP apropriados

### ✅ Fase 3: Validações e Melhorias (Branch: `feature/validacoes-melhorias`)

**Status**: Concluída

**Implementações**:
- ✅ Criação de `PromptValidator` para validações de negócio
- ✅ Implementação de exceções customizadas (`ApiException`, `ValidationException`, `ResourceNotFoundException`, etc.)
- ✅ Criação de middleware `ExceptionHandlingMiddleware` para tratamento global de exceções
- ✅ Atualização de `PromptService` com validações robustas
- ✅ Resposta de erro padronizada com `ErrorResponse`
- ✅ Logging estruturado em todas as operações
- ✅ Compilação bem-sucedida do projeto

## Validações Implementadas

**Regras de Validação por Campo**:

| Campo | Validações |
|-------|-----------|
| Nome | Mínimo 3 caracteres, máximo 255 caracteres, obrigatório |
| Conteúdo | Mínimo 10 caracteres, máximo 10.000 caracteres, obrigatório |
| Descrição | Máximo 1.000 caracteres, opcional |
| Tags | Máximo 500 caracteres, máximo 20 tags, opcional |
| Status | Valores válidos: Ativo, Inativo, Arquivado, Revisão |
| ID | Deve ser maior que zero |

## Tratamento de Exceções

A API implementa um sistema robusto de tratamento de exceções com middleware global que padroniza todas as respostas de erro.

**Exceções Implementadas**:

| Exceção | Código HTTP | Descrição |
|---------|------------|----------|
| ValidationException | 400 | Erros de validação de dados |
| ResourceNotFoundException | 404 | Recurso não encontrado |
| UnauthorizedException | 401 | Operação não autorizada |
| ConflictException | 409 | Conflito de dados |
| DatabaseException | 500 | Erro ao acessar banco de dados |

**Exemplo de Resposta de Erro**:

```json
{
  "statusCode": 400,
  "message": "Erro na validação do prompt",
  "details": null,
  "timestamp": "2025-11-12T10:30:00Z",
  "errors": [
    "Nome deve ter no mínimo 3 caracteres",
    "Conteúdo é obrigatório"
  ]
}
```

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

## Exemplos de Uso

### Criar um novo Prompt

```bash
curl -X POST https://localhost:7000/api/prompts \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Gerador de Código Python",
    "description": "Prompt para gerar código Python otimizado",
    "content": "Gere código Python limpo e eficiente para a seguinte tarefa: ...",
    "status": "Ativo",
    "createdBy": "usuario@example.com",
    "tags": "python,codigo,ia",
    "modelType": "GPT-4"
  }'
```

### Obter todos os Prompts

```bash
curl https://localhost:7000/api/prompts
```

### Buscar Prompt por ID

```bash
curl https://localhost:7000/api/prompts/1
```

### Atualizar um Prompt

```bash
curl -X PUT https://localhost:7000/api/prompts/1 \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Gerador de Código Python v2",
    "description": "Prompt atualizado para gerar código Python",
    "content": "Novo conteúdo do prompt...",
    "status": "Ativo",
    "updatedBy": "usuario@example.com"
  }'
```

### Deletar um Prompt

```bash
curl -X DELETE https://localhost:7000/api/prompts/1
```

### Incrementar Versão

```bash
curl -X POST https://localhost:7000/api/prompts/1/increment-version
```

## Próximos Passos

1. Implementar testes unitários com xUnit
2. Adicionar autenticação e autorização (JWT)
3. Implementar paginação e filtros avançados
4. Adicionar cache distribuído (Redis)
5. Implementar rate limiting
6. Adicionar auditoria de mudanças
7. Implementar versionamento de API

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
