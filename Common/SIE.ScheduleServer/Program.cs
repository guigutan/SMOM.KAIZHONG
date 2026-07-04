using SIE.Configuration;
using System;
using Topshelf;
using Topshelf.Logging;

namespace SIE.ScheduleServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Log4NetLogWriterFactory.Use("log4net.config");//要先适配日志
            var log = HostLogger.Get("HostService");
            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                Console.WriteLine(e.ExceptionObject?.ToString());
                log.Fatal(e.ExceptionObject);
            };

            ConfigManager.Create().UserJsonConfig("appsettings.json");

            HostFactory.New(x =>
            {
                x.Service<HostService>();
                x.SetDescription(Configuration.ServiceDescription);
                x.SetDisplayName(Configuration.ServiceDisplayName);
                x.SetInstanceName(Configuration.ServiceName);
                x.StartAutomaticallyDelayed(); // Automatic (Delayed) -- only available on .NET 4.0 or later
                x.RunAsLocalSystem();
                x.EnableServiceRecovery((e) =>
                {
                    e.RestartService(0);//第一次失败执行
                    e.RestartService(0);//第二次失败执行
                    e.RestartService(0);//后续失败执行

                    e.OnCrashOnly();
                    e.SetResetPeriod(1);
                });

                x.EnablePauseAndContinue();
                x.EnableShutdown();

            }).Run();
        }
    }
}
