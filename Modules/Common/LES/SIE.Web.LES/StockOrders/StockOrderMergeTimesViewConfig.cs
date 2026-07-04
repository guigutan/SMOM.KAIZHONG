using SIE.LES.StockOrders;
using SIE.MetaModel.View;
using SIE.Web.LES.StockOrders.Commands;

namespace SIE.Web.LES.StockOrders
{
    /// <summary>
    /// 合并时间段视图配置
    /// </summary>
    internal class StockOrderMergeTimesViewConfig : WebViewConfig<StockOrderMergeTimes>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.WithoutPaging();
            View.AddBehavior("SIE.Web.LES.StockOrders.Scripts.StockOrderMergeTimesBehavior");
            View.ClearCommands();
            View.UseCommands(typeof(StockOrderMergeTimesAddCommand).FullName, typeof(StockOrderMergeTimesEditCommand).FullName, typeof(StockOrderMergeTimesDeleteCommand).FullName);
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsSelection, WebCommandNames.ExportXlsAll);
            View.Property(p => p.StartTimeText).UseTextEditor().Readonly(p => p.State == Domain.State.Enable).UseListSetting(p => p.HelpInfo = "时间格式HH:mm:ss").ShowInList(120);
            View.Property(p => p.EndTimeText).UseTextEditor().Readonly(p => p.State == Domain.State.Enable).UseListSetting(p => p.HelpInfo = "时间格式HH:mm:ss").ShowInList(120);
            View.Property(p => p.IsCrossDay).Readonly();
            View.Property(p => p.Remark).Readonly(p => p.State == Domain.State.Enable);
        }
    }

}
