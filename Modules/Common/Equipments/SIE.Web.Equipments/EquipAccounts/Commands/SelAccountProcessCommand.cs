using SIE.Domain;
using SIE.Equipments.EquipAccounts;
using SIE.Web.Command;
using System;
using System.Collections.Generic;

namespace SIE.Web.Equipments.EquipAccounts.Commands
{
    /// <summary>
    /// 选择工序
    /// </summary>
    [JsCommand("SIE.Web.Equipments.EquipAccounts.Commands.SelAccountProcessCommand")]
    public class SelAccountProcessCommand : ViewCommand
    {
        /// <summary>
        /// 执行选择
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var equipProcessList = args.Data.ToJsonObject<List<EquipAccountProcess>>();
            Check.NotNullOrEmpty(equipProcessList, nameof(equipProcessList));
            if (equipProcessList == null || equipProcessList.Count == 0)
            {
                throw new ArgumentNullException("{0}数据参数不能为空".L10nFormat(nameof(equipProcessList)));
            }

            RT.Service.Resolve<EquipAccountController>().SaveEquipAccountProcessList(equipProcessList);

            return true;
        }
    }
}
