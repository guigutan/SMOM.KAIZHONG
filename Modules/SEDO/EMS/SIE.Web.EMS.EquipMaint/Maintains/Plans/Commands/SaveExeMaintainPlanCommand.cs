using SIE.Common.Configs;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.Checks.Plans;
using SIE.EMS.Enums;
using SIE.EMS.Maintains.Configs;
using SIE.EMS.Maintains.Plans;
using SIE.EMS.Maintains.Plans.ViewModels;
using SIE.EMS.Maintains.Projects;
using SIE.Web.Command;
using System;
using System.Linq;

namespace SIE.Web.EMS.EquipMaint.Maintains.Plans.Commands
{
    /// <summary>
    /// 保存执行点检计划
    /// </summary>
    [JsCommand("SIE.Web.EMS.EquipMaint.Maintains.Plans.Commands.SaveExeMaintainPlanCommand")]
    public class SaveExeMaintainPlanCommand : FormSaveCommand
    {
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="entity">实体</param>
        protected override void DoSave(Entity entity)
        {
            if (entity == null)
            {
                throw new ValidationException("没有数据可以提交。".L10N());
            }
            if (!(entity is MaintainPlan))
                throw new ValidationException("该数据不是保养执行数据格式。".L10N());

            //保养项目
            var bill = entity as MaintainPlan;

            if (bill.ActBeginDate != null || bill.ActEndDate != null)
            {
                if (bill.ActBeginDate > bill.ActEndDate)
                    throw new ValidationException("保养结束时间不能比开始时间早，不允许提交。".L10N());
            }

            if (bill.ExeState == MaintExeState.NotPerformed)
                bill.ExeState = MaintExeState.Performing;
            bill.ProjectList.ForEach(p =>
            {
                if (p.MaintainResult == CheckMaintainResult.OK || p.MaintainResult == CheckMaintainResult.NG || p.MaintainResult == CheckMaintainResult.Unright || p.ActualValue.HasValue)
                    p.ExeState = MaintExeState.Performing;
            });


            //没有执行更换的备件项目，不保存申请单数据
            if (bill.MaintainPlanSparePartList.Any(p => p.State == ChangeSparePartState.New && p.PartOutDepotDetailId.HasValue))
            {
                throw new ValidationException("【备件更换】选择了【备件出库单明细】时，必须完成【更换】操作或清除【备件出库单明细】的值才能保存。".L10N());
            }

            //工时登记
            //判断工时登记是否可为空
            var config2 = ConfigService.GetConfig(new MaintainWorkTimeConfig(), typeof(MaintainPlanViewModel));
            double workHours = 0;
            if (config2.IsMaintainForWorkTime == YesNo.Yes)
            {
                foreach (WorkHoursRegister workHoursRegister in bill.WorkHoursRegisterList)
                {
                    if (workHoursRegister.BeginDay > workHoursRegister.EndDay)
                        throw new ValidationException("工时登记保养结束时间不能比开始时间早，不允许提交。".L10N());

                    DateTime dt1 = Convert.ToDateTime(workHoursRegister.EndDay);
                    DateTime dt2 = Convert.ToDateTime(workHoursRegister.BeginDay);
                    TimeSpan ts1 = dt1.Subtract(dt2);
                    workHoursRegister.WorkHours = Math.Round(ts1.TotalHours, 2);
                    workHours += workHoursRegister.WorkHours;
                }
            }
            else
            {
                if (bill.ActEndDate != null && bill.ActBeginDate != null)
                {
                    DateTime dt1 = Convert.ToDateTime(bill.ActEndDate);
                    DateTime dt2 = Convert.ToDateTime(bill.ActBeginDate);
                    TimeSpan ts1 = dt1.Subtract(dt2);
                    workHours = Math.Round(ts1.TotalHours, 2);
                }


            }
            bill.SumWorkHours = workHours;

            base.DoSave(bill);
        }
    }
}
