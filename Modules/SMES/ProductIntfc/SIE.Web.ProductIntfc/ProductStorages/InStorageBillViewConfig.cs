using SIE.MetaModel.View;
using SIE.ProductIntfc.ProductStorages;
using SIE.Web.Common.Configs.Commands;

namespace SIE.Web.ProductIntfc.ProductStorages
{
    /// <summary>
    /// 入库单
    /// </summary>
    public class InStorageBillViewConfig : WebViewConfig<InStorageBill>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands(true).UseCommands(WebCommandNames.ExportXls);
            View.UseChildrenAsHorizontal(true);
            using (View.OrderProperties())
            {
                View.Property(p => p.No).Readonly().ShowInList(150);
                View.Property(p => p.CreateDate).Readonly().Show(ShowInWhere.All);
                View.Property(p => p.AsnNo).Readonly().ShowInList(150).HasLabel("ASN单号");
                View.Property(p => p.ReceiveState).Readonly().Show(ShowInWhere.All).HasLabel("接收情况");
                View.Property(p => p.ReceiveDate).Readonly().ShowInList(150);
                View.Property(p => p.Warehouse).Readonly().Show(ShowInWhere.All).HasLabel("收货仓库");
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
                View.ChildrenProperty(p => p.InStorageBarcodeDetailList).Show(ChildShowInWhere.All);
            }
        }
    }
}
