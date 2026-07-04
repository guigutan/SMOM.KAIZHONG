using SIE.Resources.Employees;
using SIE.Web.Command;

namespace SIE.Web.Resources.Employees.Commands
{
    //SIE.Web.Resources.Employees.Commands.SignShowImageCommand
    [JsCommand("SIE.Web.Resources.Employees.Commands.SignShowImageCommand")]
    internal class SignShowImageCommand : ViewCommand
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
            string fileContent = RT.Service.Resolve<EmployeeController>().getSignPhoto(data.Id);
            return new { fileContent = fileContent }.ToJsonString();
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
