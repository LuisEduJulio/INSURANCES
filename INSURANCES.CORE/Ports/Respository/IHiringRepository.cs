using INSURANCES.CORE.Entities;

namespace INSURANCES.CORE.Ports.Respository
{
    public interface IHiringRepository
    {
        Task<HiringEntity?> GetHiringByIdAsync(Guid id);
        Task<HiringEntity> PostCreateHiringAsync(HiringEntity hiringEntity);
    }
}