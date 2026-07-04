using SIE.Inventory.Transactions;
using SIE.Web.Inventory.Transactions.Commands;

namespace SIE.Web.Inventory.Transactions
{
    /// <summary>
    /// 功能对应事务
    /// </summary>
    internal class FunctionTransactionViewConfig : WebViewConfig<FunctionTransaction>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(typeof(LookupTransactionCommand).FullName, typeof(DeleteFunTransactionCommand).FullName);
            View.Property(p => p.TransactionCode).ShowInList(150);
            View.Property(p => p.TransactionName);
            View.Property(p => p.TransactionSourceType).Readonly(true);
            View.Property(p => p.TransactionIsInternalUse).UseEditor(WPFEditorNames.CheckDropDown);
            View.Property(p => p.TransactionState);
        }

        /// <summary>
        /// 配置选择视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.ClearCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.TransactionCode).ShowInList(150);
                View.Property(p => p.TransactionName);
                View.Property(p => p.TransactionSourceType).Readonly(true);
                View.Property(p => p.TransactionIsInternalUse).UseEditor(WPFEditorNames.CheckDropDown);
                View.Property(p => p.TransactionState);
            }
        }
    }
}
