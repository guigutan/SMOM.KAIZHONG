using SIE.Domain;
using SIE.EMS.EquipRepair.EquipRepairs.Enums;
using SIE.EMS.EquipRepair.EquipRepairs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIE.EMS.EquipRepairs.Enums;
using SIE.EMS.EquipRepair.ApiModels;

namespace SIE.EMS.EquipRepair.Controller
{
    /// <summary>
    /// 维修管理控制器(优化)
    /// </summary>
    public partial class RepairController : DomainController
    {
        #region 构建
        /// <summary>
        /// 生成设备维修操作记录
        /// </summary>
        /// <param name="type">操作类型</param>
        /// <param name="repairBillId">维修单ID</param>
        /// <param name="operationTime">操作时间</param>
        /// <param name="remark">备注</param>
        /// <param name="originalRepairMasterId">原维修责任人</param>
        /// <param name="originalRepairer">原维修人员</param>
        /// <param name="handoverConfirmResult">交机确认结果</param>
        /// <param name="engineerConfirmResult">工程确认结果</param>
        public virtual EquipRepairOperationRec GenerateOperationReccordReturn(RepairOperationType type, double repairBillId, DateTime operationTime
            , string remark = null, double? originalRepairMasterId = null, string originalRepairer = null, HandoverConfirmResult? handoverConfirmResult = null, EngineerConfirmResult? engineerConfirmResult = null)
        {
            //构建维修记录实体数据
            EquipRepairOperationRec operationRec = new EquipRepairOperationRec();
            operationRec.OperationType = type;
            operationRec.EquipRepairBillId = repairBillId;
            operationRec.OperationerId = RT.IdentityId;
            operationRec.OperationDate = operationTime;
            operationRec.Remark = remark;
            operationRec.OriginalRepairMasterId = originalRepairMasterId;
            operationRec.OriginalRepairer = originalRepairer;
            operationRec.HandoverConfirmResult = handoverConfirmResult;
            operationRec.EngineerConfirmResult = engineerConfirmResult;
            operationRec.IntervalTime = this.GetOperationIntervalTime(type, repairBillId, operationTime);
            return operationRec;
        }

        /// <summary>
        /// 生成设备维修工时记录
        /// </summary>
        /// <param name="repairBill">维修单</param>
        /// <param name="repairerId">维修人员ID</param>
        /// <param name="isMaster">是否维修责任人</param>
        /// <param name="beginDateTime">开始时间</param>
        /// <param name="endDateTime">结束时间</param>
        public virtual EquipRepairWorkingHours GenerateWorkingHourReturn(RepairCommonInfo repairBill, double? repairerId, bool isMaster, DateTime? beginDateTime = null, DateTime? endDateTime = null)
        {
            EquipRepairWorkingHours workingHour = new EquipRepairWorkingHours();
            workingHour.EquipRepairBillId = repairBill.Id;
            workingHour.RepairerId = repairerId;
            workingHour.IsRepairMaster = isMaster;
            workingHour.BeginTime = repairBill.State == EquipRepairState.WaitRepair ? null : beginDateTime;
            workingHour.EndTime = repairBill.State == EquipRepairState.WaitRepair ? null : endDateTime;
            workingHour.IsRepairEmployee = true;
            workingHour.PersistenceStatus = PersistenceStatus.New;
            return workingHour;
        }
        #endregion


        #region 业务逻辑
        /// <summary>
        /// 生成设备维修工时记录
        /// </summary>
        /// <param name="repairBillId">维修单ID</param>
        /// <param name="repairMasterId">维修责任人ID</param>
        /// <param name="repairerIds">维修人员ID列表</param>
        public virtual EntityList<EquipRepairWorkingHours> GenerateWorkingHoursReturn(double repairBillId, double repairMasterId, List<double> repairerIds)
        {
            EntityList<EquipRepairWorkingHours> saveList = new EntityList<EquipRepairWorkingHours>();
            RepairCommonInfo repairEquipInfo = Query<EquipRepairBill>().Where(p => p.Id == repairBillId).Select(p => new { Id = p.Id, State = p.RepairState }).FirstOrDefault<RepairCommonInfo>();
            // 维修责任人维修工时
            saveList.Add(GenerateWorkingHourReturn(repairEquipInfo, repairMasterId, true));
            // 其他维修人员维修工时
            foreach (var id in repairerIds)
            {
                saveList.Add(GenerateWorkingHourReturn(repairEquipInfo, id, false));
            }
            return saveList;
        }
        #endregion



    }
}
