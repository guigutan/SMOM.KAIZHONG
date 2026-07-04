using SIE.EMS.EquipLends;
using SIE.Web.Common.Attachments.Commands;
using SIE.Web.Core.Common.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.EquipLends
{
    /// <summary>
    /// 设备借还附件视图配置
    /// </summary>
    public class EquipLendAttachmentViewConfig : WebViewConfig<EquipLendAttachment>
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
