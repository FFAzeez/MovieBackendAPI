using Microsoft.Extensions.Hosting;

namespace MovieBackendAPI.Middleware
{
    public static class EnvironmentExtension
    {
        public static bool IsUat(this IHostEnvironment hostEnvironment)
            => hostEnvironment.IsEnvironment("uat");

        public static bool IsLocal(this IHostEnvironment hostEnvironment)
        => hostEnvironment.IsEnvironment("local");
    }
}
