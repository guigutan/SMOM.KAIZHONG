using SIE.MES.WorkOrders;
using SIE.MES.WorkOrders.Reworks;

namespace SIE.Web.MES.WorkOrders.Reworks
{
    /// <summary>
    /// 关联条码视图配置
    /// </summary>
    internal class UnionBarcodeViewConfig : WebViewConfig<UnionBarcode>
    {

        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(WorkOrder));
        }

        /// <summary>
        /// List View
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseChildrenAsHorizontal();
            View.UseGridSelectionModel();
            View.UseCommands("SIE.Web.MES.WorkOrders.UnionBarcodeSelCommand", "SIE.Web.MES.WorkOrders.Reworks.RemoveBarcodeCommand");
            using (View.OrderProperties())
            {
                View.Property(p => p.ReworkBarcode).ShowInList(150).Readonly();
                View.Property(p => p.OriginalBarcode).ShowInList(150).Readonly();
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