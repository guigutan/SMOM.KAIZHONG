using SIE.Items;
using SIE.MES.WorkOrders;

namespace SIE.Wpf.MES.WorkOrders
{
    /// <summary>
    /// 工单BOM替代料视图配置
    /// </summary>
    internal class WorkOrderBomAlternativeViewConfig : WPFViewConfig<WorkOrderBomAlternative>
    {
        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Item).UsePagingLookUpEditor(p => p.DisplayMember = Item.CodeProperty.Name).Show(ShowInWhere.All).HasLabel("物料编码");
                View.Property(p => p.ItemName).Show(ShowInWhere.All).HasLabel("物料名称");
            }
        }
    }
}
