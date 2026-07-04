using Hangfire;
using Hangfire.Dashboard;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using SIE.Schedule.Resolvers;

namespace SIE.ScheduleServer
{
    public class Startup
    {
        public void Configure(IApplicationBuilder app)
        {
            app.UseHangfireDashboard("", new DashboardOptions()
            {
                Authorization = new[] { new CustomAuthorizeFilter() }
            });
        }

        public void ConfigureServices(IServiceCollection servicecollection)
        {
            servicecollection
                .AddHangfire((provider, configuration) => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseStorage("hangfire"));

            //注入时区处理逻辑，用于调度看板周期任务报错过滤
            servicecollection.AddSingleton<ITimeZoneResolver, CrossPlatformTimeZoneResolver>();
        }
    }

    public class CustomAuthorizeFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            return true;
        }
    }
}
