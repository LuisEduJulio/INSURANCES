using INSURANCES.APPLICATION.Services;
using INSURANCES.CORE.Ports.Services;
using Microsoft.Extensions.DependencyInjection;

namespace INSURANCES.IOC.Register
{
    public static class IocExtensionServices
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services
                .AddTransient<IProposalService, ProposalService>()
                .AddTransient<IHiringService, HiringService>();
        }
    }
}