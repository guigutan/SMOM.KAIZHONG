using SIE.Domain;
using SIE.EMS.Checks.Plans;
using SIE.EMS.DataAuth;
using SIE.EMS.DevicePurs;
using SIE.EMS.Enums;
using SIE.EMS.Maintains.ApiModels;
using SIE.EMS.Maintains.Plans;
using SIE.Rbac.Users;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.EMS.Maintains.Controller
{
    /// <summary>
    /// 保养PDA首页统计
    /// </summary>
    public partial class MaintainController : DomainController
    {
        /// <summary>
        /// 根据保养执行状态统计各个状态的数量
        /// </summary>
        /// <param name="stateList">保养状态</param>
        /// <returns></returns>
        public virtual List<MaintainPDACountInfo> GetMaintainExePDAHomeInfos(List<MaintExeState> stateList)
        {
            List<MaintainPDACountInfo> maintainPDACountInfos = new List<MaintainPDACountInfo>();

            // 过滤责任部门权限
            var deptIds = RT.Service.Resolve<DevicePurController>().GetDutyDepartmentIds(RT.Identity.UserId).Cast<double?>();

            // 只获取可执行的数据
            var now = RF.Find<CheckPlan>().GetDbTime();
            var q = Query<MaintainPlan>().Where(p => p.PlanBeginDate <= now && stateList.Contains(p.ExeState) 
            && (deptIds.Contains(p.DepartmentId) || p.DepartmentId == null))
                .Select(p => new
                {
                    Id = p.Id,
                    Exe_State = p.ExeState,
                    Department_Id = p.DepartmentId,
                });

            var query = q.ToQuery();
            query.QueryWithEquipAccountPermissions(MaintainPlan.EquipAccountIdProperty.Name);

            var result = q.Repository.QueryList(query);

            foreach (var item in result )
            {
                var plan = item as MaintainPlan;
                MaintainPDACountInfo info = new MaintainPDACountInfo
                {
                    Id = plan.Id,
                    MaintExeState = plan.ExeState,
                    DepartmentId = plan.DepartmentId,
                };
                maintainPDACountInfos.Add(info);
            }

            return maintainPDACountInfos;
        }

        /// <summary>
        /// 获取保养确认项
        /// </summary>
        /// <param name="query">保养计划信息</param>
        /// <returns></returns>
        public virtual List<MaintainPDACountInfo> GetMaintainCfmPDAHomeInfos(List<MaintainPDACountInfo> query)
        {
            List<MaintainPDACountInfo> confirmInfos = new List<MaintainPDACountInfo>();

            var dptList = Query<DeviceDepa>()
                 .LeftJoin<DevicePur>((a, b) => a.DevicePurId == b.Id)
                 .LeftJoin<DevicePur, UserInUserGroup>((a, b) => a.UserGroupId == b.UserGroupId)
                 .Where<DevicePur, UserInUserGroup>((a, b, c) => (b.UserId == RT.Identity.UserId || c.UserId == RT.Identity.UserId) && b.MaintainConfirm)
                 .Select(a => new
                 {
                     DepartmentId = a.EnterpriseId,
                 }).ToList<double>().ToList();

            if (dptList.Count <= 0)
            {
                return new List<MaintainPDACountInfo>();
            }

            var planIds = query.Where(p => p.MaintExeState == MaintExeState.NotConfirm || p.MaintExeState == MaintExeState.Scored || p.MaintExeState == MaintExeState.Confirmed).Select(p => p.Id).ToList();
            // 保养确认项
            planIds.SplitDataExecute(temps => {
                var list = Query<MaintainPlanConfirmItem>().Where(p => temps.Contains(p.MaintainPlanId) && dptList.Contains(p.DepartmentId))
                .Select(p => new
                {
                    Id = p.Id,
                    ConfirmDptId = p.DepartmentId,
                    MaintExeState = p.MaintExeState,
                }).ToList<MaintainPDACountInfo>().ToList();
                confirmInfos.AddRange(list);
            });

            return confirmInfos;
        }
    }
}
