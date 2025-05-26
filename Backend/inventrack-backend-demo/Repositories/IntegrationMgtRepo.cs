using inventrack_backend_demo.DTOs;
using inventrack_backend_demo.Model;
using inventrack_backend_demo.Data;
using Microsoft.EntityFrameworkCore;

namespace inventrack_backend_demo.Repositories
{
    #region Integration Repositories

    public interface IIntegrationPartnerRepository
    {
        Task<List<IntegrationPartnerDto>> GetAllPartnersAsync();
        Task<IntegrationPartnerDto> GetPartnerByIdAsync(int partnerId);
        Task<IntegrationPartnerDto> CreatePartnerAsync(IntegrationPartnerDto partnerDto);
        Task<IntegrationPartnerDto> UpdatePartnerAsync(int partnerId, IntegrationPartnerDto partnerDto);
        Task<bool> DeletePartnerAsync(int partnerId);
    }

    public class IntegrationPartnerRepository : IIntegrationPartnerRepository
    {
        private readonly ApplicationDbContext _context;

        public IntegrationPartnerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<IntegrationPartnerDto>> GetAllPartnersAsync()
        {
            return await _context.IntegrationPartners
                .Select(p => new IntegrationPartnerDto
                {
                    PartnerId = p.PartnerId,
                    PartnerName = p.PartnerName,
                    PartnerType = p.PartnerType,
                    Status = p.Status,
                    LastSyncDate = p.LastSyncDate
                }).ToListAsync();
        }

        public async Task<IntegrationPartnerDto> GetPartnerByIdAsync(int partnerId)
        {
            var partner = await _context.IntegrationPartners
                .Include(p => p.Configurations)
                .FirstOrDefaultAsync(p => p.PartnerId == partnerId);

            if (partner == null) return null;

            return new IntegrationPartnerDto
            {
                PartnerId = partner.PartnerId,
                PartnerName = partner.PartnerName,
                PartnerType = partner.PartnerType,
                APIEndpoint = partner.APIEndpoint,
                AuthType = partner.AuthType,
                Status = partner.Status,
                LastSyncDate = partner.LastSyncDate,
                Configurations = partner.Configurations?.Select(c => new IntegrationConfigDto
                {
                    ConfigId = c.ConfigId,
                    PartnerId = c.PartnerId,
                    ConfigKey = c.ConfigKey,
                    IsEncrypted = c.IsEncrypted
                }).ToList()
            };
        }

        public async Task<IntegrationPartnerDto> CreatePartnerAsync(IntegrationPartnerDto partnerDto)
        {
            var partner = new IntegrationPartner
            {
                PartnerName = partnerDto.PartnerName,
                PartnerType = partnerDto.PartnerType,
                APIEndpoint = partnerDto.APIEndpoint,
                AuthType = partnerDto.AuthType,
                Status = "Active"
            };

            _context.IntegrationPartners.Add(partner);
            await _context.SaveChangesAsync();

            return await GetPartnerByIdAsync(partner.PartnerId);
        }

        public async Task<IntegrationPartnerDto> UpdatePartnerAsync(int partnerId, IntegrationPartnerDto partnerDto)
        {
            var partner = await _context.IntegrationPartners.FindAsync(partnerId);
            if (partner == null) return null;

            partner.PartnerName = partnerDto.PartnerName;
            partner.PartnerType = partnerDto.PartnerType;
            partner.APIEndpoint = partnerDto.APIEndpoint;
            partner.AuthType = partnerDto.AuthType;
            partner.Status = partnerDto.Status;
            partner.LastSyncDate = partnerDto.LastSyncDate;

            await _context.SaveChangesAsync();
            return await GetPartnerByIdAsync(partnerId);
        }

        public async Task<bool> DeletePartnerAsync(int partnerId)
        {
            var partner = await _context.IntegrationPartners.FindAsync(partnerId);
            if (partner == null) return false;

            partner.Status = "Inactive";
            await _context.SaveChangesAsync();
            return true;
        }
    }

    public interface IIntegrationConfigRepository
    {
        Task<List<IntegrationConfigDto>> GetConfigsByPartnerAsync(int partnerId);
        Task<IntegrationConfigDto> GetConfigByIdAsync(int configId);
        Task<IntegrationConfigDto> CreateConfigAsync(IntegrationConfigDto configDto);
        Task<IntegrationConfigDto> UpdateConfigAsync(int configId, IntegrationConfigDto configDto);
        Task<bool> DeleteConfigAsync(int configId);
    }

    public class IntegrationConfigRepository : IIntegrationConfigRepository
    {
        private readonly ApplicationDbContext _context;

        public IntegrationConfigRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<IntegrationConfigDto>> GetConfigsByPartnerAsync(int partnerId)
        {
            return await _context.IntegrationConfigs
                .Where(c => c.PartnerId == partnerId)
                .Select(c => new IntegrationConfigDto
                {
                    ConfigId = c.ConfigId,
                    PartnerId = c.PartnerId,
                    ConfigKey = c.ConfigKey,
                    IsEncrypted = c.IsEncrypted
                }).ToListAsync();
        }

