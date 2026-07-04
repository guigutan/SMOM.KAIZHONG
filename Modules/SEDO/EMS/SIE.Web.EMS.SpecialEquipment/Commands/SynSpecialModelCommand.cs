using SIE.EMS.SpecialEquipment.SpecialEquipmentAcounts;
using SIE.Web.Command;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.EMS.SpecialEquipment.Commands
{
    /// <summary>
    /// 同步型号数据命令（增加同步位置列表）
    /// </summary>
    public class SynSpecialModelCommand : ViewCommand
    {
        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            List<double> accountIds = args.SelectedIds.ToList();
            var errMsg = RT.Service.Resolve<SpecialEquipAccountController>().SynSpecialModelDatas(accountIds);
            if (errMsg.Length == 0)
            {
                return "同步型号数据成功";
            }
            else
            {
                return errMsg;
            }
        }
    }
}
