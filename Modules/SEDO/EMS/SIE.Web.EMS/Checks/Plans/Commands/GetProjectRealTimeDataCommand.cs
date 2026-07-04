using SIE.Core.ApiModels;
using SIE.EMS.Checks;
using SIE.EMS.Checks.Data;
using SIE.EMS.MainenanceProjects;
using SIE.EventMessages.EAP.Equipments;
using SIE.Web.Command;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.EMS.Checks.Plans.Commands
{
    /// <summary>
    /// 获取设备实时数据命令
    /// </summary>
    [JsCommand("SIE.Web.EMS.Checks.Plans.Commands.GetProjectRealTimeDataCommand")]
    public class GetProjectRealTimeDataCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var data = args.Data.ToJsonObject<CheckPlanEapData>();
            var rtn = RT.Service.Resolve<CheckPlanController>().GetProjectRealTimeData(data);

            return rtn;
        }
    }
}
