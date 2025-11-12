using Microsoft.AspNetCore.Mvc;
using PromptVersionManager.Models;
using PromptVersionManager.Services;

namespace PromptVersionManager.Controllers
{
    /// <summary>
    /// Controller para gerenciar operações de Prompts
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class PromptsController : ControllerBase
    {
        private readonly IPromptService _promptService;
        private readonly ILogger<PromptsController> _logger;

        public PromptsController(IPromptService promptService, ILogger<PromptsController> logger)
        {
            _promptService = promptService;
            _logger = logger;
        }

        /// <summary>
        /// Obtém todos os prompts
        /// </summary>
        /// <returns>Lista de todos os prompts</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Prompt>>> GetAllPrompts()
        {
            try
            {
                var prompts = await _promptService.GetAllPromptsAsync();
                return Ok(prompts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter todos os prompts");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Erro ao obter prompts" });
            }
        }

        /// <summary>
        /// Obtém um prompt pelo ID
        /// </summary>
        /// <param name="id">ID do prompt</param>
        /// <returns>Prompt encontrado</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Prompt>> GetPromptById(int id)
        {
            try
            {
                var prompt = await _promptService.GetPromptByIdAsync(id);
                return Ok(prompt);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, $"Prompt com ID {id} não encontrado");
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao obter prompt com ID {id}");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Erro ao obter prompt" });
            }
        }

        /// <summary>
        /// Busca prompts por nome
        /// </summary>
        /// <param name="name">Nome do prompt para buscar</param>
        /// <returns>Lista de prompts encontrados</returns>
        [HttpGet("search/by-name")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<Prompt>>> SearchByName([FromQuery] string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    return BadRequest(new { message = "Nome não pode estar vazio" });
                }

                var prompts = await _promptService.SearchPromptsByNameAsync(name);
                return Ok(prompts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao buscar prompts por nome: {name}");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Erro ao buscar prompts" });
            }
        }

        /// <summary>
        /// Obtém prompts por status
        /// </summary>
        /// <param name="status">Status dos prompts</param>
        /// <returns>Lista de prompts com o status especificado</returns>
        [HttpGet("filter/by-status")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<Prompt>>> GetByStatus([FromQuery] string status)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(status))
                {
                    return BadRequest(new { message = "Status não pode estar vazio" });
                }

                var prompts = await _promptService.GetPromptsByStatusAsync(status);
                return Ok(prompts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao obter prompts com status: {status}");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Erro ao obter prompts" });
            }
        }

        /// <summary>
        /// Obtém prompts por modelo de IA
        /// </summary>
        /// <param name="modelType">Tipo de modelo de IA</param>
        /// <returns>Lista de prompts para o modelo especificado</returns>
        [HttpGet("filter/by-model")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<Prompt>>> GetByModelType([FromQuery] string modelType)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(modelType))
                {
                    return BadRequest(new { message = "Tipo de modelo não pode estar vazio" });
                }

                var prompts = await _promptService.GetPromptsByModelTypeAsync(modelType);
                return Ok(prompts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao obter prompts para modelo: {modelType}");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Erro ao obter prompts" });
            }
        }

        /// <summary>
        /// Cria um novo prompt
        /// </summary>
        /// <param name="prompt">Dados do novo prompt</param>
        /// <returns>Prompt criado com ID atribuído</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Prompt>> CreatePrompt([FromBody] Prompt prompt)
        {
            try
            {
                if (prompt == null)
                {
                    return BadRequest(new { message = "Dados do prompt são obrigatórios" });
                }

                var createdPrompt = await _promptService.CreatePromptAsync(prompt);
                return CreatedAtAction(nameof(GetPromptById), new { id = createdPrompt.Id }, createdPrompt);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Erro de validação ao criar prompt");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar novo prompt");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Erro ao criar prompt" });
            }
        }

        /// <summary>
        /// Atualiza um prompt existente
        /// </summary>
        /// <param name="id">ID do prompt a atualizar</param>
        /// <param name="prompt">Dados atualizados do prompt</param>
        /// <returns>Prompt atualizado</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Prompt>> UpdatePrompt(int id, [FromBody] Prompt prompt)
        {
            try
            {
                if (prompt == null)
                {
                    return BadRequest(new { message = "Dados do prompt são obrigatórios" });
                }

                var updatedPrompt = await _promptService.UpdatePromptAsync(id, prompt);
                return Ok(updatedPrompt);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, $"Prompt com ID {id} não encontrado");
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Erro de validação ao atualizar prompt");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao atualizar prompt com ID {id}");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Erro ao atualizar prompt" });
            }
        }

        /// <summary>
        /// Deleta um prompt
        /// </summary>
        /// <param name="id">ID do prompt a deletar</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeletePrompt(int id)
        {
            try
            {
                await _promptService.DeletePromptAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, $"Prompt com ID {id} não encontrado");
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao deletar prompt com ID {id}");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Erro ao deletar prompt" });
            }
        }

        /// <summary>
        /// Incrementa a versão de um prompt
        /// </summary>
        /// <param name="id">ID do prompt</param>
        /// <returns>Nova versão do prompt</returns>
        [HttpPost("{id}/increment-version")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<object>> IncrementVersion(int id)
        {
            try
            {
                var newVersion = await _promptService.IncrementVersionAsync(id);
                return Ok(new { id, version = newVersion });
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, $"Prompt com ID {id} não encontrado");
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao incrementar versão do prompt {id}");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Erro ao incrementar versão" });
            }
        }

        /// <summary>
        /// Obtém a contagem total de prompts
        /// </summary>
        /// <returns>Número total de prompts</returns>
        [HttpGet("stats/count")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<object>> GetTotalCount()
        {
            try
            {
                var count = await _promptService.GetTotalCountAsync();
                return Ok(new { totalCount = count });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter contagem total de prompts");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Erro ao obter contagem" });
            }
        }
    }
}
