using SIE.Domain;
using SIE.EMS.InventoryPlans;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.EMS.InventoryPlans.Commands
{
    /// <summary>
    /// 选择备件基础数据
    /// </summary>
    [JsCommand("SIE.Web.EMS.InventoryPlans.Commands.SelectSparePartCommand")]
    public class SelectSparePartCommand : ViewCommand
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
            var sparePartLists = args.Data.ToJsonObject<List<SparePartList>>();
            Check.NotNullOrEmpty(sparePartLists, nameof(sparePartLists));

            if (null == sparePartLists || sparePartLists.Count == 0)
                throw new ArgumentNullException(nameof(sparePartLists));

            foreach (var item in sparePartLists)
            {
                var sparePart = new SparePartList();
                sparePart.InventoryPlanId = item.InventoryPlanId;
                sparePart.SparePartId = item.SparePartId;
                savedData.Add(sparePart);
            }
            RF.Save(savedData);
            return true;
        }
    }
}
