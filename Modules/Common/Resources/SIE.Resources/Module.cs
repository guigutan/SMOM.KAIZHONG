using SIE.Common.Employees;
using SIE.Core.Algorithms.KZ;
using SIE.Domain;
using SIE.EventMessages.Resources.WipResources;
using SIE.Modules;
using SIE.ObjectModel;
using SIE.Resources;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

[assembly: Module(typeof(Module))]

namespace SIE.Resources
{
    /// <summary>
    /// 业务相关的资源，人、企业、组织、设备等
    /// </summary>
    internal class Module : DomainModule
    {
        /// <summary>
        /// 初始化程序集
        /// </summary>
        /// <param name="app">IApp</param>
        public override void Initialize(IApp app)
        {
            RT.Service.Register<CriteriaProvider>();
            RT.Service.Register<IEmployee, SIE.Resources.Employees.EmployeeController>();
            RT.Service.Register<IWipResources, WipResourceController>();
            app.StartupCompleted += App_StartupCompleted;
        }

        private void App_StartupCompleted(object sender, EventArgs e)
        {
            if (!AppRuntime.Location.IsTest)
                InitWipResSynSettings();
        }

        /// <summary>
        /// 初始化资源同步配置
        /// </summary>
        private void InitWipResSynSettings()
        {
            Task.Run(() =>
            {
             
                 var synWipResSettingList = RF.GetAll<SynWipResSetting>();
                if (synWipResSettingList.Count > 0) return;
                Stopwatch watch = new Stopwatch();
                watch.Start();
                foreach (var module in RT.GetAllModules())
                {
                    if (new List<string> { "SIE", "SIE.Rbac", "SIE.Common" }.Contains(module.Assembly.GetName().Name)) continue;

                    foreach (var type in module.Assembly.GetTypes())
                    {
                        if (typeof(ISyncRsource).IsAssignableFrom(type) && !type.IsAbstract &&
                            !synWipResSettingList.Any(p => p.Type == type.FullName))
                        {
                            var setting = new SynWipResSetting
                            {
                                Name = string.Join(";", type.GetCustomAttributes(typeof(LabelAttribute), false).OfType<LabelAttribute>().Select(p => p.Label)).TrimStart(';'),
                                Type = type.FullName,
                                AssenblyName = type.Assembly.FullName,
                                IsSyn = false,
                            };

                            synWipResSettingList.Add(setting);
                        }
                    }
                }

                RF.Save(synWipResSettingList);
                watch.Stop();
                Debug.Write($"**************{watch.ElapsedMilliseconds}*******************");
            });
        }
    }
}
