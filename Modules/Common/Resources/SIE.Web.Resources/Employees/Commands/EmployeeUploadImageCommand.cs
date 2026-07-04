using SIE.Resources.Employees;
using SIE.Web.Command;

namespace SIE.Web.Resources.Employees.Commands
{
    /// <summary>
    /// 上传图片
    /// </summary>
    /// SIE.Web.Resources.Employees.Commands.EmployeeUploadImageCommand
    [JsCommand("SIE.Web.Resources.Employees.Commands.EmployeeUploadImageCommand")]
    public class EmployeeUploadImageCommand : ViewCommand
    {
        protected override object Excute(ViewArgs args, string scope)
        {
            LinkUserCommandViewArgs data = args.Data.ToJsonObject<LinkUserCommandViewArgs>();
            RT.Service.Resolve<EmployeeController>().SignleUploadImage(data.Id,data.fileContent,data.fileName);
            return true;
        }

        /// <summary>
        /// 视图
        /// </summary>
        public class LinkUserCommandViewArgs
        {
            /// <summary>
            /// 员工表ID
            /// </summary>
            public double Id { get; set; }

            /// <summary>
            /// 图片Base64
            /// </summary>
            public string fileContent { get; set; }

            /// <summary>
            /// 图片名称
            /// </summary>
            public string fileName { get; set; }

        }
    }
}
