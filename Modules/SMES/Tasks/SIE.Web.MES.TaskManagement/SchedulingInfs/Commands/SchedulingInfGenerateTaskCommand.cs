using AngleSharp.Css.Dom;
using SIE.MES.TaskManagement.SchedulingInfs;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.TaskManagement.SchedulingInfs.Commands
{
    /// <summary>
    /// 排程下发(创建派工任务单)
    /// </summary>
    public class SchedulingInfGenerateTaskCommand : ViewCommand<List<double>>
    {
        protected override object Excute(List<double> args, string scope)
        {
            var result = RT.Service.Resolve<SchedulingInfController>().GenerateTask(args);
            if (result.IsNullOrEmpty())
                result = "下发成功!".L10N();
            return result;
        }
    }
}
