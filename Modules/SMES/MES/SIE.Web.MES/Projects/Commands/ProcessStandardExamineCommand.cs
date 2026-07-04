using SIE.MES.Projects;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.Projects.Commands
{
    /// <summary>
    /// 工序标准参数审核命令
    /// </summary>
    public class ProcessStandardExamineCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        protected override object Excute(ViewArgs args, string scope)
        {
            var ids = args.SelectedIds.ToList();
            RT.Service.Resolve<ProjectParamController>().ExamineProcessStandard(ids);
            return true;
        }
    }
}
