using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.WebSockets;
using Gun.AspNet.Core;

namespace Microsoft.AspNetCore.Builder
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseGun(this IApplicationBuilder app)
        {

            //app.UseMiddleware<WebSocketManagerMiddleware>(app.ApplicationServices.GetRequiredService<GunHandler>());
            app.Map("/gun", (_app) => _app.UseMiddleware<WebSocketManagerMiddleware>(app.ApplicationServices.GetRequiredService<GunHandler>()));

            return app;
        } 
    }
}
