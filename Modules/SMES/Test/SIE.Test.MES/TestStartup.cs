using SIE.Configuration;
using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Test.MES
{
    public class TestStartup
    {
        /// <summary>
        /// 静态构造函数
        /// </summary>
        static TestStartup()
        {
            ConfigManager.Create().UserJsonConfig("appsettings.json");
            var _app = new DomainApp();
            _app.AllModulesIntialized += (s, e) =>
            {
                AppRuntime.Location.IsWebUI = false;
                AppRuntime.Location.IsWPFUI = false;
                AppRuntime.Location.IsTest = true;
            };
            _app.Startup();
        }
    }
}
