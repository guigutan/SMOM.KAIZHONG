using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json.Serialization;
using SIE;
using SIE.Web.Interface;
using SIE.SignalRService;
using System;
using System.Linq;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace WebClient
{
    /// <summary>
    /// WEB启动
    /// </summary>
    public class Startup
    {

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            HostingEnvironment = env;

        }

        public IConfiguration Configuration { get; }

        public IWebHostEnvironment HostingEnvironment { get; set; }


        /// <summary>
        /// 配置服务
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            #region IServiceCollection解耦 modify by lwc 2020-4-30
            var modules = RT.GetAllModules();
            modules = modules.Where(m => typeof(IStartupExtension).IsAssignableFrom(m.Instance.GetType()));
            foreach (var module in modules)
            {
                var se = (IStartupExtension)module.Instance;
                se.ConfigServices(services, HostingEnvironment);
            }
            #endregion

            //tips 添加 SignalR中间件
            services.AddSignalR(options =>
            {
                options.EnableDetailedErrors = true;

            })
            // signalR json协议的参数配置
            .AddNewtonsoftJsonProtocol(options =>
            {
                //默认的数据序列化会转成驼峰格式。这里改成不自动转换，按原类型序列化。
                options.PayloadSerializerSettings.ContractResolver = new DefaultContractResolver();
            });
            services.AddSingleton(HtmlEncoder.Create(UnicodeRanges.All));
            //添加后台服务配置
            AddHostServices(services);
        }



        /// <summary>
        /// 配置请求管道
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            #region 静态资源的重定向
            app.UseCustomRewriter();
            #endregion
            #region IApplicationBuilder解耦 modify by lwc 2020-4-30
            var modules = RT.GetAllModules();
            modules = modules.Where(m => typeof(IStartupExtension).IsAssignableFrom(m.Instance.GetType()));
            foreach (var module in modules)
            {
                var se = (IStartupExtension)module.Instance;
                se.ConfigureApp(app, HostingEnvironment);
            }
            #endregion

            UseSignalR(app);

            //把.net core自带的DI容器注入到平台容器中。提供途径，在平台容器获取到.net core自带容器的内容。
            var provider = app.ApplicationServices.CreateScope().ServiceProvider; //包含范围域中的内容
            RT.Service.Register(typeof(IServiceProvider), provider);
        }

        /// <summary>
        /// 添加后台服务配置
        /// </summary>
        /// <param name="services"></param>
        private static void AddHostServices(IServiceCollection services)
        {
            //把继承了BackGroundServiceBase的后台服务类添加HostServices配置
            var bgServiceTypes = RT.GetAllModules().SelectMany(p => p.Assembly.GetTypes()).Where(p => p.IsSubclassOf(typeof(BackGroundServiceBase)) && !p.IsAbstract).ToList();
            //根据类型指定泛型方法来调用。例如services.AddHostedService<SpcChartBgService>();
            var thisServicesType = typeof(ServiceCollectionHostedServiceExtensions).GetMethods().FirstOrDefault(p => p.Name.Equals("AddHostedService", StringComparison.OrdinalIgnoreCase) && p.IsGenericMethod && p.GetParameters().Length == 1);//扩展方法，参数长度为1
            foreach (var type in bgServiceTypes)
            {
                var methodInfo = thisServicesType?.MakeGenericMethod(type);
                methodInfo?.Invoke(null, new[] { services });
            }
        }

        /// <summary>
        /// 添加已加载模块里的Hub配置
        /// </summary>
        /// <param name="app"></param>
        private static void UseSignalR(IApplicationBuilder app)
        {
            /**
             * 官网教程改造：https://docs.microsoft.com/zh-cn/aspnet/core/tutorials/signalr?tabs=visual-studio&view=aspnetcore-3.1
             * 这里作了二次封装，把继承Hub基类的SignalR 的实现，从各dll Module中获取出来，然后注册。
             * HubAttribute 配置路径的，如
             * 示例：
             * <code>
             * [Hub("Spc控制图运行双工通信", "/spcchart_hub")]
             * public class SpcChartHub : Hub
             * ......代码略
             * </code>
             * 
             */

            //把继承了Hub的双工通信类添加SignalR配置
            var hubTypes = RT.GetAllModules().SelectMany(p => p.Assembly.GetTypes()).Where(p => p.IsSubclassOf(typeof(Hub)) && !p.IsAbstract).ToList();
            app.UseEndpoints(endpoints =>
            {
                //类型指定泛型方法来调用。实现调用例如 //endpoints.MapHub<SIE.Web.SPC.SpcCharts.Hubs.SpcChartHub>("/spcchart_hub");
                var genericMethod = typeof(HubEndpointRouteBuilderExtensions).GetMethods(BindingFlags.Static | BindingFlags.Public)
                .FirstOrDefault(p => p.Name.Equals("MapHub", StringComparison.OrdinalIgnoreCase) && p.IsGenericMethod);
                if (null != genericMethod)
                {
                    foreach (var type in hubTypes)
                    {
                        if (type.IsDefined(typeof(HubAttribute)))
                        {
                            var hubAttr = type.GetCustomAttribute<HubAttribute>();
                            // tips 这里对特性进行检查，以免编绎时不报错，然后运行时配置参数报错又找不到具体原因，因为官方的报错不是很直观，只是报连接参数不对。
                            if (hubAttr.Route.IsNullOrWhiteSpace())
                            {
                                throw new ArgumentNullException(nameof(hubAttr.Route), $"来源提示：{type.FullName}");
                            }
                            var methodInfo = genericMethod?.MakeGenericMethod(type);
                            methodInfo?.Invoke(null, new object[] { endpoints, hubAttr.Route });
                        }
                    }
                }
            });
             
            //添加MIME
            var mimeMap = RT.Config.GetSection("MimeMap");
            if (mimeMap != null && mimeMap.Keys.Count > 0)
            {
                var provider = new Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider();
                foreach (var name in mimeMap.Keys)
                {
                    var mime = mimeMap.Get<MimeMapSection>(name);
                    if (!mime.FileExtension.IsNullOrEmpty() && !mime.MimeType.IsNullOrEmpty())
                    {
                        provider.Mappings[mime.FileExtension] = mime.MimeType;
                    }
                }
                app.UseStaticFiles(new StaticFileOptions
                {
                    ContentTypeProvider = provider
                });

            }
        }
    }

    public class MimeMapSection
    {
        /// <summary>
        /// 后缀
        /// </summary>
        public string FileExtension { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public string MimeType { get; set; }
    }

    #region TODO 抽到对用的模块
    public class ResourceRedirectRule : IRule
    {
        private readonly string[] _matchPaths;
        private readonly string _newPath;

        public ResourceRedirectRule(string[] matchPaths, string newPath)
        {
            _matchPaths = matchPaths;
            _newPath = newPath;
        }

        public void ApplyRule(RewriteContext context)
        {
            var request = context.HttpContext.Request;

            // 已经是目标地址了，直接返回
            if (request.Path.StartsWithSegments(new PathString(_newPath)))
            {
                return;
            }
            var str = _matchPaths.FirstOrDefault(c => request.Path.Value.Contains(c));
            if (str != null)
            {
                var newLocation = request.Path.Value.Replace(str, _newPath);

                var response = context.HttpContext.Response;
                response.StatusCode = StatusCodes.Status302Found;
                context.Result = RuleResult.EndResponse;
                response.Headers[HeaderNames.Location] = newLocation;
            }
        }

    }

    public static class UseRewriterExtension
    {
        public static IApplicationBuilder UseCustomRewriter(this IApplicationBuilder app)
        {
            var rewrite = new RewriteOptions()
               .Add(new ResourceRedirectRule(
               matchPaths: new string[] { "/wf/_content/Elsa.Designer.Components.Web/themes/theme-neptune/resources/images" },
               newPath: "/ExtJs/themes/theme-neptune/resources/images"));
            app.UseRewriter(rewrite);
            return app;
        }
    }
    #endregion
}
