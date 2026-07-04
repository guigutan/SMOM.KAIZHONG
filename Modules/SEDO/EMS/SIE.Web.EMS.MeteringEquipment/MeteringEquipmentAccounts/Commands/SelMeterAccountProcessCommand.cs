using SIE.Domain;
using SIE.EMS.MeteringEquipment.MeteringEquipmentAccounts;
using SIE.EMS.MeteringEquipment.MeteringEquipmentAccounts.Tab;
using SIE.Equipments.EquipAccounts;
using SIE.Web.Command;
using System;
using System.Collections.Generic;

namespace SIE.Web.EMS.MeteringEquipment.MeteringEquipmentAccounts.Commands
{
    /// <summary>
    /// 选择工序
    /// </summary>
    [JsCommand("SIE.Web.EMS.MeteringEquipment.MeteringEquipmentAccounts.Commands.SelMeterAccountProcessCommand")]
    public class SelMeterAccountProcessCommand : ViewCommand
    {
        /// <summary>
        /// 执行选择
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var equipProcessList = args.Data.ToJsonObject<List<MeterEquipAccountProcess>>();
            Check.NotNullOrEmpty(equipProcessList, nameof(equipProcessList));
            if (equipProcessList == null || equipProcessList.Count == 0)
            {
                throw new ArgumentNullException("{0}数据参数不能为空".L10nFormat(nameof(equipProcessList)));
            }

            RT.Service.Resolve<MeteringEquipmentAccountController>().SaveMeterEquipAccountProcessList(equipProcessList);

            return true;
        }
    }
}
