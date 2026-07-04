using SIE.Domain;
using SIE.EMS.Checks.ApiModels;
using SIE.EMS.Checks.Plans;
using SIE.EMS.DataAuth;
using SIE.EMS.DevicePurs;
using SIE.EMS.Enums;
using SIE.Rbac.Users;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.EMS.Checks
{
    /// <summary>
    /// 点检PDA首页统计
    /// </summary>
    public partial class CheckController : DomainController
    {
        /// <summary>
        /// 根据点检执行状态统计各个状态的数量
        /// </summary>
        /// <param name="exeStates">点检执行状态集合</param>
        /// <returns></returns>
        public virtual List<CheckPDACountInfo> GetCheckExePDAHomeInfos(List<CheckExeState> exeStates)
        {
            List<CheckPDACountInfo> checkPDACountInfos = new List<CheckPDACountInfo>();

            // 过滤责任部门权限
            var deptIds = RT.Service.Resolve<DevicePurController>().GetDutyDepartmentIds(RT.Identity.UserId).Cast<double?>();

            // 只获取可执行的数据
            var now = RF.Find<CheckPlan>().GetDbTime();
            var q = Query<CheckPlan>().Where(p => p.CheckBeginDate <= now // 
                && exeStates.Contains(p.ExeState) // 包含的状态
                && (deptIds.Contains(p.DepartmentId) || p.DepartmentId == null)) // 拥有责任部门或责任部门为空
                .Select(p => new {Id = p.Id, Exe_State = p.ExeState, Department_Id = p.DepartmentId });

            var query = q.ToQuery();
            query.QueryWithEquipAccountPermissions(CheckPlan.EquipAccountIdProperty.Name);

            var result = q.Repository.QueryList(query);

            foreach (var item in result)
            {
                var plan = item as CheckPlan;
                CheckPDACountInfo info = new CheckPDACountInfo
                {
                    Id = plan.Id,
                    CheckExeState = plan.ExeState,
                    DepartmentId = plan.DepartmentId,
                };
                checkPDACountInfos.Add(info);
            }

            return checkPDACountInfos;
        }

        /// <summary>
        /// 获取点检确认项
        /// </summary>
        /// <param name="query">点检计划信息</param>
        /// <returns></returns>
        public virtual List<CheckPDACountInfo> GetCheckCfmPDAHomeInfos(List<CheckPDACountInfo> query)
        {
            List<CheckPDACountInfo> confirmInfos = new List<CheckPDACountInfo>();
            // 确认部门权限
            var dptList = Query<DeviceDepa>()
                 .LeftJoin<DevicePur>((a, b) => a.DevicePurId == b.Id)
                 .LeftJoin<DevicePur, UserInUserGroup>((a, b) => a.UserGroupId == b.UserGroupId)
                 .Where<DevicePur, UserInUserGroup>((a, b, c) => (b.UserId == RT.Identity.UserId || c.UserId == RT.Identity.UserId) && b.CheckConfirm)
                 .Select(a => new
                 {
                     DepartmentId = a.EnterpriseId,
                 }).ToList<double>().ToList();

            if (dptList.Count <= 0)
            {
                return new List<CheckPDACountInfo>();
            }

            var planIds = query.Where(p => p.CheckExeState == CheckExeState.NotConfirm || p.CheckExeState == CheckExeState.Scored || p.CheckExeState == CheckExeState.Confirmed).Select(p => p.Id).ToList();
            // 点检确认项
            planIds.SplitDataExecute(temps => {
                var list = Query<CheckPlanConfirmItem>().Where(p => temps.Contains(p.CheckPlanId) && dptList.Contains(p.DepartmentId))
                .Select(p => new
                {
                    Id = p.Id,
                    ConfirmDptId = p.DepartmentId,
                    CheckExeState = p.CheckExeState,
                }).ToList<CheckPDACountInfo>().ToList();
                confirmInfos.AddRange(list);
            });

            return confirmInfos;
        }
    }
}
