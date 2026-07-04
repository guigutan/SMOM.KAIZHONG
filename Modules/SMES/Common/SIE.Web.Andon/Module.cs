using SIE.Andon.AndonAverageTime;
using SIE.Andon.AndonMonthReports;
using SIE.Andon.Andons;
using SIE.Andon.AndonStatisticsReports;
using SIE.MetaModel;
using SIE.Modules;
using SIE.Web.Andon;
using SIE.Web.Andon.AndonAverageTime;
using SIE.Web.Andon.AndonMonthReports;
using SIE.Web.Andon.AndonStatisticsReports;
using SIE.Web.Configs;
using System;

[assembly: Module(typeof(Module))]
namespace SIE.Web.Andon
{
    /// <summary>
    /// 模块定义
    /// </summary>
    public class Module : UIModule
    {
        /// <summary>
        /// 初始化模块
        /// </summary>
        /// <param name="app"></param>
        public override void Initialize(IApp app)
        {
            app.ModuleOperations += this.App_ModuleOperations;
            WebResourceConfig.AddFilterModule(GetType());
        }
        private void App_ModuleOperations(object sender, EventArgs e)
        {
            CommonModel.Modules.AddModules(
                new WebModuleMeta()
                {
                    EntityType = typeof(AndonType),
                    Label = "安灯类型维护",
                },
                new WebModuleMeta()
                {
                    EntityType = typeof(SIE.Andon.Andons.Andon),
                    Label = "安灯维护",
                },
                new WebModuleMeta()
                {
                    EntityType = typeof(AndonManage),
                    Label = "安灯管理",
                },
                new WebModuleMeta()
                {
                    EntityType = typeof(AndonExperience),
                    Label = "安灯经验库",
                },
                new WebModuleMeta()
                {
                    Label = "安灯统计报表".L10N(),
                    EntityType = typeof(AndonStatisticsViewModel),
                    BlocksTemplate = typeof(AndonStatisticsUITemplate)
                },
                new WebModuleMeta()
                {
                    Label = "安灯统计报表(月度)".L10N(),
                    EntityType = typeof(AndonMonthReportViewModel),
                    BlocksTemplate = typeof(AndonMonthUITemplate)
                },
                new WebModuleMeta()
                { 
                    EntityType = typeof(AndonGroup),
                    Label = "安灯责任组维护基础表"
                }, new WebModuleMeta()
                {
                    Label = "安灯响应平均时长".L10N(),
                    EntityType = typeof(AndonAverageTimeViewModel),
                    //BlocksTemplate = typeof(AndonAverageTimeUITemplate) 弃用
                }
                );
        }
    }
}
