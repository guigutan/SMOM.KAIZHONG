using DevExpress.Data.Mask;
using SIE.ERPInterface.Common.Enums;
using SIE.ERPInterface.Common.Logs;
using SIE.MetaModel.View;
using SIE.Web.ERPInterface.Logs.Commands;
using System.Collections.Generic;

namespace SIE.Web.ERPInterface.Logs
{
    /// <summary>
    /// 事务上传视图配置
    /// </summary>
    internal class UploadTransactionViewConfig : WebViewConfig<UploadTransaction>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommand(typeof(ReUploadCommand).FullName);
            View.UseCommand(typeof(AbandonCommand).FullName);
            View.UseCommand(typeof(AdjustTxnDateCommand).FullName);
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll);
            //View.UseCommand(typeof(AdjustTxnDateCommand).FullName);

            View.Property(p => p.TransactionDate).Readonly().ShowInList(150);
            View.Property(p => p.OrderType).Readonly();
            //View.Property(p => p.TransactionCode).Readonly();
            //View.Property(p => p.TransactionName).Readonly();
            View.Property(p => p.TransactionType).Readonly();
            View.Property(p => p.State).Readonly();
            View.Property(p => p.ProcessMessage).Readonly();
            View.Property(p => p.ValidateMessage).Readonly();

            View.Property(p => p.LotCode).Readonly();
            View.Property(p => p.Quantity).Readonly();
            View.Property(p => p.OkQty).Readonly();
            View.Property(p => p.NgQty).Readonly();
            View.Property(p => p.ReworkQty).Readonly();
            //View.Property(p => p.BillNo).Readonly();
            //View.Property(p => p.BillLineNo).Readonly();
            View.Property(p => p.ItemCode).Readonly();
            View.Property(p => p.ItemName).Readonly();
            View.Property(p => p.ShortDescription).Readonly();
            View.Property(p => p.Bismt).Readonly();
            //View.Property(p => p.PoNo).Readonly();
            //View.Property(p => p.PoLineNo).Readonly();
            View.Property(p => p.WoNo).Readonly();
            View.Property(p => p.WorkShopCode).Show().Readonly();
            View.Property(p => p.WorkShopName).Show().Readonly();
            View.Property(p => p.Vornr).Readonly();
            View.Property(p => p.ProcessCode).Readonly();
            View.Property(p => p.WorkCenter).Show().Readonly();

            //View.Property(p => p.FromWarehouseCode).Readonly();
            //View.Property(p => p.FromWarehouseName).Readonly();
            //View.Property(p => p.FromLocationCode).Readonly();
            //View.Property(p => p.FromLocationName).Readonly();
            //View.Property(p => p.ToWarehouseCode).Readonly();
            //View.Property(p => p.ToWarehouseName).Readonly();
            View.Property(p => p.ToLocationCode).Show().Readonly();
            //View.Property(p => p.ToLocationName).Readonly();
            //View.Property(p => p.InvTransactionId).Readonly().HasLabel("事务交易ID");
            View.Property(p => p.UploadCount).Show().Readonly();
            View.Property(p => p.Zuid).Readonly().ShowInList(200);
            View.Property(p => p.Mblnr).Readonly().ShowInList(200);
            View.Property(p => p.Mjahr).Readonly().ShowInList(200);
        }

        /// <summary>
        /// 配置表单视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.TransactionDate);
        }

        ///<summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.OrderType).UseEnumEditor("KZ");
            View.Property(p => p.TransactionCode);
            View.Property(p => p.TransactionType).UseEnumEditor("KZ");
            //View.Property(p => p.State);
            View.Property(p => p.State).UseEnumMutilEditor(x => x.EnumType = typeof(ProcessState)).DefaultValue("Processing").Show(ShowInWhere.Detail);
            //View.Property(p => p.BillNo);
            View.Property(p => p.ItemCode);
            View.Property(p => p.ItemName);
            //View.Property(p => p.PoNo);
            View.Property(p => p.WoNo);
            View.Property(p => p.LotCode);
            View.Property(p => p.ProcessCode);
            View.Property(p => p.Mblnr).Show();
            View.Property(p => p.Mjahr).Show();
            View.Property(p => p.ProcessMessage);
            View.Property(p => p.Department).Show();
            View.Property(p => p.Version).Show();
            //View.Property(p => p.FromWarehouseCode);
            //View.Property(p => p.FromLocationCode);
            //View.Property(p => p.ToWarehouseCode);
            //View.Property(p => p.ToLocationCode);
            View.Property(p => p.CreateDate).UseDateRangeEditor();//p => { p.DateRangeType = ObjectModel.DateRangeType.Today; }
        }
    }
}