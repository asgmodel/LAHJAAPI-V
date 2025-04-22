using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using V1.DyModels.Dso.Requests;
using V1.DyModels.VMs;
using V1.Services.Services;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class MasterController
        : ControllerBase
    {
        private readonly ILogger<MasterController> _logger;
        private readonly IMapper _mapper;
        private readonly IUseLanguageService _languageService;
        private readonly IUseCategoryModelService _categoryModelService;
        private readonly IUseTypeModelService _typeModelService;

        public MasterController(
            ILogger<MasterController> logger,
            IMapper mapper,
            IUseLanguageService useLanguageService,
            IUseCategoryModelService categoryModelService,
            IUseTypeModelService typeModelService)
        {
            _logger = logger;
            _mapper = mapper;
            _languageService = useLanguageService;
            _categoryModelService = categoryModelService;
            _typeModelService = typeModelService;
        }

        #region Language
        [HttpGet("GetLanguages")]
        public async Task<ActionResult<List<LanguageOutputVM>>> GetLanguages(string lg = "en")
        {
            return RedirectToRoute("GetLanguages", lg);
        }

        [HttpGet("GetLanguageByCode/{code}")]
        public async Task<ActionResult<List<LanguageOutputVM>>> GetLanguageByCode(string code, string lg = "en")
        {
            return RedirectToRoute("GetLanguageByCode", new { code, lg });
        }

        [HttpPost("CreateLanguage")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CreateLanguage([FromBody] LanguageCreateVM model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state in Create: {ModelState}", ModelState);
                return BadRequest(ModelState);
            }

            try
            {
                _logger.LogInformation("Creating new Language with data: {@model}", model);
                var item = _mapper.Map<LanguageRequestDso>(model);
                var createdEntity = await _languageService.CreateAsync(item);
                var createdItem = _mapper.Map<LanguageOutputVM>(createdEntity);
                return CreatedAtAction(nameof(GetLanguageByCode), new { code = createdItem.Code }, createdItem);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating a new Language");
                return StatusCode(500, "Internal Server Error");
            }
        }
        #endregion

        #region Category
        [HttpGet("GetCategoryModelByName/{name}")]
        public IActionResult GetCategoryModelByName(string name, string lg = "en")
        {
            return RedirectToRoute("GetCategoryModelByName", new { name, lg });

        }

        [HttpPost("CreateCategoryModel")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryModelCreateVM model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state in Create: {ModelState}", ModelState);
                return BadRequest(ModelState);
            }

            try
            {
                _logger.LogInformation("Creating new CategoryModel with data: {@model}", model);
                var item = _mapper.Map<CategoryModelRequestDso>(model);
                var createdEntity = await _categoryModelService.CreateAsync(item);
                var createdItem = _mapper.Map<CategoryModelOutputVM>(createdEntity);
                return CreatedAtAction(nameof(GetCategoryModelByName), new { name = createdItem.Name }, createdItem);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating a new CategoryModel");
                return StatusCode(500, "Internal Server Error");
            }
        }
        #endregion

        #region Type
        [HttpGet("GetTypeByName/{name}")]
        public IActionResult GetTypeByName(string name, string lg = "en")
        {
            return RedirectToRoute("GetTypeModelByName", new { name, lg });
        }

        [HttpGet("types/active")]
        public IActionResult GetActiveTypes(string lg = "en")
        {
            return RedirectToRoute("GetActiveTypes", lg);
        }

        [HttpPost("CreateTypes")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<TypeModelOutputVM>>> CreateType([FromBody] TypeModelCreateVM model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state in Create: {ModelState}", ModelState);
                return BadRequest(ModelState);
            }

            try
            {
                _logger.LogInformation("Creating new TypeModel with data: {@model}", model);
                var item = _mapper.Map<TypeModelRequestDso>(model);
                var createdEntity = await _typeModelService.CreateAsync(item);
                var createdItem = _mapper.Map<TypeModelOutputVM>(createdEntity);
                return Ok(createdItem);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating a new TypeModel");
                return StatusCode(500, "Internal Server Error");
            }
        }
        #endregion

        #region Dialect
        [HttpGet("dialect/{languageId}")]
        public ActionResult<DialectOutputVM> GetDialectByLanguage(string languageId, string lg = "en")
        {
            return RedirectToRoute("GetDialectByLanguage", new { languageId, lg });
        }

        [HttpGet("dialects/{languageId}")]
        public async Task<ActionResult<List<DialectOutputVM>>> GetDialectsByLanguage(string languageId, string lg = "en")
        {
            return RedirectToRoute("GetDialectsByLanguage", new { languageId, lg });
        }

        //[HttpPost("dialects")]
        //public async Task<ActionResult<DialectView>> CreateDialect([FromBody] DialectCreate dialect, string lg)
        //{
        //    var createdDialect = await _dialectRepository.CreateAsync(dialect, lg);
        //    if (createdDialect == null) return BadRequest();
        //    return CreatedAtAction(nameof(GetDialectsByLanguage), new { languageId = dialect.LanguageId }, createdDialect);
        //}
        #endregion

        #region Advertisement
        [HttpGet("advertisements/{id}")]
        public ActionResult<AdvertisementOutputVM> GetActiveAdvertisementById(string id, string lg = "en")
        {
            return RedirectToRoute("GetAdvertisement", new { id, lg });
        }

        [HttpGet("GetActiveAdvertisements")]
        public async Task<ActionResult<List<AdvertisementOutputVM>>> GetActiveAdvertisements(string lg = "en")
        {
            return RedirectToRoute("GetActiveAdvertisements", lg);
        }

        //[HttpPost("advertisements")]
        //public async Task<IActionResult> CreateAdvertisement([FromBody] AdvertisementCreate advertisement, string lg = "en")
        //{
        //    var createdAdvertisement = await _advertisementRepository.CreateAsync(advertisement, lg);
        //    if (createdAdvertisement == null) return BadRequest();
        //    return CreatedAtAction(nameof(GetActiveAdvertisements), new { id = createdAdvertisement.Id }, createdAdvertisement);
        //}
        #endregion

        #region AdvertisementTab
        [HttpGet("advertisementtab/{id}")]
        public ActionResult<AdvertisementTabOutputVM> GetAdvertisementTabbyId(string id, string lg = "en")
        {
            return RedirectToRoute("GetAdvertisementTab", new { id, lg });
        }

        [HttpGet("advertisementtabs/{advertisementId}")]
        public ActionResult<List<AdvertisementTabOutputVM>> GetByAdvertisementId(string advertisementId, string lg = "en")
        {
            return RedirectToRoute("GetByAdvertisementId", new { advertisementId, lg });
        }

        //[HttpPost("advertisementtabs")]
        //public async Task<ActionResult<AdvertisementTabView>> CreateAdvertisementTab([FromBody] AdvertisementTabCreate advertisementTab, string lg = "en")
        //{
        //    var createdAdvertisementTab = await _advertisementTabRepository.CreateAsync(advertisementTab, lg);
        //    if (createdAdvertisementTab == null) return BadRequest();
        //    return CreatedAtAction(nameof(GetAdvertisementTabByAdvertisementId), new { advertisementId = createdAdvertisementTab.AdvertisementId }, createdAdvertisementTab);
        //}
        #endregion
    }
}