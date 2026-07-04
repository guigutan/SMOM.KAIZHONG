using SIE.FMS.FileManages;
using SIE.Web.Command;
using SIE.Web.Common.Attachments.Commands;
using System;
using System.Linq;

namespace SIE.Web.FMS.FileManage.Commands
{
    /// <summary>
    /// 上传命令
    /// </summary>
    [JsCommand("SIE.Web.FMS.FileManage.Commands.UploadCommand")]
    public class UploadCommand : UploadAttachmentCommand
    {
        /// <summary>
        /// 执行命令
        /// </summary>    
        protected override object Excute(ViewArgs args, string scope)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 删除命令
    /// </summary>
    [JsCommand("SIE.Web.FMS.FileManage.Commands.DeleteCommand")]
    public class DeleteCommand : ViewCommand
    {
        /// <summary>
        /// 执行命令
        /// </summary>    
        protected override object Excute(ViewArgs args, string scope)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 下载命令
    /// </summary>
    [JsCommand("SIE.Web.FMS.FileManage.Commands.DownLoadCommand")]
    public class DownLoadCommand : ViewCommand
    {
        /// <summary>
        /// 执行命令
        /// </summary>    
        protected override object Excute(ViewArgs args, string scope)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 预览命令
    /// </summary>
    [JsCommand("SIE.Web.FMS.FileManage.Commands.PreViewCommand")]
    public class PreViewCommand : ViewCommand
    {
        /// <summary>
        /// 执行命令
        /// </summary>    
        protected override object Excute(ViewArgs args, string scope)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 修订命令
    /// </summary>
    [JsCommand("SIE.Web.FMS.FileManage.Commands.EditCommand")]
    public class EditCommand : ViewCommand
    {
        /// <summary>
        /// 执行命令
        /// </summary>    
        protected override object Excute(ViewArgs args, string scope)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 发布命令
    /// </summary>
    [JsCommand("SIE.Web.FMS.FileManage.Commands.PublishCommand")]
    public class PublishCommand : ViewCommand
    {
        /// <summary>
        /// 执行命令
        /// </summary>    
        protected override object Excute(ViewArgs args, string scope)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 作废命令
    /// </summary>
    [JsCommand("SIE.Web.FMS.FileManage.Commands.ScarpCommand")]
    public class ScarpCommand : ViewCommand
    {
        /// <summary>
        /// 执行命令
        /// </summary>    
        protected override object Excute(ViewArgs args, string scope)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 审批流命令
    /// </summary>
    [JsCommand("SIE.Web.FMS.FileManage.Commands.FlowCommand")]
    public class FlowCommand : ViewCommand
    {
        /// <summary>
        /// 执行命令
        /// </summary>    
        protected override object Excute(ViewArgs args, string scope)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 撤回命令
    /// </summary>
    [JsCommand("SIE.Web.FMS.FileManage.Commands.ReturnCommand")]
    public class ReturnCommand : ViewCommand<double[]>
    {
        /// <summary>
        /// 执行命令
        /// </summary>    
        protected override object Excute(double[] args, string scope)
        {
            var ids = args.ToList();
            RT.Service.Resolve<FileManageController>().ReturnFlow(ids);
            return true;
        }
    }

    /// <summary>
    /// 审核命令
    /// </summary>
    [JsCommand("SIE.Web.FMS.FileManage.Commands.AuditCommand")]
    public class AuditCommand : ViewCommand
    {
        /// <summary>
        /// 执行命令
        /// </summary>    
        protected override object Excute(ViewArgs args, string scope)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 文件管理命令
    /// </summary>
    public static class FileManageCommands
    {
        /// <summary>
        /// 上传命令
        /// </summary>
        public static string UploadCommand { get { return "SIE.Web.FMS.FileManage.Commands.UploadCommand"; } }

        /// <summary>
        /// 删除命令
        /// </summary>
        public static readonly string DeleteCommand = "SIE.Web.FMS.FileManage.Commands.DeleteCommand";

        /// <summary>
        /// 下载命令
        /// </summary>
        public static readonly string DownLoadCommand = "SIE.Web.FMS.FileManage.Commands.DownLoadCommand";

        /// <summary>
        /// 预览命令
        /// </summary>
        public static readonly string PreViewCommand = "SIE.Web.FMS.FileManage.Commands.PreViewCommand";

        /// <summary>
        /// 修订命令
        /// </summary>
        public static readonly string EditCommand = "SIE.Web.FMS.FileManage.Commands.EditCommand";

        /// <summary>
        /// 发布命令
        /// </summary>
        public static readonly string PublishCommand = "SIE.Web.FMS.FileManage.Commands.PublishCommand";

        /// <summary>
        /// 作废命令
        /// </summary>
        public static readonly string ScarpCommand = "SIE.Web.FMS.FileManage.Commands.ScarpCommand";

        /// <summary>
        /// 审批流命令
        /// </summary>
        public static readonly string FlowCommand = "SIE.Web.FMS.FileManage.Commands.FlowCommand";

        /// <summary>
        /// 撤回命令
        /// </summary>
        public static readonly string ReturnCommand = "SIE.Web.FMS.FileManage.Commands.ReturnCommand";

        /// <summary>
        /// 审核命令
        /// </summary>
        public static readonly string AuditCommand = "SIE.Web.FMS.FileManage.Commands.AuditCommand";
    }
}
