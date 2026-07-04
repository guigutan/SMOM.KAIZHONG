using SIE.ProductIntfc.OutputProducts;
using SIE.Web.Items._Extentions_;

namespace SIE.Web.ProductIntfc.OutputProducts
{
    /// <summary>
    /// 入库单
    /// </summary>
    public class OutputProductRecordViewModelViewConfig : WebViewConfig<OutputProductRecordViewModel>
    {
        /// <summary>
        /// 
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(OutputProduct));
            base.ConfigView();
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            //View.DisableEditing();
            using (View.OrderProperties())
            {
                View.Property(p => p.WorkOrderNo).Readonly().ShowInList(150);
                View.Property(p => p.ProductCode).Readonly().ShowInList(150);
                View.Property(p => p.ProductName).Readonly().ShowInList(150);
                View.Property(p => p.ItemCode).Readonly().ShowInList(150);
                View.Property(p => p.ItemName).Readonly().ShowInList(150);
                View.Property(p => p.Qty).Readonly().Show(ShowInWhere.All);
                View.Property(p => p.SubmitQty).Readonly().Show();
                View.Property(p => p.InputQty).UseSpinEditor(p => { p.MinValue = 0; }).Show(ShowInWhere.All);
                View.Property(p => p.TotalQty).Readonly().Show();

            }
        }
    }
}
