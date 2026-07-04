using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using SIE.Modules;
using SIE.Runtime;
using SIE.Web.Configs;
using SIE.Web.Handlers;
using SIE.Web.Interface;
using SIE.Web.WorkBenchCore.Mvc;
using System;

[assembly: Module(typeof(Module))]

namespace SIE.Web.WorkBenchCore.Mvc
{
    public class Module : UIModule, IStartupExtension
    {
        public void ConfigServices(IServiceCollection services, IWebHostEnvironment env)
        {
           
        }

        public void ConfigureApp(IApplicationBuilder app, IWebHostEnvironment env)
        {

        }

        public override void Initialize(IApp app)
        {
            app.ModuleOperations += App_ModuleOperations;
            JavascriptHandler_Modules.JsResourcesFilterModules.Add(GetType());
            WebResourceConfig.AddResourceEmbeddedModule(GetType());
            WebResourceConfig.AddFilterModule(GetType());
            var client = app as IClientApp;
            client.LoginSuccessed += (sender, e) =>
            {

            };
        }

        void App_ModuleOperations(object sender, EventArgs e)
        {

        }
    }
}
