using SIE.EMS.Maintains.Controller;
using SIE.EMS.Maintains.Plans;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.EMS.EquipMaint.Maintains.Plans.Commands
{
    /// <summary>
    /// 保养开始命令
    /// </summary>
    public class BeginMaintainCommand : ViewCommand
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
            var data = args.Data.ToJsonObject<MaintainPlan>();
            RT.Service.Resolve<MaintainController>().BegingMaintain(data.Id);
            return true;
        }
    }
}
