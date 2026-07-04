using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.EquipRepair.EquipRepairs;
using System;
using System.Linq;

namespace SIE.Web.EMS.EquipRepair.EquipRepairs.Commands
{
    /// <summary>
    /// 保存
    /// </summary>
    [SIE.Web.Command.JsCommand("SIE.Web.EMS.EquipRepair.EquipRepairs.Commands.SaveCommand")]
    public class SaveCommand : SIE.Web.Command.SaveCommand
    {
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="data"></param>
        protected override void DoSave(EntityList data)
        {
            EntityList<EquipRepairBill> repairBillList = data as EntityList<EquipRepairBill>;
            foreach (var repairBill in repairBillList)
            {
                if (repairBill.RepairBeginDate != null)
                {
                    var sRepairBeginDate = DateTime.Parse(repairBill.RepairBeginDate.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                    if (repairBill.EquipRepairWorkingHoursList.Any(p => p.BeginTime != null && p.BeginTime < sRepairBeginDate))
                    {
                        throw new ValidationException("维修工时中存在员工开始时间早于维修开始时间,请先修改！".L10N());
                    }
                }
                if (repairBill.RepairFinishDate != null)
                    if (repairBill.EquipRepairWorkingHoursList.Where(p => p.EndTime != null).Any(p => p.EndTime != null && p.EndTime > repairBill.RepairFinishDate))
                    {
                        throw new ValidationException("维修工时中存在员工结束时间晚于维修完成时间,请先修改！".L10N());
                    }

                if (repairBill.EquipRepairWorkingHoursList.Any(p => p.BeginTime != null && p.EndTime != null && p.BeginTime > p.EndTime))
                {
                    throw new ValidationException("维修工时中存在员工开始时间晚于维修结束时间,请先修改！".L10N());
                }
            }

            //保存
            RF.Save(data);
        }
    }
}
