using SIE.Domain;
using SIE.Domain.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.WorkReportPlans
{
    /// <summary>
    /// 报工配置方案控制器
    /// </summary>
    public partial class WorkReportPlanController : DomainController
    {
        /// <summary>
        /// 获取报工配置方案
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual EntityList<WorkReportPlan> GetWorkReportPlans(WorkReportPlanCriteria criteria)
        {
            var q = Query<WorkReportPlan>();
            if (criteria.PlanCode.IsNotEmpty())
                q.Where(p => p.PlanCode.Contains(criteria.PlanCode));
            if (criteria.PlanName.IsNotEmpty())
                q.Where(p => p.PlanName.Contains(criteria.PlanName));
            if (criteria.EnableStatus.HasValue)
                q.Where(p => p.EnableStatus == criteria.EnableStatus);
            if (criteria.ProcessId.HasValue)
            {
                q.Join<ProcessInfo>((x, y) => x.Id == y.WorkReportPlanId && y.ProcessId == criteria.ProcessId);
            }

            return q.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 设置缺省默认
        /// </summary>
        /// <param name="vId"></param>
        /// <exception cref="NotImplementedException"></exception>
        public virtual void SetDefaultCommand(double vId)
        {
            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {

                DB.Update<WorkReportPlan>().Set(p => p.IsDefault, false).Where(m => m.IsDefault == true).Execute();
                var result = DB.Update<WorkReportPlan>().Set(p => p.IsDefault, true).Where(m => m.Id == vId).Execute();
                if (result <= 0)
                {
                    throw new ValidationException("设置失败，当前所选数据系统不存在，请刷新".L10N());
                }
                tran.Complete();
            }
        }

        /// <summary>
        /// 初始化默认值
        /// </summary>
        /// <exception cref="ValidationException"></exception>
        public virtual void InitializationDefault()
        {

            var defaultWorkReportPlan = Query<WorkReportPlan>().Where(m => m.IsDefault == true).FirstOrDefault();
            if (defaultWorkReportPlan != null)
            {
                throw new ValidationException("设置失败，已存在缺省方案不允许触发，请检查".L10N());
            }
            WorkReportPlan workReportPlan = new WorkReportPlan()
            {
                IsDefault = true,
                PlanCode = "BGFA001",
                EnableStatus = true,
                PlanName = "默认报工方案".L10N(),
                PlanTemplateName = TemplateNames.General,
                IsDispatchTask = true,
                IsProductionReport = true,
                IsShowFirstInsp=true,
                PersistenceStatus = PersistenceStatus.New
            };
            RF.Save(workReportPlan);
        }


    }
}
