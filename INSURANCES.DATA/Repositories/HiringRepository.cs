using INSURANCES.CORE.Entities;
using INSURANCES.CORE.Ports.Respository;
using INSURANCES.DATA.Factory;

namespace INSURANCES.DATA.Repositories
{
    public class HiringRepository(AppDbContext dbContext) : IHiringRepository
    {
        private readonly AppDbContext _dbContext = dbContext;
        public async Task<HiringEntity?> GetHiringByIdAsync(Guid id)
            => await _dbContext.HiringEntities.FindAsync(id);
        public async Task<HiringEntity> PostCreateHiringAsync(HiringEntity hiringEntity)
        {
            var newEntity = await _dbContext
                    .HiringEntities
                    .AddAsync(hiringEntity);

            await _dbContext
                .SaveChangesAsync();

            return newEntity.Entity;
        }
    }
}