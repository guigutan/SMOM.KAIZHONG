using SIE.MES.DashBoard.DashBoards.WorkShop;
using SIE.MES.DashBoard.KzBoard.RegionBoards;
using SIE.MES.DashBoard.KzReport.OrganizeCodes;
using SIE.MES.DashBoard.KzReport.ProductionLineProcesss;
using SIE.MES.DashBoard.KzReport.ProductionProcesss;
using SIE.MES.DashBoard.Reports.LineFPY;
using SIE.MES.DashBoard.Reports.ProductFPY;
using SIE.MES.DashBoard.Reports.ShopFPY;
using SIE.MES.DashBoard.TeamManagement;
using SIE.MES.DashBoard.WorkOrderReachs;
using SIE.MetaModel;
using SIE.Modules;
using SIE.Web.Configs;
using SIE.Web.MES.DashBoard;
using SIE.Web.MES.DashBoard.Reports.LineFPY;
using SIE.Web.MES.DashBoard.Reports.ProductFPY;
using SIE.Web.MES.DashBoard.Reports.ShopFPY;
using SIE.Web.MES.DashBoard.TeamManagement;
using SIE.Web.MES.DashBoard.WorkOrderReachs;
using System;

[assembly: Module(typeof(Module))]

namespace SIE.Web.MES.DashBoard
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
            app.MetaCompiled += App_MetaCompiled;
            WebResourceConfig.AddFilterModule(GetType());
        }

        /// <summary>
        /// 模块定义
        /// </summary>
        /// <param name="sender">应用程序</param>
        /// <param name="e">参数</param>
        private void App_ModuleOperations(object sender, EventArgs e)
        {
            CommonModel.Modules.AddModules(
              new WebModuleMeta()
              {
                  Label = "评分统计表".L10N(),
                  EntityType = typeof(ScoreRecordViewModel),
                  BlocksTemplate = typeof(ScoreRecordTemplate)
              }, new WebModuleMeta()
              {
                  Label = "产线直通率报表".L10N(),
                  EntityType = typeof(LineReportViewModel),
                  BlocksTemplate = typeof(LineReportTemplate)
              }, new WebModuleMeta()
              {
                  Label = "工单准时达成率报表".L10N(),
                  EntityType = typeof(WoReachReportViewModel),
                  BlocksTemplate = typeof(WorkOrderReachTemplate),
              }, new WebModuleMeta()
              {
                  Label = "产品直通率报表".L10N(),
                  EntityType = typeof(ProductReportViewModel),
                  BlocksTemplate = typeof(ProdReportTemplate)
              }, new WebModuleMeta()
              {
                  Label = "车间直通率报表".L10N(),
                  EntityType = typeof(ShopReportViewModel),
                  BlocksTemplate = typeof(ShopReportTemplate)
              }, new WebModuleMeta()
              {
                  Label = "安全生产天数".L10N(),
                  EntityType = typeof(WorkSafety)
              }, new WebModuleMeta()
              {
                  Label = "区域与产线的关联关系".L10N(),
                  EntityType = typeof(RegionBoard)
              }, new WebModuleMeta()
              {
                  Label = "产品线工序".L10N(),
                  EntityType = typeof(ProductionLineProcess)
              }, new WebModuleMeta()
              {
                  Label = "组织代码".L10N(),
                  EntityType = typeof(OrganizeCode)
              }, new WebModuleMeta()
              {
                  Label = "产能利用率工序",
                  EntityType = typeof(ProductionProcess)
              }, new WebModuleMeta()
              {
                  Label = "产能资源工序",
                  EntityType = typeof(CapacityResource)
              }
              );
        }

        /// <summary>
        /// 添加属性编辑器
        /// </summary>
        /// <param name="app">应用程序</param>
        private void App_MetaCompiled(object sender, EventArgs e)
        {
            Page82Config.Add(typeof(ScoreRecordViewModel).FullName, Page82.Page2);
            Page82Config.Add(typeof(LineReportViewModel).FullName, Page82.Page2);
            Page82Config.Add(typeof(WoReachReportViewModel).FullName, Page82.Page2);
            Page82Config.Add(typeof(ProductReportViewModel).FullName, Page82.Page2);
            Page82Config.Add(typeof(ShopReportViewModel).FullName, Page82.Page2);
        }
    }
}