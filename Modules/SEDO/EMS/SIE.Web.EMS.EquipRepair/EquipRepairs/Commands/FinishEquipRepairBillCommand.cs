using Newtonsoft.Json;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.EquipRepair.Controller;
using SIE.EMS.EquipRepair.EquipRepairs;
using SIE.EMS.EquipRepair.EquipRepairs.ViewModels;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.Web.EMS.EquipRepair.EquipRepairs.Commands
{
    /// <summary>
    /// 维修单完成命令
    /// </summary>
    public class FinishEquipRepairBillCommand : FormSaveCommand
    {
        /// <summary>
        /// 维修单完成命令执行
        /// </summary>
        /// <param name="args">视图参数</param>
        /// <param name="scope">命令参数</param>
        /// <returns>返回参数</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            if (args.Data.Contains("\"_model\":\"SIE.EMS.EquipRepair.EquipRepairs.EquipRepairBill\""))
            {
                var entity = GetDeserializeData(args, scope)[0] as EquipRepairBill;

                if (entity.RepairBeginDate != null)
                    if (entity.EquipRepairWorkingHoursList.Any(p => p.BeginTime != null && p.BeginTime < entity.RepairBeginDate))
                    {
                        throw new ValidationException("维修工时中存在员工开始时间早于维修开始时间,请先修改！".L10N());
                    }

                if (entity.RepairFinishDate != null)
                    if (entity.EquipRepairWorkingHoursList.Where(p => p.EndTime != null).Any(p => p.EndTime != null && p.EndTime > entity.RepairFinishDate))
                    {
                        throw new ValidationException("维修工时中存在员工结束时间晚于维修完成时间,请先修改！".L10N());
                    }

                if (entity.EquipRepairWorkingHoursList.Any(p => p.BeginTime != null && p.EndTime != null && p.BeginTime > p.EndTime))
                {
                    throw new ValidationException("维修工时中存在员工开始时间晚于维修结束时间,请先修改！".L10N());
                }

                RF.Save(entity);
                return true;
            }
            else
            {
                var info = JsonConvert.DeserializeObject<EquipRepairFinishCommandInfo>(args.Data);
                return RT.Service.Resolve<RepairController>().FinishRepair(info.EquipRepair, info.IsFillinReport);
            }
        }
    }
}