        public async Task<IntegrationConfigDto> GetConfigByIdAsync(int configId)
        {
            var config = await _context.IntegrationConfigs.FindAsync(configId);
            if (config == null) return null;

            return new IntegrationConfigDto
            {
                ConfigId = config.ConfigId,
                PartnerId = config.PartnerId,
                ConfigKey = config.ConfigKey,
                ConfigValue = config.ConfigValue,
                IsEncrypted = config.IsEncrypted
            };
        }

        public async Task<IntegrationConfigDto> CreateConfigAsync(IntegrationConfigDto configDto)
        {
            var config = new IntegrationConfig
            {
                PartnerId = configDto.PartnerId,
                ConfigKey = configDto.ConfigKey,
                ConfigValue = configDto.ConfigValue,
                IsEncrypted = configDto.IsEncrypted
            };

            _context.IntegrationConfigs.Add(config);
            await _context.SaveChangesAsync();

            return await GetConfigByIdAsync(config.ConfigId);
        }

        public async Task<IntegrationConfigDto> UpdateConfigAsync(int configId, IntegrationConfigDto configDto)
        {
            var config = await _context.IntegrationConfigs.FindAsync(configId);
            if (config == null) return null;

            config.ConfigKey = configDto.ConfigKey;
            config.ConfigValue = configDto.ConfigValue;
            config.IsEncrypted = configDto.IsEncrypted;

            await _context.SaveChangesAsync();
            return await GetConfigByIdAsync(configId);
        }

        public async Task<bool> DeleteConfigAsync(int configId)
        {
            var config = await _context.IntegrationConfigs.FindAsync(configId);
            if (config == null) return false;

            _context.IntegrationConfigs.Remove(config);
            await _context.SaveChangesAsync();
            return true;
        }
    }

    public interface ISyncStatusRepository
    {
        Task<List<SyncStatusDto>> GetSyncStatusesByPartnerAsync(int partnerId);
        Task<SyncStatusDto> GetLatestSyncStatusAsync(int partnerId, string entityType);
        Task<SyncStatusDto> CreateSyncStatusAsync(SyncStatusDto syncStatusDto);
    }

    public class SyncStatusRepository : ISyncStatusRepository
    {
        private readonly ApplicationDbContext _context;

        public SyncStatusRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<SyncStatusDto>> GetSyncStatusesByPartnerAsync(int partnerId)
        {
            return await _context.SyncStatuses
                .Where(s => s.PartnerId == partnerId)
                .OrderByDescending(s => s.StartTime)
                .Select(s => new SyncStatusDto
                {
                    SyncId = s.SyncId,
                    PartnerId = s.PartnerId,
                    EntityType = s.EntityType,
                    StartTime = s.StartTime,
                    EndTime = s.EndTime,
                    Status = s.Status,
                    RecordsProcessed = s.RecordsProcessed,
                    RecordsSucceeded = s.RecordsSucceeded,
                    RecordsFailed = s.RecordsFailed
                }).ToListAsync();
        }

        public async Task<SyncStatusDto> GetLatestSyncStatusAsync(int partnerId, string entityType)
        {
            var syncStatus = await _context.SyncStatuses
                .Where(s => s.PartnerId == partnerId && s.EntityType == entityType)
                .OrderByDescending(s => s.StartTime)
                .FirstOrDefaultAsync();

            if (syncStatus == null) return null;

            return new SyncStatusDto
            {
                SyncId = syncStatus.SyncId,
                PartnerId = syncStatus.PartnerId,
                EntityType = syncStatus.EntityType,
                StartTime = syncStatus.StartTime,
                EndTime = syncStatus.EndTime,
                Status = syncStatus.Status,
                RecordsProcessed = syncStatus.RecordsProcessed,
                RecordsSucceeded = syncStatus.RecordsSucceeded,
                RecordsFailed = syncStatus.RecordsFailed,
                ErrorDetails = syncStatus.ErrorDetails
            };
        }

        public async Task<SyncStatusDto> CreateSyncStatusAsync(SyncStatusDto syncStatusDto)
        {
            var syncStatus = new SyncStatus
            {
                PartnerId = syncStatusDto.PartnerId,
                EntityType = syncStatusDto.EntityType,
                StartTime = DateTime.UtcNow,
                Status = "In Progress"
            };

            _context.SyncStatuses.Add(syncStatus);
            await _context.SaveChangesAsync();

            // Update partner's last sync date
            var partner = await _context.IntegrationPartners.FindAsync(syncStatusDto.PartnerId);
            if (partner != null)
            {
                partner.LastSyncDate = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }

            return new SyncStatusDto
            {
                SyncId = syncStatus.SyncId,
                PartnerId = syncStatus.PartnerId,
                EntityType = syncStatus.EntityType,
                StartTime = syncStatus.StartTime,
                Status = syncStatus.Status
            };
        }
    }

    #endregion
}
