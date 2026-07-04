using SIE.Domain;
using SIE.EMS.DevicePurs;
using SIE.EMS.EquipRepair.ApiModels;
using SIE.EMS.EquipRepair.EquipRepairs;
using SIE.EMS.EquipRepair.EquipRepairs.Enums;
using SIE.EMS.EquipRepairs.Enums;
using SIE.Fixtures.Repairs;
using SIE.Rbac.Users;
using System.Collections.Generic;
using System.Linq;

namespace SIE.EMS.EquipRepair.Controller
{
    /// <summary>
    /// 维修PDA首页统计
    /// </summary>
    public partial class RepairController : DomainController
    {
        /// <summary>
        /// 根据维修状态统计各个状态的统计
        /// </summary>
        /// <returns></returns>
        public virtual List<RepairPDACountInfo> GetRepairPDAHomeInfos()
        {
            List<RepairPDACountInfo> equipRepairStates = new List<RepairPDACountInfo>();

            // 维修执行RepairOperationType == 4 Exists维修工时
            List<EquipRepairState> exeStates = new List<EquipRepairState> { EquipRepairState.WaitRepair, EquipRepairState.Repairing, EquipRepairState.Suspending };

            // 交机确认、工程确认RepairOperationType == 6，7
            List<EquipRepairState> cfmStates = new List<EquipRepairState> { EquipRepairState.WaitConfirm, EquipRepairState.WaitScore };

            // 接单RepairOperationType == 1 Exists设备与人员
            List<EquipRepairState> takeStates = new List<EquipRepairState> { EquipRepairState.ApplyRepair };

            var cfmInfos = Query<EquipRepairBill>()
                .Where(p => cfmStates.Contains(p.RepairState))
                .GroupBy(p => new { p.RepairState })
                .Select(p => new { State = p.RepairState, Count = p.RepairState.COUNT()}).ToList<StateCount>();
            // 交机确认
            var handCount = cfmInfos.FirstOrDefault(p => p.State == EquipRepairState.WaitConfirm);
            RepairPDACountInfo handoverInfo = new RepairPDACountInfo
            {
                Type = RepairOperationType.HandoverConfirm,
                State = EquipRepairState.WaitConfirm,
                Count = handCount != null ? handCount.Count : 0,
            };
            equipRepairStates.Add(handoverInfo);

            // 工程确认
            var cfmCount = cfmInfos.FirstOrDefault(p => p.State == EquipRepairState.WaitScore);
            RepairPDACountInfo engineerInfo = new RepairPDACountInfo
            {
                Type = RepairOperationType.EngineerConfirm,
                State = EquipRepairState.WaitScore,
                Count = cfmCount != null ? cfmCount.Count : 0,
            };
            equipRepairStates.Add(engineerInfo);

            var taksInfosCount = Query<EquipRepairBill>().Where(p => takeStates.Contains(p.RepairState))
                .Exists<DevicePur>((x, y) => y.LeftJoin<UserInUserGroup>((a, b) => a.UserGroupId == b.UserGroupId).Where<UserInUserGroup>((a, b) => (a.UserId == RT.Identity.UserId || b.UserId == RT.Identity.UserId) && a.EquipMaintain))
                .Count();
            // 接单
            RepairPDACountInfo takeInfo = new RepairPDACountInfo
            {
                Type = RepairOperationType.Take,
                State = EquipRepairState.ApplyRepair,
                Count = taksInfosCount,
            };
            equipRepairStates.Add(takeInfo);

            var exeInfos = Query<EquipRepairBill>().Where(p => exeStates.Contains(p.RepairState))
                .Exists<EquipRepairWorkingHours>((x, y) => y.Where(p => p.EquipRepairBillId == x.Id && p.IsRepairEmployee && p.RepairerId == RT.IdentityId))
                .GroupBy(p => new { p.RepairState })
                .Select(p => new { State = p.RepairState, Count = p.RepairState.COUNT() })
                .ToList<StateCount>();
            // 执行
            foreach (var state in exeStates)
            {
                var stateCount = exeInfos.FirstOrDefault(p => p.State == state);
                RepairPDACountInfo exeInfo = new RepairPDACountInfo
                {
                    Type = RepairOperationType.Begin,
                    State = state,
                    Count = stateCount != null ? stateCount.Count : 0,
                };
                equipRepairStates.Add(exeInfo);
            }

            return equipRepairStates;
        }
    }
}
