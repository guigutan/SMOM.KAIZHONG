using Newtonsoft.Json;
using SIE.Configuration;
using SIE.Domain;
using SIE.Domain.Serialization.Json;
using SIE.Security;
using System;

namespace SIE.UT.MES
{
    /// <summary>
    /// 
    /// </summary>
    public class AppInit : IDisposable
    {
        readonly DomainApp app;
        public AppInit()
        {
            JsonConvert.DefaultSettings = () =>
            {
                var setting = new JsonSerializerSettings();
                setting.TypeNameHandling = TypeNameHandling.Auto;
                setting.TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple;
                setting.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                setting.Converters.Add(new DomainJsonConverter());
                setting.DateTimeZoneHandling = DateTimeZoneHandling.Local;
                return setting;
            };
            DomainJsonSerializerSettings.Default = new DomainJsonSerializerSettings(JsonConvert.DefaultSettings)
            {
                ReferenceListHandling = ReferenceListHandling.All,
            };
            ConfigManager.Create().UserJsonConfig("appsettings.json");
            app = new DomainApp();
            app.Startup();
            var service = RT.Service.Resolve<IAuthenticationService>();
            ////service.Login("zh", "", new LoginInfo());
            service.Login("HCQ", "666666", new LoginInfo());
        }



        public void Dispose()
        {
            app.NotifyExit();
        }
    }
}
