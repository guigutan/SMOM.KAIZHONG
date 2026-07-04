using SIE.Resources.Employees;
using SIE.Security;
using SIE.Web.Command;

namespace SIE.Web.Resources.Employees.Commands
{
    /// <summary>
    /// 首页上传图片
    /// </summary>
    [AllowAnonymous]
    public class PortalUploadImageCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            LinkUserCommandViewArgs data = args.Data.ToJsonObject<LinkUserCommandViewArgs>();
            var UserId = RT.IdentityId;
            RT.Service.Resolve<EmployeeController>().SignleUploadImage(UserId, data.fileContent, data.fileName);
            return "修改成功";
        }

        /// <summary>
        /// 视图
        /// </summary>
        public class LinkUserCommandViewArgs
        {
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
