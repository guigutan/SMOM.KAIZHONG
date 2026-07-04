using SIE.Api;
using SIE.Domain;
using SIE.EMS.Checks;
using SIE.EMS.Checks.ApiModels;
using SIE.EMS.Checks.Confirmations;
using SIE.EMS.DevicePurs;
using SIE.EMS.Enums;
using  SIE.EMS.Maintains.ApiModels;
using SIE.EMS.Maintains.Controller;
using SIE.EMS.EquipRepair.ApiModels;
using SIE.EMS.EquipRepair.Controller;
using SIE.EMS.EquipRepair.EquipRepairs;
using SIE.EMS.EquipRepair.EquipRepairs.Enums;
using SIE.EMS.Maintains.Confirmations;
using System;
using System.Collections.Generic;
using System.Linq;
using SIE.EMS.Lubrications;
using SIE.Equipments.EquipAccounts;
using SIE.EMS.DataAuth;
using SIE.EMS.EquipRepairs.Enums;

namespace SIE.EMS.API.APIs
{
    /// <summary>
    /// 用户管理接口
    /// </summary>
    public class UserManageApiController : DomainController
    {
        /// <summary>
        /// 用户管理的单据数量
        /// </summary>
        /// <param name="menuInfo">菜单信息</param>
        /// <returns></returns>
        [ApiService("用户管理的单据数量")]
        public virtual UserManageBillCountInfo GetUserManageBillCount([ApiParameter("菜单信息")] MenuInfo menuInfo)
        {
            UserManageBillCountInfo userManageBillCountInfo = new UserManageBillCountInfo()
            {
                PendingBillInfo = new PendingBillInfo() { BillInfoList = new List<BillInfo>() },
                ProcessingBillInfo = new PendingBillInfo() { BillInfoList = new List<BillInfo>() },
                CompleteBillInfo = new ComplateBillInfo() { BillInfoList = new List<BillInfo>() }
            };

            #region 点检
            GetCheckInfo(menuInfo, userManageBillCountInfo);

            #endregion

            #region 保养
            GetMaintainInfo(menuInfo, userManageBillCountInfo);

            #endregion

            #region 维修
            GetRepairInfo(menuInfo, userManageBillCountInfo);
            #endregion

            #region 润滑
            GetLubricationInfo(menuInfo, userManageBillCountInfo);
            #endregion

            #region 待处理

            foreach (var item in userManageBillCountInfo.PendingBillInfo.BillInfoList)
            {
                userManageBillCountInfo.PendingBillInfo.BillCount += item.Count;
            }
            #endregion

            #region 处理中


            foreach (var item in userManageBillCountInfo.ProcessingBillInfo.BillInfoList)
            {
                userManageBillCountInfo.ProcessingBillInfo.BillCount += item.Count;
            }
            #endregion

            #region 已处理

            foreach (var item in userManageBillCountInfo.CompleteBillInfo.BillInfoList)
            {
                userManageBillCountInfo.CompleteBillInfo.BillCount += item.Count;
            }
            #endregion

            return userManageBillCountInfo;
        }

