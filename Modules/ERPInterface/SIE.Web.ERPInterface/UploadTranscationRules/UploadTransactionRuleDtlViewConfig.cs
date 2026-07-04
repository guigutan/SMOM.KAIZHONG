using SIE.Domain;
using SIE.ERPInterface.Common.UploadTransactionRules;
using SIE.Inventory.Transactions;

namespace SIE.Web.ERPInterface.UploadTransactionRules
{
    /// <summary>
    /// 交易上传规则明细 视图
    /// </summary>
    public class UploadTransactionRuleDtlViewConfig : WebViewConfig<UploadTransactionRuleDtl>
    {
        /// <summary>
        /// 
        /// </summary>
        protected override void ConfigView()
        {
            View.DomainName("交易上传规则明细");
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseDefaultCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.OrderType);
                View.Property(p => p.TransactionId).UseDataSource((e, c, r) =>
                {
                    var dtl = e as UploadTransactionRuleDtl;
                    if (dtl == null)
                        return new EntityList<Transaction>();
                    return RT.Service.Resolve<TransactionController>().GetTransactions(c, r, dtl.OrderType);
                }).UsePagingLookUpEditor(p => { p.ReloadDataOnPopping = true; });
                View.Property(p => p.TransactionType);
            }
        }

        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.OrderType);
                View.Property(p => p.TransactionType);
            }
        }
    }
}
