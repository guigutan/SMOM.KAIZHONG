using SIE.MES.DashBoard.Reports.LineFPY;
using SIE.MetaModel.View;
using SIE.Resources.WipResources;
using System.Collections.Generic;

namespace SIE.Web.MES.DashBoard.Reports.LineFPY
{
    /// <summary>
    /// 产线报表查询实体视图配置
    /// </summary>
    internal class LineReportCriteriaViewConfig : WebViewConfig<LineReportViewModelCriteria>
    {
        /// <summary>
        /// 默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.UseDefaultCommands();
            View.ReplaceCommands(WebCommandNames.ExecuteQuery, "SIE.Web.MES.DashBoard.Reports.LineFPY.Commands.LineReportCriteriaCommand");
            using (View.OrderProperties())
            {
                View.Property(p => p.Resource).UseDataSource((e, c, r) =>
                {
                    var stateList = new List<ResourceState>() { ResourceState.Actived, ResourceState.Stop, ResourceState.Unused };
                    return RT.Service.Resolve<WipResourceController>().GetWipResources(stateList, new List<SyncSourceType>() { SyncSourceType.Enterprise }, c, r);
                }).HasLabel("资源").Show().UseListSetting(e => { e.HelpInfo = "显示不失效且来源类型为企业模型的生产资源"; });
                View.Property(p => p.CollectDate).HasLabel("日期").UseDateRangeEditor(p =>
                {
                    p.DateFormat = "Y/m/d";
                    p.DateRangeType = ObjectModel.DateRangeType.Month;
                }).Show();
            }
        }
    }
}
