using SIE.Resources.Employees;
using SIE.Web.Command;

namespace SIE.Web.Resources.Employees.Commands
{
    [JsCommand("SIE.Web.Resources.Employees.Commands.EnableSignCommand")]
    internal class EnableSignCommand : ViewCommand
    {
        protected override object Excute(ViewArgs args, string scope)
        {
            LinkUserCommandViewArgs data = args.Data.ToJsonObject<LinkUserCommandViewArgs>();
            RT.Service.Resolve<EmployeeController>().EnableSignState(data.Id);
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
