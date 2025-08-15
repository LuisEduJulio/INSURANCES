using INSURANCES.CORE.Dtos;
using INSURANCES.CORE.ModelView;

namespace INSURANCES.CORE.Ports.Services
{
    public interface IHiringService
    {
        Task<HiringModelView> GetHiringByIdAsync(Guid Id);
        Task<HiringModelView> PostCreateHiringAsync(PostHiringDto postHiringDto);
    }
}