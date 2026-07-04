using SIE.TurnoverTools.TurnoverTools;
using SIE.MetaModel.View;

namespace SIE.Web.Elec.MES.TurnoverTools
{
    /// <summary>
    /// 绑定明细视图配置
    /// </summary>
    internal class TurnoverToolBindingViewConfig : WebViewConfig<TurnoverToolBinding>
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
            View.Property(p => p.Sn).ShowInList(width: 150);
            View.Property(p => p.Qty);
            View.Property(p => p.ItemCode).ShowInList(width: 150).HasLabel("物料编码");
            View.Property(p => p.ItemName).ShowInList(width: 150).HasLabel("物料名称");
            View.Property(p => p.WorkOrderId).ShowInList(width: 150).HasLabel("工单号");
            View.Property(p => p.IsBindFinish).Readonly();
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