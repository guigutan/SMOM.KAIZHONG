using SIE.EMS.EquipRepair.ApiModels;
using SIE.EMS.EquipRepair.Controller;
using SIE.EMS.EquipRepair.EquipRepairs;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using SIE.Resources.Employees;
using SIE.Domain;

namespace SIE.Web.EMS.EquipRepair.EquipRepairs.Commands
{
    /// <summary>
    /// 维修单转派命令
    /// </summary>
    public class TransferEquipRepairCommand : ViewCommand<ViewArgs>
    {

        /// <summary>
        /// 维修单转派命令执行
        /// </summary>
        /// <param name="args">视图参数</param>
        /// <param name="scope">命令参数</param>
        /// <returns>返回参数</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var repairBill = args.Data.ToJsonObject<EquipRepairBill>();

            //构建转派参数
            var repairInfo = new TransfeRepairInfo();
            repairInfo.RepairBillId = repairBill.Id;
            repairInfo.RepairMasterId = (double)repairBill.RepairMasterId;
            repairInfo.EstimateFinishDate = repairBill.EstimateFinishDate;
            repairInfo.RepairWay = (int)repairBill.RepairWay;
            repairInfo.SupplierId = repairBill.SupplierId;
            repairInfo.SendRepairWay = (int?)repairBill.SendRepairWay;
            repairInfo.DeliveryNo = repairBill.DeliveryNo;
            repairInfo.ContactPerson = repairBill.ContactPerson;
            repairInfo.ContactPhone = repairBill.ContactPhone;
            repairInfo.SendRepairDate = repairBill.SendRepairDate;
            repairInfo.PredictBackDate = repairBill.PredictBackDate;
            repairInfo.Remark = repairBill.TransferReason;
            repairInfo.OriginalRepairMasterId = (double)repairBill.OriginalRepairMasterId;

            //通过编码获取维修人员
            List<RepairerResultInfo> repairers = new List<RepairerResultInfo>();

            //维修人员可为空
            if (!repairBill.RepairEmployeeIds.IsNullOrEmpty())
            {
                //维修人员不为空时，才获取维修人员的员工信息

                //获取新的维修人员
                EntityList<Employee> employees = null;

                employees = RT.Service.Resolve<RepairController>()
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

            //通过编码获取原维修人员
            List<RepairerResultInfo> originalRepairers = new List<RepairerResultInfo>();

            //维修人员可为空
            if (!repairBill.OriginalRepairEmployeeIds.IsNullOrEmpty())
            {
                //维修人员不为空时，才获取维修人员的员工信息
                var originalEmployees = RT.Service.Resolve<RepairController>()
                .GetEmployeeListByIds(repairBill.OriginalRepairEmployeeIds);
                originalEmployees.ForEach(employee =>
                {
                    originalRepairers.Add(new RepairerResultInfo()
                    {
                        EmployeeId = employee.Id,
                        EmployeeCode = employee.Code,
                        EmployeeName = employee.Name
                    });
                });
            }

            repairInfo.OriginalRepairers = originalRepairers;

            RT.Service.Resolve<RepairController>().TransferRepair(repairInfo);

            return true;
        }
    }
}