        /// <summary>
        /// 获取维修统计信息
        /// </summary>
        /// <param name="menuInfo"></param>
        /// <param name="userManageBillCountInfo"></param>
        private void GetRepairInfo(MenuInfo menuInfo, UserManageBillCountInfo userManageBillCountInfo)
        {
            List<RepairPDACountInfo> repairPDACountInfos = RT.Service.Resolve<RepairController>().GetRepairPDAHomeInfos();

            #region 待执行维修执行
            //待维修的记录
            var toRepairCount = repairPDACountInfos.Where(p => p.State == EquipRepairState.WaitRepair && p.Type == RepairOperationType.Begin).Sum(p => p.Count);

            userManageBillCountInfo.PendingBillInfo.BillInfoList.Add(new BillInfo()
            {
                Code = menuInfo.PendingMenuInfo.RepairApplyRepair,
                Count = toRepairCount
            });
            #endregion

            #region 处理中维修执行
            //维修中、暂停的记录
            var stopAndRepairingCount = repairPDACountInfos.Where(p => (p.State == EquipRepairState.Repairing || p.State == EquipRepairState.Suspending) && p.Type == RepairOperationType.Begin).Sum(p => p.Count);
            userManageBillCountInfo.ProcessingBillInfo.BillInfoList.Add(new BillInfo()
            {
                Code = menuInfo.ProcessingMenuInfo.Repairing,
                Count = stopAndRepairingCount
            });
            #endregion

            #region 已完成维修执
            //已完成的记录
            var completedCount = repairPDACountInfos.Where(p => p.State == EquipRepairState.Completed && p.Type == RepairOperationType.Begin).Sum(p => p.Count);

            userManageBillCountInfo.CompleteBillInfo.BillInfoList.Add(new BillInfo()
            {
                Code = menuInfo.CompletedMenuInfo.RepairCompleted,
                Count = completedCount
            });
            #endregion



            //维修交机确认
            var waitConfirmCount = repairPDACountInfos.Where(p => p.State == EquipRepairState.WaitConfirm && p.Type == RepairOperationType.HandoverConfirm).Sum(p => p.Count);
            userManageBillCountInfo.PendingBillInfo.BillInfoList.Add(new BillInfo()
            {
                Code = menuInfo.PendingMenuInfo.RepairWaitConfirm,
                Count = waitConfirmCount
            });

            // 维修工程确认
            var scoreConfirmCount = repairPDACountInfos.Where(p => p.State == EquipRepairState.WaitScore && p.Type == RepairOperationType.EngineerConfirm).Sum(p => p.Count);
            userManageBillCountInfo.PendingBillInfo.BillInfoList.Add(new BillInfo()
            {
                Code = menuInfo.PendingMenuInfo.EngineerConfirm,
                Count = scoreConfirmCount
            });

            //维修接单的记录
            var repairRecords_ReceiveCount = repairPDACountInfos.Where(p => p.State == EquipRepairState.ApplyRepair && p.Type == RepairOperationType.Take).Sum(p => p.Count);
            userManageBillCountInfo.PendingBillInfo.BillInfoList.Add(new BillInfo()
            {
                Code = menuInfo.PendingMenuInfo.RepairReceive,
                Count = repairRecords_ReceiveCount
            });

        }

        /// <summary>
        /// 获取保养信息
        /// </summary>
        /// <param name="menuInfo"></param>
        /// <param name="userManageBillCountInfo"></param>
        private void GetMaintainInfo(MenuInfo menuInfo, UserManageBillCountInfo userManageBillCountInfo)
        {
            List<MaintExeState> states = new List<MaintExeState> { MaintExeState.NotPerformed, MaintExeState.Overdue, MaintExeState.Performing, MaintExeState.Performed,
                MaintExeState.NotConfirm, MaintExeState.Scored, MaintExeState.Confirmed };
            var maintainExeInfos = RT.Service.Resolve<MaintainController>().GetMaintainExePDAHomeInfos(states);
            #region 保养执行
            //未执行的记录
            var notPerformedCount = maintainExeInfos.Count(p => p.MaintExeState == MaintExeState.NotPerformed || p.MaintExeState == MaintExeState.Overdue);
            userManageBillCountInfo.PendingBillInfo.BillInfoList.Add(new BillInfo()
            {
                Code = menuInfo.PendingMenuInfo.MaintainNotPerformed,
                Count = notPerformedCount
            });

            //执行中的记录
            var performingCount = maintainExeInfos.Count(p => p.MaintExeState == MaintExeState.Performing);
            userManageBillCountInfo.ProcessingBillInfo.BillInfoList.Add(new BillInfo()
            {
                Code = menuInfo.ProcessingMenuInfo.Maintaining,
                Count = performingCount
            });

            //已执行的记录
            var performedCount = maintainExeInfos.Count(p => p.MaintExeState == MaintExeState.Performed);
            #endregion

            #region 保养确认
            var maintainCfmInfos = RT.Service.Resolve<MaintainController>().GetMaintainCfmPDAHomeInfos(maintainExeInfos);

            // 待确认的保养确认
            var notConfirmCount = maintainCfmInfos.Count(p => p.MaintExeState == MaintExeState.NotConfirm);
            userManageBillCountInfo.PendingBillInfo.BillInfoList.Add(new BillInfo()
            {
                Code = menuInfo.PendingMenuInfo.MaintainNotConfirm,
                Count = notConfirmCount
            });


            // 已评分、已确认的保养确认
            var hasConfirmCount = maintainCfmInfos.Count(p => p.MaintExeState == MaintExeState.Scored || p.MaintExeState == MaintExeState.Confirmed);

            userManageBillCountInfo.CompleteBillInfo.BillInfoList.Add(new BillInfo()
            {
                Code = menuInfo.CompletedMenuInfo.MaintainPerformed,
                Count = performedCount + hasConfirmCount
            });
            #endregion

        }

