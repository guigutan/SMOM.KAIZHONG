using SIE.ERPInterface.Common.Controller;
using SIE.ERPInterface.Common.Enums;
using SIE.ERPInterface.Ebs;
using SIE.Items;
using SIE.Web.Command;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.ERPInterface.Logs.Commands
{
    /// <summary>
    /// 重传命令
    /// </summary>
    public class ReUploadErpUploadLogCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            List<double> idlist = args.SelectedIds.ToList(); //事务Id列表

            RT.Service.Resolve<UploadBaseController>().AdjustTransation(idlist, ProcessState.Retry);
            return true;
        }
    }
    /// <summary>
    /// 查看报文命令
    /// </summary>
    public class LookUpContextCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            return true;
        }
    }

    #region 关闭上传命令
    /// <summary>
    /// 关闭上传命令
    /// </summary>
    public class CloseUploadErpUploadLogCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>bool</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            List<double> idlist = args.SelectedIds.ToList(); //事务Id列表

            RT.Service.Resolve<UploadBaseController>().AdjustTransationFromCommand(idlist, ProcessState.Abandon);
            return true;
        }
    }
    #endregion

    #region 恢复上传命令
    /// <summary>
    /// 恢复上传命令
    /// </summary>
    public class RestoreUploadErpUploadLogCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>bool</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            List<double> idlist = args.SelectedIds.ToList(); //事务Id列表

            RT.Service.Resolve<UploadBaseController>().AdjustTransationFromCommand(idlist, ProcessState.Retry);
            return true;
        }
    }
    #endregion
}


