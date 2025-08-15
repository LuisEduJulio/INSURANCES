namespace INSURANCES.UTILITY.Helpers
{
    public static class EnvironmentHelper
    {
        public static readonly Func<string, string, string> Env = delegate (string variable, string defaultValue)
        {
            if (IsProduction())
                return Environment.GetEnvironmentVariable(variable) ??
                       throw new InvalidOperationException();

            return Environment.GetEnvironmentVariable(variable) ?? defaultValue;
        };
        public static bool IsProduction()
        {
            var environmentVariable = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            return environmentVariable != null &&
                   (environmentVariable.Contains("Production",
                        StringComparison.InvariantCultureIgnoreCase) ||
                    environmentVariable.Contains("Producao", StringComparison.InvariantCultureIgnoreCase) ||
                    environmentVariable.Contains("Produção", StringComparison.InvariantCultureIgnoreCase));
        }
        public static string GetApplicationName(string name) => name;
        public static string GetApplicationSwagger() => "/swagger/v1/swagger.json";
        public static string GetVersionApi() => "v1";
        public static string GetCross() => "AllowOrigin";
    }
}