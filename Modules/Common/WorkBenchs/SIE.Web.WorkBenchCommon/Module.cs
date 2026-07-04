using SIE.MetaModel;
using SIE.Modules;
using SIE.Web.WorkBenchCommon;
using SIE.WorkBenchCommon.Workbench.Base;
using SIE.WorkBenchCommon.Workbench.KPI;
using SIE.WorkBenchCommon.Workbench.TargetWarn;
using System;
using System.Collections.Generic;
using System.Text;

[assembly: Module(typeof(Module))]
namespace SIE.Web.WorkBenchCommon
{
    /// <summary>
    /// 当前工程所对应的模块类。
    /// </summary>
    class Module : SIE.Modules.UIModule
    {
        public override void Initialize(IApp app)
        {

            app.ModuleOperations += (o, e) =>
            {
                CommonModel.Modules.AddModules(
                    new WebModuleMeta()
                    {
                        Label = "布局管理".L10N(),
                        EntityType = typeof(LayoutInfo),

                    },
                     new WebModuleMeta()
                     {
                         Label = "组件管理".L10N(),
                         EntityType = typeof(ComponentInfo),

                     },
                     new WebModuleMeta()
                     {
                         Label = "工作台定义".L10N(),
                         EntityType = typeof(WorkbenchDefinition),

                     },
                     new WebModuleMeta()
                     {
                         Label = "KPI目标设定".L10N(),
                         EntityType = typeof(QuotaTargetSetting)

                     },
                     new WebModuleMeta()
                     {
                         Label = "目标达成率预警设定".L10N(),
                         EntityType = typeof(TargetWarnSetting)
                     }
                );
            };
        }


    }
}
