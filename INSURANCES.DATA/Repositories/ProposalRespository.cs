using INSURANCES.CORE.Entities;
using INSURANCES.CORE.Ports.Respository;
using INSURANCES.DATA.Factory;
using Microsoft.EntityFrameworkCore;

namespace INSURANCES.DATA.Repositories
{
    public class ProposalRespository(AppDbContext dbContext) : IProposalRespository
    {
        private readonly AppDbContext _dbContext = dbContext;
        public async Task<ProposalEntity?> GetByIdProposalAsync(Guid id)
            => await _dbContext.ProposalEntities.Include(c => c.Hiring).FirstOrDefaultAsync(c => c.Id == id);
        public async Task<IList<ProposalEntity>> GetListProposalAsync(PaginationEntity paginationEntity)
        {
            var getEntities = await _dbContext
                   .ProposalEntities
                   .OrderBy(m => m.Id)
                   .Skip(paginationEntity.Count * (paginationEntity.Page - 1))
                   .Take(paginationEntity.Count)
                   .Include(c => c.Hiring)
                   .ToListAsync();

            return getEntities;
        }
        public async Task<ProposalEntity> PostCreateProposalAsync(ProposalEntity proposalEntity)
        {
            var newEntity = await _dbContext
                    .ProposalEntities
                    .AddAsync(proposalEntity);

            await _dbContext
                .SaveChangesAsync();

            return newEntity.Entity;
        }
        public async Task<ProposalEntity> PutAlterStatusByIdProposalAsync(ProposalEntity proposalEntity)
        {
            var updateEntity = _dbContext
                    .ProposalEntities
                    .Update(proposalEntity);

            await _dbContext.SaveChangesAsync();

            return updateEntity.Entity;
        }
    }
}