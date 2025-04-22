using AutoGenerator.Helper;
using AutoGenerator.Helper.Translation;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using V1.DyModels.Dso.Requests;
using V1.DyModels.Dso.Responses;
using V1.DyModels.VMs;
using V1.Services.Services;

namespace V1.Controllers.Api
{
    //[ApiExplorerSettings(GroupName = "V1")]
    [Route("api/V1/Api/[controller]")]
    [ApiController]
    public class DialectController : ControllerBase
    {
        private readonly IUseDialectService _dialectService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        public DialectController(IUseDialectService dialectService, IMapper mapper, ILoggerFactory logger)
        {
            _dialectService = dialectService;
            _mapper = mapper;
            _logger = logger.CreateLogger(typeof(DialectController).FullName);
        }

        // Get all Dialects.
        [HttpGet(Name = "GetDialects")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<DialectOutputVM>>> GetAll()
        {
            try
            {
                _logger.LogInformation("Fetching all Dialects...");
                var result = await _dialectService.GetAllAsync();
                var items = _mapper.Map<List<DialectOutputVM>>(result);
                return Ok(items);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching all Dialects");
                return StatusCode(500, "Internal Server Error");
            }
        }

        // Get a Dialect by ID.
        [HttpGet("{id}", Name = "GetDialect")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<DialectOutputVM>> GetById(string id, string lg = "en")
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                _logger.LogWarning("Invalid Dialect ID received.");
                return BadRequest("Invalid Dialect ID.");
            }

            try
            {
                _logger.LogInformation("Fetching Dialect with ID: {id}", id);
                var entity = await _dialectService.GetByIdAsync(id);
                if (entity == null)
                {
                    _logger.LogWarning("Dialect not found with ID: {id}", id);
                    return NotFound();
                }

                var item = _mapper.Map<DialectOutputVM>(entity, opt => opt.Items.Add(HelperTranslation.KEYLG, lg));
                return Ok(item);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching Dialect with ID: {id}", id);
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("GetDialectByLanguage/{langId}", Name = "GetDialectByLanguage")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<DialectOutputVM>> GetDialectByLanguageId(string langId, string lg = "en")
        {
            if (string.IsNullOrWhiteSpace(langId))
            {
                _logger.LogWarning("InvallangId Dialect language id received.");
                return BadRequest("InvallangId Dialect language id.");
            }

            try
            {
                _logger.LogInformation("Fetching Dialect with language id: {langId}", langId);
                var entity = await _dialectService.GetOneByAsync([new FilterCondition(nameof(DialectResponseDso.LanguageId), langId)]);
                if (entity == null)
                {
                    _logger.LogWarning("Dialect not found with language id: {langId}", langId);
                    return NotFound();
                }

                var item = _mapper.Map<DialectOutputVM>(entity, opt => opt.Items.Add(HelperTranslation.KEYLG, lg));
                return Ok(item);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching Dialect with language id: {langId}", langId);
                return StatusCode(500, "Internal Server Error");
            }
        }


        [HttpGet("GetDialectsByLanguage/{langId}", Name = "GetDialectsByLanguage")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<DialectOutputVM>>> GetDialectsByLanguage(string langId, string lg = "en")
        {
            if (string.IsNullOrWhiteSpace(langId))
            {
                _logger.LogWarning("InvallangId Dialects language id received.");
                return BadRequest("InvallangId Dialects language id.");
            }

            try
            {
                _logger.LogInformation("Fetching Dialects with language id: {langId}", langId);
                var entity = await _dialectService.GetAllByAsync([new FilterCondition(nameof(DialectResponseDso.LanguageId), langId)]);
                if (entity.TotalRecords == 0)
                {
                    _logger.LogWarning("Dialects not found with language id: {langId}", langId);
                    return NotFound();
                }

                var item = _mapper.Map<List<DialectOutputVM>>(entity.Data, opt => opt.Items.Add(HelperTranslation.KEYLG, lg));
                return Ok(item);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching Dialects with language id: {langId}", langId);
                return StatusCode(500, "Internal Server Error");
            }
        }

        // // Get a Dialect by Lg.
        [HttpGet("GetDialectByLanguage", Name = "GetDialectByLg")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<DialectOutputVM>> GetDialectByLg(DialectFilterVM model)
        {
            var id = model.Id;
            if (string.IsNullOrWhiteSpace(id))
            {
                _logger.LogWarning("Invalid Dialect ID received.");
                return BadRequest("Invalid Dialect ID.");
            }

            try
            {
                _logger.LogInformation("Fetching Dialect with ID: {id}", id);
                var entity = await _dialectService.GetByIdAsync(id);
                if (entity == null)
                {
                    _logger.LogWarning("Dialect not found with ID: {id}", id);
                    return NotFound();
                }

                var item = _mapper.Map<DialectOutputVM>(entity, opt => opt.Items.Add(HelperTranslation.KEYLG, model.Lg));
                return Ok(item);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching Dialect with ID: {id}", id);
                return StatusCode(500, "Internal Server Error");
            }
        }

        // // Get a Dialects by Lg.
        [HttpGet("GetDialectsByLanguage", Name = "GetDialectsByLg")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<DialectOutputVM>>> GetDialectsByLg(string? lg)
        {
            if (string.IsNullOrWhiteSpace(lg))
            {
                _logger.LogWarning("Invalid Dialect lg received.");
                return BadRequest("Invalid Dialect lg null ");
            }

            try
            {
                var dialects = await _dialectService.GetAllAsync();
                if (dialects == null)
                {
                    _logger.LogWarning("Dialects not found  by  ");
                    return NotFound();
                }

                var items = _mapper.Map<IEnumerable<DialectOutputVM>>(dialects, opt => opt.Items.Add(HelperTranslation.KEYLG, lg));
                return Ok(items);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching Dialects with Lg: {lg}", lg);
                return StatusCode(500, "Internal Server Error");
            }
        }

        // Create a new Dialect.
        [HttpPost(Name = "CreateDialect")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<DialectOutputVM>> Create([FromBody] DialectCreateVM model)
        {
            if (model == null)
            {
                _logger.LogWarning("Dialect data is null in Create.");
                return BadRequest("Dialect data is required.");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state in Create: {ModelState}", ModelState);
                return BadRequest(ModelState);
            }

            try
            {
                _logger.LogInformation("Creating new Dialect with data: {@model}", model);
                var item = _mapper.Map<DialectRequestDso>(model);
                var createdEntity = await _dialectService.CreateAsync(item);
                var createdItem = _mapper.Map<DialectOutputVM>(createdEntity);
                return Ok(createdItem);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating a new Dialect");
                return StatusCode(500, "Internal Server Error");
            }
        }

        // Create multiple Dialects.
        [HttpPost("createRange")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<DialectOutputVM>>> CreateRange([FromBody] IEnumerable<DialectCreateVM> models)
        {
            if (models == null)
            {
                _logger.LogWarning("Data is null in CreateRange.");
                return BadRequest("Data is required.");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state in CreateRange: {ModelState}", ModelState);
                return BadRequest(ModelState);
            }

            try
            {
                _logger.LogInformation("Creating multiple Dialects.");
                var items = _mapper.Map<List<DialectRequestDso>>(models);
                var createdEntities = await _dialectService.CreateRangeAsync(items);
                var createdItems = _mapper.Map<List<DialectOutputVM>>(createdEntities);
                return Ok(createdItems);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating multiple Dialects");
                return StatusCode(500, "Internal Server Error");
            }
        }

        // Update an existing Dialect.
        [HttpPut(Name = "UpdateDialect")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<DialectOutputVM>> Update([FromBody] DialectUpdateVM model)
        {
            if (model == null)
            {
                _logger.LogWarning("Invalid data in Update.");
                return BadRequest("Invalid data.");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state in Update: {ModelState}", ModelState);
                return BadRequest(ModelState);
            }

            try
            {
                _logger.LogInformation("Updating Dialect with ID: {id}", model?.Id);
                var item = _mapper.Map<DialectRequestDso>(model);
                var updatedEntity = await _dialectService.UpdateAsync(item);
                if (updatedEntity == null)
                {
                    _logger.LogWarning("Dialect not found for update with ID: {id}", model?.Id);
                    return NotFound();
                }

                var updatedItem = _mapper.Map<DialectOutputVM>(updatedEntity);
                return Ok(updatedItem);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating Dialect with ID: {id}", model?.Id);
                return StatusCode(500, "Internal Server Error");
            }
        }

        // Delete a Dialect.
        [HttpDelete("{id}", Name = "DeleteDialect")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(string? id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                _logger.LogWarning("Invalid Dialect ID received in Delete.");
                return BadRequest("Invalid Dialect ID.");
            }

            try
            {
                _logger.LogInformation("Deleting Dialect with ID: {id}", id);
                await _dialectService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting Dialect with ID: {id}", id);
                return StatusCode(500, "Internal Server Error");
            }
        }

        // Get count of Dialects.
        [HttpGet("CountDialect")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<int>> Count()
        {
            try
            {
                _logger.LogInformation("Counting Dialects...");
                var count = await _dialectService.CountAsync();
                return Ok(count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while counting Dialects");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}