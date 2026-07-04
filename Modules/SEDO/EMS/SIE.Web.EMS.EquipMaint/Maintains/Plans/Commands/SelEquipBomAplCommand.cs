using SIE.Domain;
using SIE.EMS.Enums;
using SIE.EMS.Maintains.Controller;
using SIE.EMS.Maintains.Plans;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.EMS.EquipMaint.Maintains.Plans.Commands
{
    /// <summary>
    /// 申请选择备件BOM
    /// </summary>
    [JsCommand("SIE.Web.EMS.EquipMaint.Maintains.Plans.Commands.SelEquipBomAplCommand")]
    public class SelEquipBomAplCommand : ViewCommand
    {
        /// <summary>
        /// 执行选择
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var sparePartList = args.Data.ToJsonObject<List<MaintainPlanSparePartApl>>();
            Check.NotNullOrEmpty(sparePartList, nameof(sparePartList));
            if (sparePartList == null || sparePartList.Count == 0)
                throw new ArgumentNullException("{0}数据参数不能为空".L10nFormat(nameof(sparePartList)));

            EntityList<MaintainPlanSparePartApl> maintainPlanSpareParts = new EntityList<MaintainPlanSparePartApl>();
            sparePartList.ForEach(p =>
            {
                p.PersistenceStatus = PersistenceStatus.New;
                maintainPlanSpareParts.Add(p);
            });
            RF.Save(maintainPlanSpareParts);
            if (sparePartList.FirstOrDefault()?.MaintainPlan?.ExeState == MaintExeState.NotPerformed)
            {
                RT.Service.Resolve<MaintainController>().ChangeMaintainPlanState(sparePartList.FirstOrDefault().MaintainPlanId, MaintExeState.Performing);
            }
            return true;
        }
    }
}
