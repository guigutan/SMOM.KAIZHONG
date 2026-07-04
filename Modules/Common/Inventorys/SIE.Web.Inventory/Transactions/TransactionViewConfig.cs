using SIE.Inventory.Transactions;
using SIE.MetaModel.View;
using SIE.Web.Inventory.Transactions.Commands;

namespace SIE.Web.Inventory.Transactions
{
    /// <summary>
    /// 单据小类
    /// </summary>
    internal class TransactionViewConfig : WebViewConfig<Transaction>
    {
        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.DomainName("单据小类").HasDelegate(Transaction.NameProperty);
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.InlineEdit();
            View.UseCommands(WebCommandNames.Add, WebCommandNames.Edit, typeof(DeleteTransactionCommand).FullName, WebCommandNames.Save,
             typeof(SetTransactionInternalUseCommand).FullName, typeof(SetTransactionIsUploadCommand).FullName);
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsSelection, WebCommandNames.ExportXlsAll);
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).Readonly(p => p.IsEdit).ShowInList(150);
                View.Property(p => p.Name).Readonly(p => p.IsEdit);
                View.Property(p => p.Description);
                View.Property(p => p.SourceType).Readonly();
                View.Property(p => p.State).Readonly();
                View.Property(p => p.IsInternalUse).Readonly();
                View.Property(p => p.SortOut);
                View.Property(p => p.IsUpload).Readonly();
                View.Property(p => p.IsEdit).Readonly();
                View.Property(p => p.MesProcessName);
                View.Property(p => p.RfcProcessName);
            }
        }

        /// <summary>
        /// 配置选择视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.Description);
            View.Property(p => p.SourceType);
            View.Property(p => p.State);
        }

        /// <summary>
        /// 培训查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.Description);
            View.Property(p => p.SourceType).UseEnumEditor(p => p.AllowBlank = true);
            View.Property(p => p.State).UseEnumEditor(p => p.AllowBlank = true);
        }
    }
}
