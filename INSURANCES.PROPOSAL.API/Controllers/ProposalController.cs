using INSURANCES.CORE.Dtos;
using INSURANCES.CORE.Ports.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace INSURANCES.PROPOSAL.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProposalController(ILogger<ProposalController> logger, IProposalService proposalService) : ControllerBase
    {
        private readonly ILogger<ProposalController> _logger = logger;
        private readonly IProposalService _proposalService = proposalService;

        [HttpGet("list")]
        public async Task<IActionResult> GetProposalListAsync([FromQuery] GetProposalListDto proposalListDto)
        {
            try
            {
                return Ok(await _proposalService.GetProposalListAsync(proposalListDto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[ProposalController][GetProposalListAsync] - Erro: {Message}, Dto: {dto}",
                    ex.Message,
                    JsonConvert.SerializeObject(proposalListDto));
                throw;
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProposalByIdAsync([FromRoute] Guid id)
        {
            try
            {
                return Ok(await _proposalService.GetProposalByIdAsync(id));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[ProposalController][GetProposalByIdAsync] - Erro: {Message}, Id: {id}",
                    ex.Message,
                    JsonConvert.SerializeObject(id));
                throw;
            }
        }

        [HttpPost("add")]
        public async Task<IActionResult> PostProposalByIdAsync([FromBody] PostProposalDto postProposalDto)
        {
            try
            {
                return Ok(await _proposalService.PostProposalByIdAsync(postProposalDto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[ProposalController][PostProposalByIdAsync] - Erro: {Message}, Dto: {dto}",
                    ex.Message,
                    JsonConvert.SerializeObject(postProposalDto));
                throw;
            }
        }

        [HttpPut("update/status")]
        public async Task<IActionResult> PutProposalUpdateStatusByIdAsync([FromBody] PutProposalUpdateStatusByIdDto putProposalAlterStatusDto)
        {
            try
            {
                return Ok(await _proposalService.PutProposalUpdateStatusByIdAsync(putProposalAlterStatusDto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[ProposalController][PutProposalUpdateStatusByIdAsync] - Erro: {Message}, Dto: {dto}",
                    ex.Message,
                    JsonConvert.SerializeObject(putProposalAlterStatusDto));
                throw;
            }
        }
    }
}