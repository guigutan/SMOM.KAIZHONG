using SIE.Domain;
using SIE.EMS.MeteringEquipment.MeteringEquipmentAccounts.Tab;
using SIE.Equipments.DeviceIOTParas.Enums;
using SIE.Equipments.Enums;
using SIE.Equipments.EquipAccounts;
using SIE.Web.Command;
using System;
using System.Collections.Generic;

namespace SIE.Web.EMS.MeteringEquipment.MeteringEquipmentAccounts.Commands
{
    /// <summary>
    /// 选择物联参数
    /// </summary>
    [JsCommand("SIE.Web.EMS.MeteringEquipment.MeteringEquipmentAccounts.Commands.SelMeterAccountPhysicalUnionCommand")]
    public class SelMeterAccountPhysicalUnionCommand : ViewCommand
    {
        /// <summary>
        /// 执行选择
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var equipPhysicalUnionList = args.Data.ToJsonObject<List<MeterEquipAccountPhysicalUnion>>();
            Check.NotNullOrEmpty(equipPhysicalUnionList, nameof(equipPhysicalUnionList));
            if (equipPhysicalUnionList == null || equipPhysicalUnionList.Count == 0)
            {
                throw new ArgumentNullException("{0}数据参数不能为空".L10nFormat(nameof(equipPhysicalUnionList)));
            }
            EntityList<MeterEquipAccountPhysicalUnion> equipPhysicalUnions = new EntityList<MeterEquipAccountPhysicalUnion>();
            equipPhysicalUnionList.ForEach(p =>
            {
                p.PersistenceStatus = PersistenceStatus.New;
                p.EquipPara = p.PhysicalUnion.From == FromType.Interface ? EquipPara.AutomaticValue : EquipPara.ManualValue;
                equipPhysicalUnions.Add(p);
            });
            RF.Save(equipPhysicalUnions);
            return true;
        }
    }
}
