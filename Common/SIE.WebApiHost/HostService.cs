using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using SIE.Configuration;
using SIE.Domain;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Threading;
using Topshelf.Logging;

namespace SIE.WebApiHost
{
    /// <summary>
    /// Host服务
    /// </summary>
    public class HostService
    {
        static readonly LogWriter log = HostLogger.Get("HostService"); //logger name
        readonly IHostBuilder hostBuilder;
        IHost host;
        readonly DomainApp app;

        /// <summary>
        /// Host服务
        /// </summary>
        public HostService()
        {
            var path = typeof(Program).Assembly.Location; //获取dll路径 支持.netCore
            var pathToContentRoot = Path.GetDirectoryName(path);
            Directory.SetCurrentDirectory(pathToContentRoot);
            log.Debug("SetCurrentDirectory:" + pathToContentRoot);

            //支持取配置端口
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .Build();

            string envUrls = Environment.GetEnvironmentVariable("ASPNETCORE_URLS");
            string serverUrls = AppRuntime.Config.Get(ConfigKeys.Urls, envUrls.IsNotEmpty() ? envUrls : "http://*:5000");
            log.Info("宿主环境:".L10N() + Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") + " urls:" + envUrls);
            log.Info("使用URLs:".L10N() + serverUrls);
            Console.WriteLine(serverUrls);

            hostBuilder = Host.CreateDefaultBuilder();
            hostBuilder.ConfigureWebHostDefaults(webBuilder =>
            {
                if (IsProcessContained("iisexpress"))
                {
                    log.Warn("当前使用iisexpress运行，Http上传配置受限于iisexpress, 请注意生产环境是dotnet自托管执行！".L10N());
                    //使用下面的Kestrel选项会导致程序启动界面阻塞，无论时阻塞执行还是异步执行
                }
                else
                {
                    //https://github.com/dotnet/aspnetcore/blob/3.0/src/Servers/HttpSys/src/MessagePump.cs#L86
                    webBuilder.UseKestrel(options =>
                    {
                        //所有controller都不限制post的body大小
                        options.Limits.MaxRequestBodySize = null;
                        options.Limits.MinRequestBodyDataRate = new MinDataRate(bytesPerSecond: 100, gracePeriod: TimeSpan.FromSeconds(10));
                        options.AddServerHeader = false;
                        if (serverUrls.Contains("https://", StringComparison.InvariantCultureIgnoreCase))
                        {
                            serverUrls.Split(';').ForEach(item =>
                            {
                                string[] itemOpts = item.Split(':');
                                if (item.StartsWith("https://", StringComparison.InvariantCultureIgnoreCase))
                                {
                                    int port = (itemOpts.Length == 3) ? int.Parse(itemOpts[2]) : 443;
                                    options.ListenAnyIP(port, cfg =>
                                    {
                                        cfg.UseHttps(GetHttpsCertificate(config, itemOpts[1].TrimStart('/')));
                                    });
                                }
                                else
                                {
                                    int port = (itemOpts.Length == 3) ? int.Parse(itemOpts[2]) : 80;
                                    options.ListenAnyIP(port);
                                }
                            });
                        }
                    });
                }
                webBuilder.UseConfiguration(config)
                    .UseStartup<WebApiStartup>();
            });

            log.Debug("Api Host Build...");
            app = new DomainApp();
        }

        private static X509Certificate2 GetHttpsCertificate(IConfigurationRoot config, string cnName = "localhost")
        {
            var certificateSettings = config.GetSection("certificateSettings");
            if (certificateSettings != null)
            {
                var certificateFileName = certificateSettings.GetValue<string>("filename");
                var certificatePassword = certificateSettings.GetValue<string>("password");
                if (!string.IsNullOrEmpty(certificateFileName) && File.Exists(certificateFileName))
                    return new X509Certificate2(certificateFileName, certificatePassword);
            }

            if (cnName == "*" || cnName == "127.0.0.1") 
                cnName = "localhost";
            using (var store = new X509Store(StoreName.My, StoreLocation.LocalMachine))
            {
                store.Open(OpenFlags.ReadOnly);
                var certCollection = store.Certificates;
                var currentCerts = certCollection.Find(X509FindType.FindBySubjectDistinguishedName, $"CN={cnName}", false);
                if (currentCerts.Count == 0)
                {
#pragma warning disable S112 // General exceptions should never be thrown
                    throw new Exception($"Https certificate of CN={cnName} is not found.");
#pragma warning restore S112 // General exceptions should never be thrown
                }
                return currentCerts[0];
            }
        }

        /// <summary>
        /// 判断是否是依赖dotnet依赖执行
        /// </summary>
        /// <param name="processName">包含的进程名称</param>
        /// <returns></returns>
        public static bool IsProcessContained(string processName = "dotnet")
        {
            string currentProcessName = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
            return currentProcessName.Contains(processName);
        }

        /// <summary>
        /// 初始化Log4net日志目录
        /// </summary>
        /// <param name="logCfgName"></param>
        public static void InitialLog4net(string logCfgName)
        {
            string workDir = Path.GetDirectoryName(typeof(HostService).Assembly.Location);
            if (Directory.GetCurrentDirectory() != workDir)
                Directory.SetCurrentDirectory(workDir);
            log4net.Config.XmlConfigurator.ConfigureAndWatch(log4net.LogManager.GetRepository(typeof(HostService).Assembly), new FileInfo(logCfgName));
        }

        /// <summary>
        /// 判断是否以windows服务方式运行
        /// </summary>
        /// <returns></returns>
        public static bool IsRunInTopShelfServices()
        {
            string[] args = Environment.GetCommandLineArgs();
            return Array.Exists(args, s => s == "-servicename") && Array.Exists(args, s => s == "-instance");
        }

        CancellationToken hostToken = new CancellationToken();

        /// <summary>
        /// 启动
        /// </summary>
        public void Start()
        {
            try
            {
                log.Debug("HostService Starting...");
                log.Debug("dotnet run:" + IsProcessContained("dotnet"));

                app.Startup();
                host = hostBuilder.Build();
                log.Debug("HostService Starting ...");
                if (IsRunInTopShelfServices())
                {
                    log.Debug("服务状态，异步执行！");
                    host.RunAsync(hostToken);
                }
                else
                {
                    log.Debug("用户交互状态，阻塞执行！".L10N());
                    Console.WriteLine("启动成功".L10N());
                    host.Run();
                }
                log.Debug("HostService Started");
            }
            catch (Exception exc)
            {
                log.Fatal(exc.GetExceptionDetail());
                if (System.Diagnostics.Debugger.IsAttached)
                    System.Diagnostics.Debugger.Break();
                Console.WriteLine("启动失败:".L10N() + exc.GetExceptionDetail());
                //必须的插件启动如果异常，则抛出，终止程序
                EnsurePluginsStarted(exc);
            }
        }

        /// <summary>
        /// 必须的插件启动如果异常，则抛出，终止程序
        /// </summary>
        /// <param name="exc"></param>
        private void EnsurePluginsStarted(Exception exc)
        {
            //MQ模块初始化异常处理
            MQInitError(exc);

            //Redis模块初始化异常处理
            RedisInitError(exc);
        }

        /// <summary>
        /// MQ模块初始化异常处理
        /// </summary>
        /// <param name="exc"></param>
        private void MQInitError(Exception exc)
        {
            var pattern = @".*模块\[SIE.MQ.*?\]初始化失败";
            var match = Regex.Match(exc.Message, pattern);
            //匹配MQ初始化异常，抛出
            if (match.Success)
                throw exc;
        }

        /// <summary>
        /// Redis模块初始化异常处理
        /// </summary>
        /// <param name="exc"></param>
        private void RedisInitError(Exception exc)
        {
            if (exc.TargetSite.Name == "RedisInitTryConnect")
                throw exc;
        }

        /// <summary>
        /// 停止 
        /// </summary>
        public void Stop()
        {
            log.Debug("HostService Stopping ...");
            host?.StopAsync(hostToken);
            app.NotifyExit();
            log.Debug("HostService Stopped");
        }

    }
}
