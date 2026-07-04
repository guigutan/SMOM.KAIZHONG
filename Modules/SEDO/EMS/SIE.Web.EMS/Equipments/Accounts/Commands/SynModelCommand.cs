using SIE.EMS.Equipments;
using SIE.Web.Command;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.EMS.Equipments.Accounts.Commands
{
    /// <summary>
    /// 同步型号数据命令（增加同步位置列表）
    /// </summary>
    public class SynModelCommand : ViewCommand
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
            RT.Service.Resolve<EquipController>().SynModelDatas(accountIds);
            return "同步型号数据成功";
        }
    }
}
