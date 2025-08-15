using INSURANCES.CORE.Dtos;
using INSURANCES.CORE.Ports.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace INSURANCES.HIRING.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HiringController(ILogger<HiringController> logger, IHiringService hiringService) : ControllerBase
    {
        private readonly ILogger<HiringController> _logger = logger;
        private readonly IHiringService _hiringService = hiringService;

        [HttpGet("{id}")]
        public async Task<IActionResult> GetHiringByIdAsync([FromRoute] Guid id)
        {
            try
            {
                return Ok(await _hiringService.GetHiringByIdAsync(id));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[HiringController][GetHiringByIdAsync] - Erro: {Message}, Id: {id}",
                    ex.Message,
                    JsonConvert.SerializeObject(id));
                throw;
            }
        }

        [HttpPost("add")]
        public async Task<IActionResult> PostCreateHiringAsync([FromBody] PostHiringDto postHiringDto)
        {
            try
            {
                return Ok(await _hiringService.PostCreateHiringAsync(postHiringDto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[HiringController][PostCreateHiringAsync] - Erro: {Message}, Dto: {dto}",
                    ex.Message,
                    JsonConvert.SerializeObject(postHiringDto));
                throw;
            }
        }
    }
}
