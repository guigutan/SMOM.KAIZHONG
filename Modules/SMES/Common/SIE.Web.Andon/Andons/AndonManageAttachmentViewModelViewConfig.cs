using SIE.Andon.Andons.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.Andon.Andons
{
    /// <summary>
    /// 附件信息视图配置
    /// </summary>
    public class AndonManageAttachmentViewModelViewConfig : WebViewConfig<AndonManageAttachmentViewModel>
    {
        /// <summary>
        /// 表单视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.AttachmentName).UseImageComponentEditor(p => { p.Xtype = "andonmanageattachmentbtn"; }).HasLabel("").Readonly();
        }
    }
}
