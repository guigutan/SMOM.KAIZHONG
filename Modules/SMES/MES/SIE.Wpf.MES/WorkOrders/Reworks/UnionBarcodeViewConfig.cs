using SIE.MES.WorkOrders;
using SIE.MES.WorkOrders.Reworks;

namespace SIE.Wpf.MES.WorkOrders.Reworks
{
    /// <summary>
    /// 关联条码视图配置
    /// </summary>
    internal class UnionBarcodeViewConfig : WPFViewConfig<UnionBarcode>
    {
        /// <summary>
        /// List View
        /// </summary>
        protected override void ConfigListView()
        {
            View.AssignAuthorize(typeof(WorkOrder));
            View.UseChildrenAsHorizontal();
            View.AddBehavior(typeof(UnionBarcodeViewBehavior));
            View.UseCommands(typeof(UnionBarcodeAllSelectCommand), typeof(RemoveBarcodeCommand), typeof(UnionBarcodeSelCommand))
                .UseLayoutSize(-1, -1);
            using (View.OrderProperties())
            {
                View.Property(p => p.ReworkBarcode).UseListSetting(w => w.ListGridWidth = 150).Readonly();
                View.Property(p => p.OriginalBarcode).UseListSetting(w => w.ListGridWidth = 150).Readonly();
                View.Property(p => p.CodeState).Readonly();
                View.Property(p => p.CreateBy).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateBy).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }
    }
}