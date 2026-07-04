using SIE.FMS;
using SIE.FMS.FileManages;
using SIE.Web.Command;

namespace SIE.Web.FMS.FileManage.Commands
{
    /// <summary>
    /// 设置文件管理员命令
    /// </summary>
    [JsCommand("SIE.Web.FMS.FileManage.Commands.SetAdminCommand")]
    public class SetAdminCommand : ViewCommand<ViewArgs>
    {
        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var fileUserGroup = args.Data.ToJsonObject<FileUserGroup>();
            RT.Service.Resolve<FileManageController>().SetAdmin(fileUserGroup);
            return true;
        }
    }
}
