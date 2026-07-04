using SIE.Inventory.TransactionProcessing;
using SIE.MetaModel.View;
using System;

namespace SIE.Web.Inventory.TransactionProcessing
{
    /// <summary>
    /// 库存交易视图配置
    /// </summary>
    internal class InvTransactionViewConfig : WebViewConfig<InvTransaction>
    {
        /// <summary>
        /// 子类重写此方法，并完成对 Meta 属性的配置。
        /// 注意：
        /// * 为了给当前类的子类也运行同样的配置，这个方法可能会被调用多次。
        /// </summary>
        protected override void ConfigView()
        {
            View.DomainName("流水账报表");
            View.ClearCommands();
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsSelection, WebCommandNames.ExportXlsAll);
            View.DisableEditing();
            View.Property(p => p.TransactionDate).FixColumn().UseDateEditor(p => p.Format = "Y-m-d H:i:s").ShowInList(150);
            View.Property(p => p.TransactionType).FixColumn();
            View.Property(p => p.BillNo).FixColumn().ShowInList(150);
            View.Property(p => p.BillDtlNo);
            View.Property(p => p.TransactionType);
            View.Property(p => p.ItemCode).ShowInList(150);
            View.Property(p => p.ItemName).ShowInList(150);
            View.Property(p => p.ItemSpecificationModel).HasLabel("规格型号");
            View.Property(p => p.ItemExtPropName).ShowInList(width: 185);
            View.Property(p => p.Qty);
            View.Property(p => p.UnitName).HasLabel("单位(主)");
            View.Property(p => p.LotCode);
            View.Property(p => p.FromWarehouse);
            View.Property(p => p.FromLocation);
            View.Property(p => p.FromLpn);
            View.Property(p => p.FromOnhandSate);
            View.Property(p => p.ToWarehouse);
            View.Property(p => p.ToLocation);
            View.Property(p => p.ToLpn);
            View.Property(p => p.ToOnhandState);
            View.Property(p => p.StorerCode);
            View.Property(p => p.ProjectNo);
            View.Property(p => p.TaskNo);
            View.Property(p => p.OrderType);
            View.Property(p => p.Transaction);
            View.Property(p => p.SecondDtlLineNo);
            View.Property(p => p.UploadFlag).ShowInList().UseListSetting(p => p.HelpInfo = "事务已上传到中间表标记".L10N());
            View.Property(p => p.ToStorerCode);
            View.Property(p => p.Remark);
            View.Property(p => p.SourceBillNo);
            View.Property(p => p.SourceBillId);
            View.Property(p => p.SourceBillDtlNo);
            View.Property(p => p.SourceBillDtlId);
            View.Property(p => p.PoNo);
            View.Property(p => p.PoLineNo);
            View.Property(p => p.PodistributionId);
            View.Property(p => p.PoLinelocationId);
            View.Property(p => p.OrderNo);
            View.Property(p => p.CustomerName);
            View.Property(p => p.Supplier);
            View.Property(p => p.Enterprise);
            View.Property(p => p.Reason);
            View.Property(p => p.SpecialTransMark);
            View.Property(p => p.PurchaseQty);
            View.Property(p => p.PurchaseUnit);
            View.Property(p => p.ErpWarehouseCode);
            View.Property(p => p.ErpOrganizationName);
            View.Property(p => p.ErpAccount);
            View.Property(p => p.TargetErpWarehouseCode);
            View.Property(p => p.Id).HasLabel("ID").ShowInList(60);
            View.Property(p => p.CreateDate).UseDateEditor(p => p.Format = "Y-m-d H:i:s");
        }
    }
}
