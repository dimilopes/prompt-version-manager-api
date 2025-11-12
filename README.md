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

## Fases de Desenvolvimento

### ✅ Fase 1: Modelagem do Domínio (Branch: `feature/modelagem-dominio`)

**Implementações**:
- Criação da classe `Prompt` com 12 propriedades para rastreamento de versões
- Implementação da classe `DatabaseConnection` para gerenciar conexões SQLite
- Configuração do banco de dados com tabela `Prompts` criada automaticamente
- Integração com injeção de dependência do ASP.NET Core
- Configuração de CORS para permitir requisições de diferentes origens

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

### ✅ Fase 2: Implementação do Core (Branch: `feature/implementacao-core`)

**Implementações**:
- **Repository Pattern**: Interface `IPromptRepository` e implementação `PromptRepository` com Dapper
- **Service Layer**: Interface `IPromptService` e implementação `PromptService` com lógica de negócio
- **Controller**: `PromptsController` com 10 endpoints REST completos
- Injeção de dependência completa
- Documentação Swagger/OpenAPI integrada
- Logging estruturado em todas as operações

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

### ✅ Fase 3: Validações e Melhorias (Branch: `feature/validacoes-melhorias`)

**Implementações**:
- Criação de `PromptValidator` para validações de negócio
- Implementação de exceções customizadas
- Criação de middleware para tratamento global de exceções
- Atualização de `PromptService` com validações robustas
- Resposta de erro padronizada
- Logging estruturado em todas as operações

---

## 🛡️ Tratamento de Exceções

A API implementa um sistema robusto e centralizado de tratamento de exceções através de um middleware global que padroniza todas as respostas de erro.

### Exceções Customizadas Implementadas

A aplicação define as seguintes exceções customizadas, todas herdando de `ApiException`:

#### 1. **ValidationException** (Código HTTP: 400)
Lançada quando há erros de validação de dados de entrada.

```csharp
throw new ValidationException("Erro na validação do prompt", 
    new List<string> { "Nome deve ter no mínimo 3 caracteres" });
```

#### 2. **ResourceNotFoundException** (Código HTTP: 404)
Lançada quando um recurso solicitado não é encontrado no banco de dados.

```csharp
throw new ResourceNotFoundException($"Prompt com ID {id} não encontrado");
```

#### 3. **UnauthorizedException** (Código HTTP: 401)
Lançada quando uma operação não é autorizada.

```csharp
throw new UnauthorizedException("Acesso negado");
```

#### 4. **ConflictException** (Código HTTP: 409)
Lançada quando há um conflito nos dados (ex: falha ao atualizar).

```csharp
throw new ConflictException("Falha ao atualizar o prompt");
```

#### 5. **DatabaseException** (Código HTTP: 500)
Lançada quando há erros ao acessar o banco de dados.

```csharp
throw new DatabaseException("Erro ao obter prompts", ex);
```

### Middleware de Tratamento Global

O middleware `ExceptionHandlingMiddleware` intercepta todas as exceções não tratadas e as converte em respostas HTTP padronizadas.

**Localização**: `Middleware/ExceptionHandlingMiddleware.cs`

**Funcionalidades**:
- Captura todas as exceções não tratadas
- Mapeia exceções customizadas para códigos HTTP apropriados
- Retorna respostas padronizadas em JSON
- Registra erros para auditoria e debugging

### Formato Padronizado de Resposta de Erro

Todas as respostas de erro seguem o padrão abaixo:

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

**Campos da Resposta**:
- `statusCode`: Código HTTP da resposta
- `message`: Mensagem de erro principal
- `details`: Detalhes adicionais (opcional)
- `timestamp`: Data e hora do erro em UTC
- `errors`: Lista de erros específicos (apenas para ValidationException)

### Exemplos de Tratamento de Exceções

#### Exemplo 1: Validação de Entrada
```csharp
public async Task<Prompt> CreatePromptAsync(Prompt prompt)
{
    if (prompt == null)
    {
        throw new ValidationException("Dados do prompt são obrigatórios", 
            new List<string> { "Prompt não pode ser nulo" });
    }

    var validationResult = _validator.ValidateForCreation(prompt);
    if (!validationResult.IsValid)
    {
        throw new ValidationException("Erro na validação do prompt", 
            validationResult.Errors);
    }
    // ... resto da implementação
}
```

#### Exemplo 2: Recurso Não Encontrado
```csharp
public async Task<Prompt> GetPromptByIdAsync(int id)
{
    var prompt = await _promptRepository.GetByIdAsync(id);
    
    if (prompt == null)
    {
        throw new ResourceNotFoundException($"Prompt com ID {id} não encontrado");
    }
    
    return prompt;
}
```

#### Exemplo 3: Erro de Banco de Dados
```csharp
try
{
    return await _promptRepository.GetAllAsync();
}
catch (Exception ex)
{
    _logger.LogError(ex, "Erro ao obter todos os prompts");
    throw new DatabaseException("Erro ao obter prompts do banco de dados", ex);
}
```

### Validações Implementadas

O `PromptValidator` implementa as seguintes regras de validação:

| Campo | Validações |
|-------|-----------|
| Nome | Mínimo 3 caracteres, máximo 255 caracteres, obrigatório |
| Conteúdo | Mínimo 10 caracteres, máximo 10.000 caracteres, obrigatório |
| Descrição | Máximo 1.000 caracteres, opcional |
| Tags | Máximo 500 caracteres, máximo 20 tags, opcional |
| Status | Valores válidos: Ativo, Inativo, Arquivado, Revisão |
| ID | Deve ser maior que zero |

---

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

---

## Padrões de Arquitetura Implementados

- **Repository Pattern**: Abstração da camada de dados
- **Service Layer**: Lógica de negócio centralizada
- **Dependency Injection**: Inversão de controle
- **Middleware Pattern**: Tratamento centralizado de exceções
- **Validator Pattern**: Validações de negócio reutilizáveis

---

## Contribuindo

Para contribuir com o projeto, siga o fluxo de branches:

1. Crie uma branch a partir de `develop`
2. Faça suas alterações
3. Envie um pull request com uma descrição clara
