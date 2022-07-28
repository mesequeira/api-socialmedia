using Microsoft.AspNetCore.Mvc;

namespace ApiSocialMedia.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddApiVersioningExtension(this IServiceCollection services)
        {
            services.AddApiVersioning(p =>
            {
                p.DefaultApiVersion = new ApiVersion(1, 0);

                p.AssumeDefaultVersionWhenUnspecified = true;

                p.ReportApiVersions = true;
            });
        }
    }
}
