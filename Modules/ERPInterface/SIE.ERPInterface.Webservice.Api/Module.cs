using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SIE.Modules;
using SIE.Web.Interface;
using SoapCore;
using System.ServiceModel;
using SIE.ERPInterface.Webservice.Api;
using SIE.ERPInterface.Webservice.Api.Controller;

[assembly: Module(typeof(Module))]
namespace SIE.ERPInterface.Webservice.Api
{
    /// <summary>
    /// 模块设置
    /// </summary>
    public class Module : UIModule, IStartupExtension
    {
        public void ConfigServices(IServiceCollection services, IWebHostEnvironment env)
        {
            services.TryAddSingleton<InfWebservice>();
            services.TryAddSingleton<SmomWebservice>();
            services.AddMvc(x => x.EnableEndpointRouting = false);
            services.AddSoapCore();
        }

        public void ConfigureApp(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSoapEndpoint<InfWebservice>("/SIE/INFService.svc", new BasicHttpBinding(), SoapSerializer.DataContractSerializer);
            app.UseSoapEndpoint<InfWebservice>("/SIE/INFService.asmx", new BasicHttpBinding(), SoapSerializer.XmlSerializer);
            app.UseSoapEndpoint<SmomWebservice>("/SIE/SMOMService.svc", new BasicHttpBinding(), SoapSerializer.DataContractSerializer);
            app.UseSoapEndpoint<SmomWebservice>("/SIE/SMOMService.asmx", new BasicHttpBinding(), SoapSerializer.XmlSerializer);
            app.UseMvc();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="app">app</param>
        public override void Initialize(IApp app)
        {
        }
    }
}
