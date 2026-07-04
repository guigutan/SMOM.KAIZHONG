using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using SIE.Modules;
using SIE.Services;
using SIE;
using SIE.Web;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using Topshelf.Logging;

namespace WebClient
{
    /// <summary>
    /// Host服务
    /// </summary>
    public class HostService
    {
        static readonly LogWriter log = HostLogger.Get("HostService"); //logger name
        readonly IHostBuilder hostBuilder;
        IHost host;
        readonly WebApp app;

        /// <summary>
        /// 
        /// </summary>
        public HostService(string hostUrls, params string[] args)
        {
            Console.WriteLine(hostUrls);
            var path = typeof(Program).Assembly.Location; //获取dll路径 支持.netCore
            var pathToContentRoot = Path.GetDirectoryName(path);
            Directory.SetCurrentDirectory(pathToContentRoot);
            log.Info("SetCurrentDirectory:" + pathToContentRoot);

            string webRootDir = Environment.GetEnvironmentVariable("ASPNETCORE_WEBROOT");
            //支持环境变量设置内容根目录
            if (webRootDir.IsNotEmpty())
                webRootDir = Path.GetDirectoryName(Path.GetFullPath(webRootDir, pathToContentRoot));
            else
                webRootDir = "wwwroot";

            //支持取配置端口
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(GetJsonConfigName(), optional: true)
                .Build();
            hostBuilder = Host.CreateDefaultBuilder(args);
            bool inProxyMode = IsProcessContained("iisexpress");
            hostBuilder.ConfigureWebHostDefaults(webBuilder =>
            {
                if (inProxyMode)
                    webBuilder.UseIISIntegration();
                else
                {
                    webBuilder.UseKestrel((context, options) =>
                    {
                        options.Limits.MaxRequestBodySize = int.MaxValue; //请求内容大小限制 
                        options.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(30);
                        options.Limits.RequestHeadersTimeout = TimeSpan.FromMinutes(1);

                        if (hostUrls.Contains("https://", StringComparison.InvariantCultureIgnoreCase))
                        {
                            hostUrls.Split(';').ForEach(item =>
                            {
                                string[] itemOpts = item.Split(':');
                                if (item.StartsWith("https://", StringComparison.InvariantCultureIgnoreCase))
                                {
                                    int port = (itemOpts.Length == 3) ? Convert.ToInt32(itemOpts[2]) : 443;
                                    options.ListenAnyIP(port, cfg =>
                                    {
                                        cfg.UseHttps(GetHttpsCertificate(config, itemOpts[1].TrimStart('/')));
                                    });
                                }
                                else
                                {
                                    int port = (itemOpts.Length == 3) ? Convert.ToInt32(itemOpts[2]) : 80;
                                    options.ListenAnyIP(port);
                                }
                            });
                        }

                    });
                }
                webBuilder.UseConfiguration(config)
                .UseUrls(hostUrls.Split(';'))
                .UseContentRoot(pathToContentRoot)
                .UseWebRoot(webRootDir)
                .UseStartup<Startup>();
            });

            log.Info("Web Host Build...");
            app = new WebApp();
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

            if (cnName == "*" || cnName == "127.0.0.1") cnName = "localhost";
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
        /// 获取环境配置文件(JSON)
        /// </summary>
        /// <returns></returns>
        public static string GetJsonConfigName()
        {
            //支持环境变量配置文件
            string netcoreEnv = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (netcoreEnv != null && netcoreEnv != "Production" && File.Exists("appsettings." + netcoreEnv + ".json"))
                return "appsettings." + netcoreEnv + ".json";
            return "appsettings.json";
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
            log.Debug(args);
            return Array.Exists(args, s => s == "-servicename") && Array.Exists(args, s => s == "-instance");
        }

        /// <summary>
        /// 启动
        /// </summary>
        public void Start()
        {
            try
            {
                log.Info("HostService Starting...");
                log.Debug("dotnet run:" + IsProcessContained("dotnet"));
                app.Startup();
                host = hostBuilder.Build();
                if (IsRunInTopShelfServices())
                    host.RunAsync();
                else
                {
                    var message = GetNotRemoteClassVerify();//这里就是执行的方法
                    Console.WriteLine(message);
                    Console.WriteLine("启动成功");
                    host.Run();
                }

                log.Info("HostService Started");
            }
            catch (Exception exc)
            {
                log.Fatal(exc);
                if (System.Diagnostics.Debugger.IsAttached)
                    System.Diagnostics.Debugger.Break();
				Console.WriteLine("启动失败:" + exc.GetExceptionDetail());
                throw new SIE.PlatformException("启动失败", exc);
            }
        }

        /// <summary>
        /// 停止 
        /// </summary>
        public void Stop()
        {
            host?.StopAsync().Wait();
            app.NotifyExit();
            log.Info("HostService Stopped");
        }

        /// <summary>
        /// 检查控制器类是否需要添加虚拟方法
        /// </summary>
        string GetNotRemoteClassVerify()
        {
            var modules = RT.GetAllModules().Where(f => (f.Instance is DomainModule) && f.Assembly != typeof(SIE.Domain.Entity).Assembly);
            var dic = new Dictionary<Type, List<MethodInfo>>();
            //获取所有的虚方法
            foreach (var plugin in modules)
            {
                foreach (var type in plugin.Assembly.GetTypesSafely())
                {
                    if (!typeof(RemoteService).IsAssignableFrom(type))
                        continue;
                    var methodInfos = new List<MethodInfo>();
                    var methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                    foreach (var method in methods)
                    {
                        if ((method.IsPublic || method.IsFamily) && !method.IsVirtual)
                        {
                            if (Attribute.IsDefined(method.DeclaringType, typeof(IgnoreProxyAttribute))
                               || Attribute.IsDefined(method, typeof(IgnoreProxyAttribute)))
                                continue;
                            if (method.DeclaringType == typeof(RemoteService) || method.DeclaringType == typeof(object))
                                continue;
                            methodInfos.Add(method);
                        }
                    }
                    if (methodInfos.Any())
                        dic.Add(type, methodInfos);
                }
            }
            //遍历输出
            var msg = string.Empty;
            foreach (var type in dic.Keys)
            {
                msg += "控制器：{0}\r\n".FormatArgs(type.FullName);
                foreach (var method in dic[type])
                {
                    msg += "        方法：{0}\r\n".FormatArgs(method.Name);
                }
            }
            return msg;
        }
    }
}
