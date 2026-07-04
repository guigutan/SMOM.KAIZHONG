using SIE.Configuration;
using SIE.Domain;
using System;

namespace SIE.xUnit.Core
{
    /// <summary>
    /// 程序启动，用于初始化DomainApp
    /// </summary>
    public class TestStarup
    {
        /// <summary>
        /// 静态构造函数
        /// </summary>
        static TestStarup()
        {
            ConfigManager.Create().UserJsonConfig("appsettings.json");
            var _app = new DomainApp();
            _app.AllModulesIntialized += (s, e) =>
            {
                AppRuntime.Location.IsWebUI = true;
                AppRuntime.Location.IsWPFUI = false;
                AppRuntime.Location.IsTest = false;
            };
            _app.Startup();

#if DEBUG
            SIE.Data.DbAccesserFactory.DbCommandPrepared += (s, e) =>
            {
                string sqlDebug = e.DbCommand.ToTraceString();
                System.Diagnostics.Debug.WriteLine(sqlDebug);
            };
#endif
        }

        ////SingleTransactionScope scope;

        //public TestStarup()
        //{
        //    scope = DB.TransactionScope(WcsEntityDataProvider.ConnectionStringName);
        //}

        //~TestStarup()
        //{
        //    scope.Dispose();
        //}
    }
}