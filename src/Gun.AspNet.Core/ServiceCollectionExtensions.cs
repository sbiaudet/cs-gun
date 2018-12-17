using Gun.AspNet.Core;
using Gun.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGun(this IServiceCollection services)
        {
            services.AddTransient<IDuplicateManager, DuplicateManager>();
            services.AddTransient<WebSocketConnectionManager>();

            services.AddSingleton<GunHandler>();

            return services;
        }
    }
}
