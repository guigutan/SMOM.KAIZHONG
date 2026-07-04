using SIE.TurnoverTools.TurnoverTools;
using SIE.MetaModel.View;

namespace SIE.Web.Elec.MES.TurnoverTools
{
    /// <summary>
    /// 操作日志视图配置
    /// </summary>
    internal class TurnoverToolActionLogViewConfig : WebViewConfig<TurnoverToolActionLog>
    {
        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.UseDefaultCommands();
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.DisableEditing();
            View.ClearCommands();
            View.UseCommand(WebCommandNames.ExportXls);
            View.Property(p => p.TurnoverToolAction);
            View.Property(p => p.Sn);
            View.Property(p => p.Qty);
            View.Property(p => p.ItemCode);
            View.Property(p => p.ItemName);
            View.Property(p => p.WorkOrderNo);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
        }

        ///<summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            // 配置默认视图
        }

        ///<summary>
        /// 配置下拉视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            // 配置默认视图
        }
    }
}