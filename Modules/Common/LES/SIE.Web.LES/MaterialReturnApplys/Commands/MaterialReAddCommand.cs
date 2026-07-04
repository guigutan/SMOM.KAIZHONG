using SIE.LES.MaterialReturnApplys;
using SIE.LES.MaterialReturnApplys.Enums;
using SIE.Web.Command;
using System;
using System.Linq;

namespace SIE.Web.LES.MaterialReturnApplys.Commands
{
    /// <summary>
    /// 退料申请添加命令
    /// </summary>
    public class MaterialReAddCommand : ViewCommand
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
            var data = args.Data.ToJsonObject<MaterialReturnApply>();
            data.No = RT.Service.Resolve<MaterialReturnApplyController>().GetMrNoLists(1).FirstOrDefault();
            data.ReStatus = ReStatus.Saved;
            data.ReType = ReType.WorkOrderReturn;
            return data;
        }
    }
}
