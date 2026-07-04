using SIE.Domain.Validation;
using SIE.EMS.EquipRepair.ApiModels;
using SIE.EMS.EquipRepair.Controller;
using SIE.EMS.EquipRepair.EquipRepairs;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.EMS.EquipRepair.EquipRepairs.Commands
{
    /// <summary>
    /// 维修单派工命令
    /// </summary>
    public class DispatchEquipRepairCommand : ViewCommand<ViewArgs>
    {

        /// <summary>
        /// 维修单派工命令执行
        /// </summary>
        /// <param name="args">视图参数</param>
        /// <param name="scope">命令参数</param>
        /// <returns>返回参数</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var repairBill = args.Data.ToJsonObject<EquipRepairBill>();
            var repairInfo = new DispatchRepairInfo();
            repairInfo.RepairBillId = repairBill.Id;

            if (!repairBill.RepairMasterId.HasValue)
            {
                throw new ValidationException("请填写【维修责任人】".L10N());
            }

            if (repairBill.EstimateFinishDate <= DateTime.Now)
            {
                throw new ValidationException("【预计完成时间】不能小于当前时间".L10N());
            }

            repairInfo.RepairMasterId = repairBill.RepairMasterId.Value;
            repairInfo.EstimateFinishDate = repairBill.EstimateFinishDate;
            if (!repairBill.RepairWay.HasValue)
            {
                throw new ValidationException("请填写【派工类型】".L10N());
            }
            repairInfo.RepairWay =  (int)repairBill.RepairWay.Value;
            repairInfo.SupplierId = repairBill.SupplierId;
            repairInfo.SendRepairWay = (int?)repairBill.SendRepairWay;
            repairInfo.DeliveryNo = repairBill.DeliveryNo;
            repairInfo.ContactPerson = repairBill.ContactPerson;
            repairInfo.ContactPhone = repairBill.ContactPhone;
            repairInfo.SendRepairDate = repairBill.SendRepairDate;
            repairInfo.PredictBackDate = repairBill.PredictBackDate;
            repairInfo.ProjectId = repairBill.ProjectId;
            repairInfo.ProjectKeyItemId = repairBill.ProjectKeyItemId;

            //通过编码获取维修人员
            List<RepairerResultInfo> repairers = new List<RepairerResultInfo>();

            //维修人员可为空
            if (!repairBill.RepairEmployeeIds.IsNullOrEmpty())
            {
                //维修人员不为空时，才获取维修人员的员工信息

                var employees = RT.Service.Resolve<RepairController>()
                    .GetEmployeeListByIdsString(repairBill.RepairEmployeeIds);

                employees.ForEach(employee =>
                {
                    repairers.Add(new RepairerResultInfo()
                    {
                        EmployeeId = employee.Id,
                        EmployeeCode = employee.Code,
                        EmployeeName = employee.Name
                    });
                });
            }

            repairInfo.Repairers = repairers;
            RT.Service.Resolve<RepairController>().DispatchRepair(repairInfo);
            return true;
        }
    }
}
