using SIE.Resources.Employees;
using SIE.Web.Command;

namespace SIE.Web.Resources.Employees.Commands
{
    /// <summary>
    /// 关联账号命令
    /// </summary>
    [JsCommand("SIE.Web.Resources.Employees.Commands.LinkUserCommand")]
    public class LinkUserCommand : ViewCommand<ViewArgs>
    {
        /// <summary>
        /// 命令执行
        /// </summary>
        /// <param name="args">视图参数</param>
        /// <param name="scope">范围</param>
        /// <returns></returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var data = args.Data.ToJsonObject<LinkUserCommandViewArgs>();
            RT.Service.Resolve<EmployeeController>().LinkUser(data.EmployeeId, data.UserId);
            return true;
        }
    }

    /// <summary>
    /// 取消账号关联命令
    /// </summary>
    [JsCommand("SIE.Web.Resources.Employees.Commands.UnLinkUserCommand")]
    public class UnLinkUserCommand : ViewCommand
    {
        /// <summary>
        /// 命令执行
        /// </summary>
        /// <param name="args">视图参数</param>
        /// <param name="scope">范围</param>
        /// <returns></returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var employee = args.Data.ToJsonObject<Employee>();
            RT.Service.Resolve<EmployeeController>().UnlinkUser(employee.Id);
            return true;
        }
    }

    /// <summary>
    /// 视图
    /// </summary>
    public class LinkUserCommandViewArgs
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public double UserId { get; set; }

        /// <summary>
        /// 员工ID
        /// </summary>
        public double EmployeeId { get; set; }
    }
}
