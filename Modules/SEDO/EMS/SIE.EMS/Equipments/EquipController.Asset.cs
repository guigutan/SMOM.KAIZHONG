using SIE.Domain;
using SIE.EMS.Equipments.Accounts;
using SIE.EMS.MainenanceProjects;
using SIE.Equipments.EquipAccounts;
using System.Collections.Generic;
using System.Linq;

namespace SIE.EMS.Equipments
{
    /// <summary>
    /// 固定资产控制器
    /// </summary>
    public partial class EquipController : DomainController
    {
        /// <summary>
        /// 根据设备台账Id和周期类型获取设备台账保养项目列表
        /// </summary>
        /// <param name="accountId">设备台账Id</param>
        /// <param name="cycleType">周期类型</param>
        /// <returns>设备台账保养项目列表</returns>
        public virtual EntityList<EquipAccountMaintainProject> GetProjectByAccount(double accountId, int cycleType)
        {
            var query = Query<EquipAccountMaintainProject>();
            query.Exists<EquipAccount>((a, b) => b.Where(p => p.Id == a.EquipAccountId && p.Id == accountId));
            query.Exists<ProjectDetail>((a, b) => b.Where(p => p.Id == a.ProjectDetailId && p.CycleType == (CycleType)cycleType));
            return query.ToList(null, new EagerLoadOptions().LoadWithViewProperty().LoadWith(EquipAccountMaintainProject.ProjectDetailProperty));
        }

        /// <summary>
        /// 根据设备台账Id和周期类型获取设备台账保养项目列表
        /// </summary>
        /// <param name="accountId">设备台账Id</param>
        /// <param name="cycleType">周期类型</param>
        /// <returns>设备台账保养项目列表</returns>
        public virtual EntityList<EquipAccountCheckProject> GetCheckProjectsByAccount(double accountId, int cycleType)
        {
            var query = Query<EquipAccountCheckProject>();
            query.Exists<EquipAccount>((a, b) => b.Where(p => p.Id == a.EquipAccountId && p.Id == accountId));
            query.Exists<ProjectDetail>((a, b) => b.Where(p => p.Id == a.ProjectDetailId && p.CycleType == (CycleType)cycleType));
            return query.ToList(null, new EagerLoadOptions().LoadWithViewProperty().LoadWith(EquipAccountCheckProject.ProjectDetailProperty));
        }

        /// <summary>
        /// 根据设备台账Id列表和周期类型获取设备台账点检项目列表
        /// </summary>
        /// <param name="accountIds">设备台账Id列表</param>
        /// <param name="cycleType">周期类型</param>
        /// <returns>设备台账点检项目列表</returns>
        public virtual EntityList<EquipAccountCheckProject> GetEquipAccountCheckProjects(List<double> accountIds, CycleType cycleType)
        {
            return accountIds.SplitContains(tempIds =>
            {
                return Query<EquipAccountCheckProject>()
                    .Exists<ProjectDetail>((a, b) => b.Where(f => f.Id == a.ProjectDetailId
                        && f.CycleType == cycleType
                        && tempIds.Contains(a.EquipAccountId)))
                    .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }
    }
}