using SIE.Configuration;
using SIE.Event.Bus;
using System;
using System.Threading;
using Topshelf;
using Topshelf.Logging;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Hangfire;
using Hangfire.Client;
using Hangfire.Common;
using Hangfire.Server;
using Hangfire.States;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore;
using SIE.Schedule.Filters;
using SIE.Schedule.Resolvers;

namespace SIE.ScheduleServer
{
    public class HostService : ServiceControl
    {
        static readonly LogWriter _log = HostLogger.Get("HostService");
        Timer timer;
        HostApp app;
        IWebHost webHost;
        IHost host;

        public HostService()
        {
            app = new HostApp();
        }

        public bool Start(HostControl hostControl)
        {
            _log.Info("Schedule service Starting...");
            hostControl.RequestAdditionalTime(TimeSpan.FromSeconds(10));
            StartupHost();
            StartHangfireServer();
            Console.WriteLine("Schedule service start");
            _log.Info("Schedule service started");
            RT.EventBus.Publish(new ServiceStartEvent());
            return true;
        }


        void StartupHost()
        {
            try
            {
                app.Startup();
            }
            catch (Exception exc)
            {
                Console.WriteLine("领域应用程序在启动失败:" + exc);
                Console.ReadLine();
                throw;
            }
        }

        public bool Stop(HostControl hostControl)
        {
            StopHost();
            _log.Info("HostService Stopped");
            RT.EventBus.Publish(new ServiceStopEvent());
            return true;
        }

        public bool Pause(HostControl hostControl)
        {
            StopHost();
            _log.Info("HostService Paused");
            RT.EventBus.Publish(new ServicePauseEvent());
            return true;
        }

        public bool Continue(HostControl hostControl)
        {
            StartupHost();
            _log.Info("HostService Continued");
            RT.EventBus.Publish(new ServiceResumeEvent());
            return true;
        }

        public void StopHost()
        {
            try
            {
                app.NotifyExit();
                _log.Info("领域应用程序已退出".L10N());
                host.StopAsync();
                webHost.StopAsync();
            }
            catch (Exception exc)
            {
                _log.Info("领域应用程序退出失败:".L10N() + exc);
                throw;
            }
        }

        void StartHangfireServer()
        {
            host = new HostBuilder()
                 .ConfigureLogging(x => x.AddConsole().SetMinimumLevel(LogLevel.Information))
                 .ConfigureServices((hostContext, services) =>
                 {
                     services.Configure<HostOptions>(option =>
                     {
                         option.ShutdownTimeout = TimeSpan.FromSeconds(60);
                     });

                     //注入时区处理逻辑，转换时区，用于调度后台定时任务触发时间计算
                     services.TryAddSingleton<ITimeZoneResolver>(x => new CrossPlatformTimeZoneResolver());

                     services.TryAddSingleton<BackgroundJobServerOptions>(new BackgroundJobServerOptions
                     {
                         ServerName = string.Format("{0}{1}", Environment.UserDomainName, Guid.NewGuid().ToString()),
                         Queues = new[] { "queue0", "queue1", "queue2", "queue3", "queue4", "default" },
                         StopTimeout = TimeSpan.FromSeconds(15),
                         ShutdownTimeout = TimeSpan.FromSeconds(30)
                     });

                     services.TryAddSingleton<IBackgroundJobFactory>(x => new CustomBackgroundJobFactory(
                         new BackgroundJobFactory(x.GetRequiredService<IJobFilterProvider>())));

                     services.TryAddSingleton<IBackgroundJobPerformer>(x => new CustomBackgroundJobPerformer(
                         new BackgroundJobPerformer(
                             x.GetRequiredService<IJobFilterProvider>(),
                             x.GetRequiredService<JobActivator>(),
                             TaskScheduler.Default)));

                     services.TryAddSingleton<IBackgroundJobStateChanger>(x => new CustomBackgroundJobStateChanger(
                             new BackgroundJobStateChanger(x.GetRequiredService<IJobFilterProvider>())));

                     services.AddHangfire((provider, configuration) => configuration
                         .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                         .UseSimpleAssemblyNameTypeSerializer()
                         .UseFilter(new RecurringJobFilter()) //避免失效调度任务执行
                         .UseStorage("hangfire"));

                     services.AddHostedService<RecurringJobsService>();
                     services.AddHangfireServer();
                 })
                 .Build();

            host.RunAsync();


            string serverUrls = AppRuntime.Config.Get(ConfigKeys.Urls, "http://*:5050");
            webHost = WebHost.CreateDefaultBuilder()
                .UseKestrel()
                .ConfigureLogging(x => x.AddConsole().SetMinimumLevel(LogLevel.Information))
                .UseUrls(serverUrls)
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                .Build();

            webHost.RunAsync();
        }
    }
}
