using SIE.Domain;
using SIE.EMS.InventoryPlans;
using SIE.Resources.Employees;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.EMS.InventoryPlans.Commands
{
    /// <summary>
    /// 选择设备台账
    /// </summary>
    [JsCommand("SIE.Web.EMS.InventoryPlans.Commands.SelectEquipListCommand")]
    public class SelectEquipListCommand : ViewCommand
    {

        /// <summary>
        /// 执行选择
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var meta = ClientEntities.Find(args.Type);
            var savedData = RF.Find(meta.EntityType).NewList();
            var equipmentLists = args.Data.ToJsonObject<List<EquipmentList>>();
            Check.NotNullOrEmpty(equipmentLists, nameof(equipmentLists));

            if (null == equipmentLists || equipmentLists.Count == 0)
                throw new ArgumentNullException(nameof(equipmentLists));

            foreach (var item in equipmentLists)
            {
                var equipment = new EquipmentList();
                equipment.InventoryPlanId = item.InventoryPlanId;
                equipment.EquipAccoutId = item.EquipAccoutId;
                equipment.EquipTypeId = item.EquipTypeId;
                equipment.UseDeptId = item.UseDeptId;
                equipment.EquipModelId = item.EquipModelId;
                equipment.WorkShopId = item.WorkShopId;
                savedData.Add(equipment);
            }
            RF.Save(savedData);
            return true;
        }
    }
}
