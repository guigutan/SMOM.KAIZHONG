using SIE.EMS.EquipRepair.ApiModels;
using SIE.EMS.EquipRepair.Controller;
using SIE.EMS.EquipRepair.EquipRepairs;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using SIE.Domain;
using SIE.EMS.EquipRepair.EquipRepairs.Enums;
using SIE.EMS.DevicePurs;
using SIE.Resources.Employees;
using SIE.Domain.Validation;

namespace SIE.Web.EMS.EquipRepair.EquipRepairs.Commands
{
    /// <summary>
  /// 接单采购订单
  /// </summary>
    [JsCommand("SIE.Web.EMS.EquipRepair.EquipRepairs.Commands.TakeOrderCommand")]
    public class TakeOrderCommand : ViewCommand<ViewArgs>
    {
        /// <summary>
        /// 接单
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>bool</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var repairBill = args.Data.ToJsonObject<EquipRepairBill>();
            
            var repairInfo = new TakeRepairInfo();
            repairInfo.RepairBillId = repairBill.Id;
            repairInfo.RepairMasterId = (double)repairBill.RepairMasterId;
            repairInfo.EstimateFinishDate = repairBill.EstimateFinishDate;

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

            RT.Service.Resolve<RepairController>().TakeRepair(repairInfo);
            return true;
        }
    }
}
