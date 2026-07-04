using AngleSharp.Dom;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.Checks.Plans.ViewModels;
using SIE.EMS.EquipRepair.Controller;
using SIE.EMS.EquipRepair.EquipRepairs;
using SIE.EMS.EquipRepairs.Enums;
using SIE.Web.Command;
using System;

namespace SIE.Web.EMS.EquipRepair.EquipRepairs.Commands
{
    /// <summary>
    /// 保存报修
    /// </summary>
    [JsCommand("SIE.Web.EMS.EquipRepair.EquipRepairs.Commands.SaveEquipRepairCommand")]
    public class SaveEquipRepairCommand : ViewCommand
    {
        /// <summary>
        /// 执行(提示是否已存在对应设备的未完成的报修单)
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var bill = args.Data.ToJsonObject<EquipRepairBill>();
            if (bill == null)
            {
                throw new ValidationException("该数据不是报修数据格式。".L10N());
            }
            if (bill.EquipAccountId == 0 && bill.SparePartId == 0)
            {
                throw new ValidationException("设备或备件不能空。".L10N());
            }
            if (bill.DeviceAbnormalId == 0 && string.IsNullOrEmpty(bill.DeviceAbnormalRemark))
            {
                throw new ValidationException("【故障现象】与【故障现象（备注）】至少有一个不为空。".L10N());
            }
            bill.RepairState = EquipRepairState.ApplyRepair;
            //因改动点检,保养,润滑,定检等页面的报修,故来源状态从对应页面传入,不在此固定.
            RT.Service.Resolve<RepairController>().GenerateRepair(bill);
            return true;
        }
    }
}
