using SIE.Resources.Employees;
using SIE.Web.Command;

namespace SIE.Web.Resources.Employees.Commands
{
    [JsCommand("SIE.Web.Resources.Employees.Commands.DeleteSignCommand")]
    internal class DeleteSignCommand : ViewCommand
    {
        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            LinkUserCommandViewArgs data = args.Data.ToJsonObject<LinkUserCommandViewArgs>();
            RT.Service.Resolve<EmployeeController>().DelteSignData(data.Id);
            return true;
        }

        /// <summary>
        /// 视图
        /// </summary>
        public class LinkUserCommandViewArgs
        {
            /// <summary>
            /// 签名表ID
            /// </summary>
            public double Id { get; set; }

        }
    }

    
}
