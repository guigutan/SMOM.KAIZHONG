using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using SIE.Domain.Serialization.Json;
using SIE.ServerCore.Consul;
using System;
using System.Linq;
using System.Reflection;

namespace SIE.WebApiHost
{

    #region 模块以委托方式扩展
    public delegate void ConfigureServicesExtension(IServiceCollection services, IWebHostEnvironment HostingEnvironment, IConfiguration Configuration);

    public delegate void ConfigureExtension(IApplicationBuilder app, IWebHostEnvironment env, IConfiguration configuration);
    #endregion

    /// <summary>
    /// WebApi服务启动
    /// </summary>
    public class WebApiStartup
    {
        #region 企业微信推送临时增加 2021-5-31
        public WebApiStartup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            HostingEnvironment = env;
        }

        public IConfiguration Configuration { get; }

        public IWebHostEnvironment HostingEnvironment { get; set; }
        #endregion

        /// <summary>
        /// 判断模块实例是否存在指定委托的函数签名，如存在则创建符合委托的实例
        /// </summary>
        /// <typeparam name="DelegateType">签名委托类型</typeparam>
        /// <param name="instanceTarget">模块实例</param>
        /// <param name="func">导出的委托函数实例</param>
        /// <returns></returns>
        bool HasDelegateMethod<DelegateType>(object instanceTarget, out DelegateType func)
            where DelegateType : MulticastDelegate
        {
            func = default(DelegateType);
            Type delegateType = typeof(DelegateType);
            MethodInfo funMethod = delegateType.GetMethod("Invoke");
            ParameterInfo[] tarParams = funMethod.GetParameters();
            Type instanceType = instanceTarget.GetType();
            foreach (MethodInfo method in instanceType.GetMethods())
            {
                var myParams = method.GetParameters();
                if (myParams.Length == tarParams.Length)
                {
                    bool match = true;
                    for (int i = 0, j = myParams.Length; i < j; i++)
                    {
                        if (!myParams[i].ParameterType.Equals(tarParams[i].ParameterType))
                        {
                            match = false;
                            break;
                        }
                    }

                    if (match)
                    {
                        func = (DelegateType)Delegate.CreateDelegate(delegateType, instanceTarget, method.Name, false, true);
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 此方法运行时调用。配置服务容器
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            RT.GetAllModules().ForEach(m =>
            {
                ConfigureServicesExtension extFunc = null;
                if (HasDelegateMethod(m.Instance, out extFunc))
                    extFunc(services, HostingEnvironment, Configuration);
            });

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            //配置文件大小限制
            services.Configure<FormOptions>(options =>
            {
                options.ValueLengthLimit = int.MaxValue;
                options.MultipartBodyLengthLimit = int.MaxValue;
                options.MultipartHeadersLengthLimit = int.MaxValue;
            });

            //启用内存中间件
            services.AddMemoryCache();

            //启用目录浏览中间件
            //services.AddDirectoryBrowser();

            services
                .AddMvc(options =>
                {
                    options.Filters.Add<GlobalExceptionFilterAttribute>();
                })
                .AddNewtonsoftJson(options =>
                {
                    //忽略循环引用
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    //不使用驼峰样式的key
                    options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver();
                    options.SerializerSettings.TypeNameHandling = TypeNameHandling.None;
                    options.SerializerSettings.TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple;
                    options.SerializerSettings.Converters.Add(new DomainJsonConverter());
                    options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
                });
            services.AddConsul(Configuration);
            services.AddCors();
        }

        /// <summary>
        /// 此方法运行时调用。配置http请求管道
        /// </summary>
        /// <param name="app"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IConfiguration configuration)
        {
            RT.GetAllModules().ForEach(m =>
            {
                ConfigureExtension extFunc = null;
                if (HasDelegateMethod(m.Instance, out extFunc))
                    extFunc(app, env, configuration);
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //静态文件映射配置
            //ConfigStaticFileMapping(app, env, configuration);

            //解决Ubuntu Nginx 代理不能获取IP问题
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            //允许跨站调用
            app.UseCors(config => config.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

            //配置路由
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller}/{action}/{id?}", "test/index");
            });
            app.UseConsul();
            app.Run(async context =>
            {
                string page = "";
                if (context.Request.Method == "GET")
                {
                    page = Api.ApiHelpPage.Default.GetHelpPage();
                    context.Response.ContentType = "text/html; charset=utf-8";  //支持中文
                }
                else
                {
                    context.Response.StatusCode = 404;
                }
#pragma warning disable CRR0029 // ConfigureAwait(true) is called implicitly
                await context.Response.WriteAsync(page);
#pragma warning restore CRR0029 // ConfigureAwait(true) is called implicitly
            });

            var provider = app.ApplicationServices.CreateScope().ServiceProvider;
            RT.Service.Register(typeof(IServiceProvider), provider);
        }
    }
    /// <summary>
    /// 全局异常拦截器
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public sealed class GlobalExceptionFilterAttribute : ExceptionFilterAttribute
    {
        readonly SIE.Logging.ILog log = Logging.LogManager.GetLogger("error_logger");
        /// <summary>
        /// 处理异常
        /// </summary>
        /// <param name="context"></param>
        public override void OnException(ExceptionContext context)
        {
            if (!context.ExceptionHandled)
            {
                var sieVersion = AssemblyUtil.GetSieFileVersion();
                var exceptionMsg = "SIE.DLL版本:[{0}], 异常:{1}".FormatArgs(sieVersion, context.Exception.GetExceptionDetail());
                log.Error(exceptionMsg);
                context.Result = new ContentResult
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    ContentType = "text/html;charset=utf-8",
                    Content = exceptionMsg.Replace("\r\n", "<br/>")
                };
                context.ExceptionHandled = true;
            }
        }
    }

}
