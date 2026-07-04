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
    /// 选择备件
    /// </summary>
    [JsCommand("SIE.Web.EMS.EquipMaint.Maintains.Plans.Commands.SelSparePartCommand")]
    public class SelSparePartCommand : ViewCommand
    {
        /// <summary>
        /// 执行选择
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var sparePartList = args.Data.ToJsonObject<List<MaintainPlanSparePart>>();
            Check.NotNullOrEmpty(sparePartList, nameof(sparePartList));
            if (sparePartList == null || sparePartList.Count == 0)
            {
                throw new ArgumentNullException("{0}数据参数不能为空".L10nFormat(nameof(sparePartList)));
            }

            EntityList<MaintainPlanSparePart> maintainPlanSpareParts = new EntityList<MaintainPlanSparePart>();
            sparePartList.ForEach(p =>
            {
                p.PersistenceStatus = PersistenceStatus.New;
                p.ChangeQty = 1;
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
