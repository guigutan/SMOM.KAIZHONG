using SIE.ERPInterface.Common.UploadTransactionRules;
using SIE.MetaModel.View;
using SIE.Web.ERPInterface.UploadTransactionRules.Commands;

namespace SIE.Web.ERPInterface.UploadTransactionRules
{
    /// <summary>
    /// 事务上传仓库排除列表 视图
    /// </summary>
    public class UploadTransactionExclWhViewConfig : WebViewConfig<UploadTransactionExclWh>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.FormEdit();
            View.UseCommands(typeof(TransactionWarehouseLookupCommand).FullName, WebCommandNames.Delete);
            using (View.OrderProperties())
            {
                View.Property(p => p.WarehouseCode);
                View.Property(p => p.WarehouseName);
            }
        }
    }
}
