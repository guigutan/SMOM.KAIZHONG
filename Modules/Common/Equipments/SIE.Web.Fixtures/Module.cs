using SIE.Fixtures.FixtureDemands;
using SIE.Fixtures.FixtureRecords;
using SIE.Fixtures.Fixtures.Abnormals;
using SIE.Fixtures.Fixtures.Accounts;
using SIE.Fixtures.InboundOrders;
using SIE.Fixtures.MaintainTasks;
using SIE.Fixtures.Models;
using SIE.Fixtures.Projects;
using SIE.Fixtures.Querys.ViewModels;
using SIE.Fixtures.Repairs;
using SIE.Fixtures.Warns;
using SIE.MetaModel;
using SIE.Modules;
using SIE.Web.Fixtures;
using SIE.Web.Fixtures.Accounts;
using System;

[assembly: Module(typeof(Module))]
namespace SIE.Web.Fixtures
{
    /// <summary>
    /// 模块配置
    /// </summary>
    public class Module : UIModule
    {
        /// <summary>
        /// 初始化模块
        /// </summary>
        /// <param name="app">应用程序</param>
        public override void Initialize(IApp app)
        {
            app.ModuleOperations += App_ModuleOperations;
        }

        /// <summary>
        /// 模块定义
        /// </summary>
        /// <param name="sender">应用程序</param>
        /// <param name="e">参数</param>
        private void App_ModuleOperations(object sender, EventArgs e)
        {
            CommonModel.Modules.AddModules(new WebModuleMeta()
            {
                EntityType = typeof(FixtureAbnormal),
                Label = "工治具异常类型"
            }, new WebModuleMeta()
            {
                EntityType = typeof(FixtureModel),
                Label = "工治具型号"
            }, new WebModuleMeta()
            {
                EntityType = typeof(MaintainProject),
                Label = "工治具保养项目"
            }, new WebModuleMeta()
            {
                EntityType = typeof(FixtureAccountStock),
                Label = "工治具库存台账"
            }, new WebModuleMeta()
            {
                EntityType = typeof(FixtureEncode),
                Label = "工治具编码"
            }, new WebModuleMeta()
            {
                EntityType = typeof(FixtureAccountModel),
                Label = "工治具台账",
                BlocksTemplate = typeof(FixtureAccountModelUITemplate)
            }, new WebModuleMeta()
            {
                EntityType = typeof(SIE.Fixtures.FixtureTypes.FixtureType),
                Label = "工治具类型"
            },
            new WebModuleMeta()
            {
                EntityType = typeof(InboundOrder),
                Label = "工治具入库"
            },
             new WebModuleMeta()
             {
                 EntityType = typeof(FixtureDemand),
                 Label = "工治具需求清单"
             },
             new WebModuleMeta()
             {
                 EntityType = typeof(FixtureRepair),
                 Label = "工治具报修"
             }, new WebModuleMeta()
             {
                 EntityType = typeof(FixtureQueryViewModel),
                 Label = "工治具查询"
             }, new WebModuleMeta()
             {
                 EntityType = typeof(FixtureWarn),
                 Label = "工治具保养预警"
             }, new WebModuleMeta()
             {
                 EntityType = typeof(MaintainTask),
                 Label = "工治具保养任务"
             }, new WebModuleMeta()
             {
                 EntityType = typeof(FixtureRecord),
                 Label = "工治具出入库记录"
             }
            );
        }
    }
}
