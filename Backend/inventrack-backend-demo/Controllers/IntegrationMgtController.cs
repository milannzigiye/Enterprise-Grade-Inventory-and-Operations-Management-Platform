using inventrack_backend_demo.DTOs;
using inventrack_backend_demo.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace inventrack_backend_demo.Controllers
{
    #region Integration Controllers

    [ApiController]
    [Route("api/[controller]")]
    public class IntegrationPartnerController : ControllerBase
    {
        private readonly IIntegrationPartnerRepository _partnerRepository;

        public IntegrationPartnerController(IIntegrationPartnerRepository partnerRepository)
        {
            _partnerRepository = partnerRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<IntegrationPartnerDto>>> GetAllPartners()
        {
            var partners = await _partnerRepository.GetAllPartnersAsync();
            return Ok(partners);
        }

        [HttpGet("{partnerId}")]
        public async Task<ActionResult<IntegrationPartnerDto>> GetPartnerById(int partnerId)
        {
            var partner = await _partnerRepository.GetPartnerByIdAsync(partnerId);
            if (partner == null)
                return NotFound();

            return Ok(partner);
        }

        [HttpPost]
        public async Task<ActionResult<IntegrationPartnerDto>> CreatePartner(IntegrationPartnerDto partnerDto)
        {
            var partner = await _partnerRepository.CreatePartnerAsync(partnerDto);
            return CreatedAtAction(nameof(GetPartnerById), new { partnerId = partner.PartnerId }, partner);
        }

        [HttpPut("{partnerId}")]
        public async Task<ActionResult<IntegrationPartnerDto>> UpdatePartner(int partnerId, IntegrationPartnerDto partnerDto)
        {
            var partner = await _partnerRepository.UpdatePartnerAsync(partnerId, partnerDto);
            if (partner == null)
                return NotFound();

            return Ok(partner);
        }

        [HttpDelete("{partnerId}")]
        public async Task<ActionResult> DeletePartner(int partnerId)
        {
            var result = await _partnerRepository.DeletePartnerAsync(partnerId);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class IntegrationConfigController : ControllerBase
    {
        private readonly IIntegrationConfigRepository _configRepository;

        public IntegrationConfigController(IIntegrationConfigRepository configRepository)
        {
            _configRepository = configRepository;
        }

        [HttpGet("partner/{partnerId}")]
        public async Task<ActionResult<List<IntegrationConfigDto>>> GetConfigsByPartner(int partnerId)
        {
            var configs = await _configRepository.GetConfigsByPartnerAsync(partnerId);
            return Ok(configs);
        }

        [HttpGet("{configId}")]
        public async Task<ActionResult<IntegrationConfigDto>> GetConfigById(int configId)
        {
            var config = await _configRepository.GetConfigByIdAsync(configId);
            if (config == null)
                return NotFound();

            return Ok(config);
        }

        [HttpPost]
        public async Task<ActionResult<IntegrationConfigDto>> CreateConfig(IntegrationConfigDto configDto)
        {
            var config = await _configRepository.CreateConfigAsync(configDto);
            return CreatedAtAction(nameof(GetConfigById), new { configId = config.ConfigId }, config);
        }

        [HttpPut("{configId}")]
        public async Task<ActionResult<IntegrationConfigDto>> UpdateConfig(int configId, IntegrationConfigDto configDto)
        {
            var config = await _configRepository.UpdateConfigAsync(configId, configDto);
            if (config == null)
                return NotFound();

            return Ok(config);
        }

        [HttpDelete("{configId}")]
        public async Task<ActionResult> DeleteConfig(int configId)
        {
            var result = await _configRepository.DeleteConfigAsync(configId);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class SyncStatusController : ControllerBase
    {
        private readonly ISyncStatusRepository _syncStatusRepository;

        public SyncStatusController(ISyncStatusRepository syncStatusRepository)
        {
            _syncStatusRepository = syncStatusRepository;
        }

        [HttpGet("partner/{partnerId}")]
        public async Task<ActionResult<List<SyncStatusDto>>> GetSyncStatusesByPartner(int partnerId)
        {
            var statuses = await _syncStatusRepository.GetSyncStatusesByPartnerAsync(partnerId);
            return Ok(statuses);
        }

        [HttpGet("partner/{partnerId}/entity/{entityType}/latest")]
        public async Task<ActionResult<SyncStatusDto>> GetLatestSyncStatus(int partnerId, string entityType)
        {
            var status = await _syncStatusRepository.GetLatestSyncStatusAsync(partnerId, entityType);
            if (status == null)
                return NotFound();

            return Ok(status);
        }

        [HttpPost]
        public async Task<ActionResult<SyncStatusDto>> CreateSyncStatus(SyncStatusDto syncStatusDto)
        {
            var status = await _syncStatusRepository.CreateSyncStatusAsync(syncStatusDto);
            return Ok(status);
        }
    }

    #endregion
}
