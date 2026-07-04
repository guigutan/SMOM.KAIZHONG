using SIE.Resources.ProcessTechs;
using SIE.Web.Command;

namespace SIE.Web.Resources.ProcessTechs.Commands
{
    /// <summary>
    /// 制程工艺复制新增命令
    /// </summary>
    public class ProcessTechCopyCommand : ViewCommand
    {
        /// <summary>
        /// 制程工艺复制新增命令
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var data = args.Data.ToJsonObject<ProcessTech>();
            data.Code = RT.Service.Resolve<ProcessTechBaseController>().GetProcessTechNo();
            return data;
        }
    }
}
