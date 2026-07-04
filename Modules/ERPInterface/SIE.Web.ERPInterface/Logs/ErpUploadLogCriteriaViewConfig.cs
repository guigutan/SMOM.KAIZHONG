using SIE.Core.Enums;
using SIE.ERPInterface.Common.Logs;

namespace SIE.Web.ERPInterface.Logs
{
    /// <summary>
    /// 事务上传记录查询实体配置视图
    /// </summary>
    public class ErpUploadLogCriteriaViewConfig : WebViewConfig<ErpUploadLogCriteria>
    {
        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.OrderNo).Show();
                View.Property(p => p.OrderType).UseEnumMutilEditor(p => p.EnumType = typeof(OrderType)).Show();
                View.Property(p => p.IsSuccess).UseCheckDropDownEditor().Show();
                View.Property(p => p.LogDate).Show();
                View.Property(p => p.TransactionId).UseTextEditor(p => p.InputType = "number").Show();
            }
        }
    }
}