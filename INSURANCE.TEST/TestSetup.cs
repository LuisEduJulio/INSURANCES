using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace INSURANCE.TEST
{
    public abstract class TestBase
    {
        protected readonly ITestOutputHelper Output;
        protected readonly IServiceProvider ServiceProvider;

        protected TestBase(ITestOutputHelper output)
        {
            Output = output;
            ServiceProvider = BuildServiceProvider();
        }

        private static IServiceProvider BuildServiceProvider()
        {
            var services = new ServiceCollection();

            // Configurar logging para testes
            services.AddLogging(builder =>
            {
                builder.AddConsole();
                builder.SetMinimumLevel(LogLevel.Debug);
            });

            // Configurar AutoMapper
            services.AddAutoMapper(typeof(INSURANCES.CORE.Mappers.HiringMapper));

            return services.BuildServiceProvider();
        }

        protected void LogTestInfo(string message)
        {
            Output.WriteLine($"[{DateTime.Now:HH:mm:ss}] {message}");
        }

        protected void LogTestError(string message, Exception? exception = null)
        {
            Output.WriteLine($"[{DateTime.Now:HH:mm:ss}] ERROR: {message}");
            if (exception != null)
            {
                Output.WriteLine($"Exception: {exception.Message}");
                Output.WriteLine($"StackTrace: {exception.StackTrace}");
            }
        }
    }
}
