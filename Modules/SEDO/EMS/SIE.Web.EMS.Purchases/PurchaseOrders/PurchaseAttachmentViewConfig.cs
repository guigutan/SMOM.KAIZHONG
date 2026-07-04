using SIE.EMS.Purchases.PurchaseOrders;
using SIE.Web.Common.Attachments.Commands;
using SIE.Web.Core.Common.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.EMS.Purchases.PurchaseOrders
{
    /// <summary>
    /// 资产采购订单附件视图配置
    /// </summary>
    public class PurchaseAttachmentViewConfig : WebViewConfig<PurchaseAttachment>
    {
        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.ReplaceCommands(typeof(DeleteAttachmentCommand).FullName, typeof(ImmediateDeleteCommand).FullName);
        }
    }
}
