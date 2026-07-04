using SIE.EMS.SpareParts;
using SIE.Web.Common.Attachments.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.SpareParts
{
    /// <summary>
    /// 附件视图配置
    /// </summary>
    internal class SparePartAttachmentViewConfig : WebViewConfig<SparePartAttachment>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.RemoveCommands(typeof(ViewImageCommand).FullName);
        }
    }
}
