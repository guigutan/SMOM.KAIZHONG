using Microsoft.AspNetCore.Hosting;
using SIE;
using SIE.Configuration;
using System;
using Topshelf;
using Topshelf.Logging;

namespace WebClient
{
    public class Program
    {
        /// <summary>
        /// 
        /// </summary>
        protected Program() { }
        public static void Main(string[] args)
        {
            //启动变量与配置
            var cfg = HostService.GetJsonConfigName();
            Console.WriteLine(cfg);
            ConfigManager.Create().UserJsonConfig(cfg);
            string envUrls = Environment.GetEnvironmentVariable("ASPNETCORE_URLS");
            string serverUrls = AppRuntime.Config.Get("urls", envUrls.IsNotEmpty() ? envUrls : "http://*:5000");
            Log4NetLogWriterFactory.Use("log4net.config");//要先适配日志
            HostService.InitialLog4net("log4net.config");
            var log = HostLogger.Get("HostService");

            log.Info("宿主环境:".L10N() + Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") + " urls:" + envUrls);
            log.Info("使用URLs:".L10N() + serverUrls);
            Console.WriteLine(serverUrls);

            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                log.Error(e.ExceptionObject?.ToString());
                log.Fatal(e.ExceptionObject);
            };

            SIE.Data.DbAccesserFactory.DbCommandPrepared += (s, e) =>
            {
                string sqlDebug = e.DbCommand.ToTraceString();
                System.Diagnostics.Trace.WriteLine(sqlDebug);
            };
#if DEBUG
            //SIE.Data.DbAccesserFactory.DbCommandPrepared += (s, e) =>
            //{
            //    string sqlDebug = e.DbCommand.ToTraceString();
            //    System.Diagnostics.Trace.WriteLine(sqlDebug);
            //};
#endif
            try
            {
                // 兼容方案： hotfix topshelf 和 iisexpress mode 未知的参数 "%LAUNCHER_ARGS%" 
                var processName = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
                if ("iisexpress" == processName)
                {
                    new HostService(serverUrls).Start();
                }
                else
                {
                    var rc = HostFactory.Run(x =>
                    {
                        x.Service<HostService>(s =>
                        {
                            s.ConstructUsing(name => new HostService(serverUrls));
                            s.WhenStarted(tc => tc.Start());
                            s.WhenStopped(tc => tc.Stop());
                        });
                        x.StartAutomaticallyDelayed();
                        x.RunAsLocalSystem();
                        x.EnableServiceRecovery((e) =>
                        {
                            e.RestartService(0);//第一次失败执行
                            e.RestartService(1);//第二次失败执行
                            e.RestartService(1);//后续失败执行

                            e.OnCrashOnly();
                            e.SetResetPeriod(1);
                        });

                        x.EnablePauseAndContinue();
                        x.EnableShutdown();

                        x.SetDescription(Configuration.ServiceDescription);
                        x.SetDisplayName(Configuration.ServiceDisplayName);
                        x.SetInstanceName(Configuration.ServiceName);
                    });
                    log.Info("服务退出:".L10N() + rc.ToString());
                    Environment.ExitCode = (int)Convert.ChangeType(rc, rc.GetTypeCode());
                }
            }
            catch (Exception e)
            {
                log.Fatal(e.GetExceptionDetail());
                Console.WriteLine(e.Message);
            }
            Console.WriteLine("按任意键退出...");
            Console.ReadKey();
        }
    }
}
