using SIE.LES.Distributions;
using SIE.MetaModel.View;
using SIE.Web.LES.Distributions.Commands;

namespace SIE.Web.LES.Distributions
{
    /// <summary>
    /// 配送管理
    /// </summary>
    public class DistributionViewConfig : WebViewConfig<Distribution>
    {
        /// <summary>
        /// 视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(typeof(CancelDistributionCommand).FullName, typeof(PrintBillDistributionCommand).FullName);
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsSelection, WebCommandNames.ExportXlsAll);
            View.UseCommands("SIE.Web.LES.Distributions.Commands.DistributionSettingCommand");
            View.DisableEditing();
            using (View.OrderProperties())
            {
                View.Property(p => p.No).ShowInList(120);
                View.Property(p => p.OrderState);
                View.Property(p => p.SourceNo);
                View.Property(p => p.Lpn);
                View.Property(p => p.WarehouseId);
                View.Property(p => p.StorageLocationName).HasLabel("发货库位");
                View.Property(p => p.ProductLineName);
                View.Property(p => p.IsCallAgv);
                View.Property(p => p.DeliveryManName);
                View.Property(p => p.ReceiverName);
                View.Property(p => p.ReceiveDate).ShowInList(150);
                View.Property(p => p.CreateByName).Readonly();
                View.Property(p => p.CreateDate).Readonly();
                View.Property(p => p.UpdateByName).Readonly();
                View.Property(p => p.UpdateDate).Readonly();
                View.ChildrenProperty(p => p.DistributionDetailList);
                View.ChildrenProperty(p => p.DistributionLabelList);
            }
        }
    }
}
