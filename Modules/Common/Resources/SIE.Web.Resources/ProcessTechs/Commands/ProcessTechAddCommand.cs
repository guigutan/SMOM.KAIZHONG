using SIE.Resources.ProcessTechs;
using SIE.Security;
using SIE.Web.Command;

namespace SIE.Web.Resources.ProcessTechs.Commands
{
    /// <summary>
    /// 制程工艺添加命令
    /// </summary>
    [AllowAnonymous]
    public class ProcessTechAddCommand : ViewCommand
    {
        /// <summary>
        /// 制程工艺添加命令
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var data = args.Data.ToJsonObject<ProcessTech>();
            data.IsScheduling = true;
            data.Code = RT.Service.Resolve<ProcessTechBaseController>().GetProcessTechNo();
            return data;
        }
    }
}