        /// <summary>
        /// 获取点检信息
        /// </summary>
        /// <param name="menuInfo"></param>
        /// <param name="userManageBillCountInfo"></param>
        private void GetCheckInfo(MenuInfo menuInfo, UserManageBillCountInfo userManageBillCountInfo)
        {
            List<CheckExeState> states = new List<CheckExeState> { CheckExeState.NotPerformed, CheckExeState.Overdue, CheckExeState.Performing, CheckExeState.Performed,
                CheckExeState.NotConfirm, CheckExeState.Scored, CheckExeState.Confirmed };
            var checkExeInfos = RT.Service.Resolve<CheckController>().GetCheckExePDAHomeInfos(states);

            #region 点检执行
            // 未执行的点检执行(未执行、超期)
            var notPerformedCount = checkExeInfos.Count(p => p.CheckExeState == CheckExeState.NotPerformed || p.CheckExeState == CheckExeState.Overdue);

            userManageBillCountInfo.PendingBillInfo.BillInfoList.Add(new BillInfo()
            {
                Code = menuInfo.PendingMenuInfo.CheckNotPerformed,
                Count = notPerformedCount
            });


            // 执行中的点检执行(执行中)
            var performingCount = checkExeInfos.Count(p => p.CheckExeState == CheckExeState.Performing);
            userManageBillCountInfo.ProcessingBillInfo.BillInfoList.Add(new BillInfo()
            {
                Code = menuInfo.ProcessingMenuInfo.Checking,
                Count = performingCount
            });

            // 已执行的点检执行(已执行)
            var performedCount = checkExeInfos.Count(p => p.CheckExeState == CheckExeState.Performed);
            #endregion


            #region 点检确认
            var checkCfmInfos = RT.Service.Resolve<CheckController>().GetCheckCfmPDAHomeInfos(checkExeInfos);

            // 待确认的点检确认
            var notConfirmCount = checkCfmInfos.Count(p => p.CheckExeState == CheckExeState.NotConfirm);
            userManageBillCountInfo.PendingBillInfo.BillInfoList.Add(new BillInfo()
            {
                Code = menuInfo.PendingMenuInfo.CheckNotConfirm,
                Count = notConfirmCount
            });


            // 已评分、已确认的点检确认
            var hasConfirmCount = checkCfmInfos.Count(p => p.CheckExeState == CheckExeState.Scored || p.CheckExeState == CheckExeState.Confirmed);

            userManageBillCountInfo.CompleteBillInfo.BillInfoList.Add(new BillInfo()
            {
                Code = menuInfo.CompletedMenuInfo.CheckPerformed,
                Count = performedCount + hasConfirmCount
            });
            #endregion
        }

        /// <summary>
        /// 获取润滑信息
        /// </summary>
        /// <param name="menuInfo">菜单信息</param>
        /// <param name="billInfo">用户管理的单据数量</param>        
        private void GetLubricationInfo(MenuInfo menuInfo, UserManageBillCountInfo billInfo)
        {
            List<LubricationStatus> states = new List<LubricationStatus> { LubricationStatus.Pending, LubricationStatus.Doing, LubricationStatus.Done };

            var query = RT.Service.Resolve<LubricationController>().GetLubPDAHomeInfo(states);


            //未执行的记录
            var pendingCount = query.Count(p => p == LubricationStatus.Pending);
            billInfo.PendingBillInfo.BillInfoList.Add(new BillInfo()
            {
                Code = menuInfo.PendingMenuInfo.LubricationNotPerformed,
                Count = pendingCount
            });

            //执行中的记录
            var doingCount = query.Count(p => p == LubricationStatus.Doing);
            billInfo.ProcessingBillInfo.BillInfoList.Add(new BillInfo()
            {
                Code = menuInfo.ProcessingMenuInfo.Lubricationing,
                Count = doingCount
            });

            //已执行的记录
            var doneCount = query.Count(p => p == LubricationStatus.Done);
            billInfo.CompleteBillInfo.BillInfoList.Add(new BillInfo()
            {
                Code = menuInfo.CompletedMenuInfo.LubricationPerformed,
                Count = doneCount
            });
        }
    }
}
