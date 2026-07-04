using SIE.MES.DashBoard.Reports.LineFPY;
using SIE.MES.DashBoard.Reports.ProductFPY;
using SIE.MES.DashBoard.Reports.ShopFPY;
using SIE.MES.DashBoard.WorkOrderReachs;
using SIE.MetaModel;
using SIE.Modules;
using SIE.Wpf.MES.DashBoard;
using SIE.Wpf.MES.DashBoard.Reports.Commons;
using SIE.Wpf.MES.DashBoard.WorkOrderReachs;
using System;

[assembly: Module(typeof(Module))]
namespace SIE.Wpf.MES.DashBoard
{
    /// <summary>
    /// 模块定义
    /// </summary>
    public class Module : UIModule
    {
        /// <summary>
        /// 模块初始化
        /// </summary>
        /// <param name="app">应用程序</param>
        public override void Initialize(IApp app)
        {
            app.ModuleOperations += App_ModuleOperations;
        }

        /// <summary>
        /// 模块操作
        /// </summary>
        /// <param name="sender">发送者</param>
        /// <param name="e">事件参数</param>
        private void App_ModuleOperations(object sender, EventArgs e)
        {
            CommonModel.Modules.AddModules(new WPFModuleMeta()
            {
                Label = "车间直通率报表",
                EntityType = typeof(ShopReportViewModel),
                BlocksTemplate = typeof(ReportCommonTemplate),
                ViewGroup = ViewConfig.ListView,
                TryAutoLoadData = true
            }, new WPFModuleMeta()
            {
                Label = "产线直通率报表",
                EntityType = typeof(LineReportViewModel),
                BlocksTemplate = typeof(ReportCommonTemplate),
                ViewGroup = ViewConfig.ListView,
                TryAutoLoadData = true
            }, new WPFModuleMeta()
            {
                Label = "产品直通率报表",
                EntityType = typeof(ProductReportViewModel),
                BlocksTemplate = typeof(ReportCommonTemplate),
                ViewGroup = ViewConfig.ListView,
                TryAutoLoadData = true
            }, new WPFModuleMeta()
            {
                Label = "工单准时达成率报表",
                EntityType = typeof(WoReachReportViewModel),
                BlocksTemplate = typeof(WorkOrderReachTemplate),
                ViewGroup = ViewConfig.ListView,
                TryAutoLoadData = true
            });
        }
    }
}