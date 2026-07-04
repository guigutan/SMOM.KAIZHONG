using SIE.MES.ProjectDesigns;
using SIE.MES.ProjectDesigns.ChildInfos;
using SIE.MetaModel.View;
using SIE.Web.Common.Attachments.Commands;
using SIE.Web.MES.ProjectDesigns.ChildCommands.DocumentCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.ProjectDesigns.ChildInfos
{
    /// <summary>
    /// 工艺资料-文档上传附件视图配置
    /// </summary>
    public class DesignTreeDocumentAttachmentViewConfig : WebViewConfig<DesignTreeDocumentAttachment>
    {
        /// <summary>
        /// 查询视图
        /// </summary>
        public const string LookUpViewGroup = "LookUpViewGroup";

        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(ProjectDesign));
            View.DeclareExtendViewGroup(LookUpViewGroup);
            View.InlineEdit();
            if (ViewGroup == LookUpViewGroup)
            {
                ReadOnlyView();
            }
        }

        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.ReplaceCommands(typeof(UploadAttachmentCommand).FullName, typeof(DocumentUploadCommand).FullName);
        }

        /// <summary>
        /// 查看界面视图
        /// </summary>
        private void ReadOnlyView()
        {
            base.ConfigListView();
            View.RemoveCommands(typeof(UploadAttachmentCommand).FullName, typeof(DocumentUploadCommand).FullName, typeof(DeleteAttachmentCommand).FullName);
        }
    }

}
