using INSURANCES.CORE.Mappers;
using Microsoft.Extensions.DependencyInjection;

namespace INSURANCES.IOC.Register
{
    public static class IocExtensionAutoMappers
    {
        public static void RegisterAutoMappers(this IServiceCollection services)
        {
            services
                 .AddAutoMapper(typeof(HiringMapper))
                 .AddAutoMapper(typeof(ProposalMapper));
        }
    }
}
