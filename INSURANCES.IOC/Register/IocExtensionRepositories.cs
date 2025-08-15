using INSURANCES.CORE.Ports.Respository;
using INSURANCES.DATA.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace INSURANCES.IOC.Register
{
    public static class IocExtensionRepositories
    {
        public static void RegisterRepositories(this IServiceCollection services)
        {
            services
                .AddTransient<IProposalRespository, ProposalRespository>()
                .AddTransient<IHiringRepository, HiringRepository>();
        }
    }
}
