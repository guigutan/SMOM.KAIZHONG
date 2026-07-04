using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.Report.FaultCountReports
{
    /// <summary>
    /// 统计报表控制器
    /// </summary>
   public class FaultCountReportController : DomainController
    {

        /// <summary>
        /// 获取交易明细报表
        /// </summary>
        /// <param name="criteria">查询实体</param>
        /// <returns>交易明细</returns>
        public virtual EntityList<FaultCountReport> GetFaultCountReports(FaultCountReportCriteria criteria)
        {
            var query = Query<FaultCountReport>();

            if (criteria.FactoryId.HasValue)
            {
                query.Where(p => p.EquipAccount.FactoryId == criteria.FactoryId);
            }
            if (criteria.DepartmentId.HasValue)
            {
                query.Where(p => p.EquipAccount.UseDepartmentId == criteria.DepartmentId);
            }
            if (criteria.Code.IsNotEmpty())
            {
                query.Where(p => p.EquipAccount.Code.Contains(criteria.Code));
            }
            if (criteria.RepairMasterId.HasValue)
            {
                query.Where(p => p.RepairMasterId == criteria.RepairMasterId);
            }
            if (criteria.ApplyRepairDate.BeginValue.HasValue)
            {
                query.Where(p => p.ApplyRepairDate >= criteria.ApplyRepairDate.BeginValue);
            }
            if (criteria.ApplyRepairDate.EndValue.HasValue)
            {
                query.Where(p => p.ApplyRepairDate < criteria.ApplyRepairDate.EndValue.Value);
            }

            EagerLoadOptions elo = new EagerLoadOptions();

            if (criteria.OrderInfoList.Count > 0)
            {
                query.OrderBy(criteria.OrderInfoList);
            }
            else
            {
                query.OrderByDescending(p => p.Id);
            }
            return query.Where(p => p.InvOrgId == AppRuntime.InvOrg).ToList(criteria.PagingInfo, elo.LoadWithViewProperty());
        }
    }
}
