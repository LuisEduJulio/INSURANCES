using INSURANCES.CORE.Dtos;
using INSURANCES.DATA.Entities;
using INSURANCES.DATA.Factory;
using INSURANCES.IOC.Register;
using INSURANCES.UTILITY.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using FluentValidation;

namespace INSURANCES.IOC.Dependecies
{
    public static class DependencyContainer
    {
        public static IServiceCollection RegisterIocDependencies(this IServiceCollection Services, IConfiguration Configuration, string name = "")
        {
            Services
               .AddMvc();

            Services
               .AddHttpClient();

            Services
                .AddControllers();

            Services
               .RegisterAutoMappers();

            Services
                .RegisterRepositories();

            Services
                .RegisterServices();

            Services
                .AddLogging();

            Services
            .AddValidatorsFromAssemblyContaining<GetProposalListDto>();

            Services
                .Configure<AppSettings>(Configuration);

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            var configuration = builder.Build();

            Services
                .AddDbContext<AppDbContext>(options => options
                   .UseSqlServer(Configuration
                       .GetConnectionString("SqlServerConnection")));

            var appSettings = configuration.Get<AppSettings>();

            Services.AddCors(c =>
            {
                c.AddPolicy(EnvironmentHelper.GetCross(),
                    options => options
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });

            Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(EnvironmentHelper.GetVersionApi(),
                    new OpenApiInfo
                    {
                        Title = EnvironmentHelper.GetApplicationName(name),
                        Version = EnvironmentHelper.GetVersionApi()
                    });
            });

            Services.AddEndpointsApiExplorer();

            Services.AddAuthorization();

            return Services;
        }
    }
}
