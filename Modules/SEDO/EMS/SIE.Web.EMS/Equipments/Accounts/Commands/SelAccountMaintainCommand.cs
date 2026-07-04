using SIE.EMS.Common.Entity;
using SIE.EMS.Equipments;
using SIE.Web.Command;
using System.Collections.Generic;

namespace SIE.Web.EMS.Equipments.Accounts.Commands
{
    /// <summary>
    /// 添加保养项目
    /// </summary>
    [JsCommand("SIE.Web.EMS.Equipments.Accounts.Commands.SelAccountMaintainCommand")]
    public class SelAccountMaintainCommand : ViewCommand
    {
        /// <summary>
        /// 执行添加
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var checkProjectInfos = args.Data.ToJsonObject<List<CheckProjectInfo>>();
            Check.NotNullOrEmpty(checkProjectInfos, nameof(checkProjectInfos));
            RT.Service.Resolve<EquipController>().SaveAccountMaintainCommand(checkProjectInfos);
            return true;
        }
    }
}
