using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.EarlierStage.Budgets
{
    /// <summary>
    /// 预算变更查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("预算变更查询实体")]
    public class BudgetChangeCriteria : BudgetCriteria
    {
        /// <summary>
        /// 查询逻辑
        /// </summary>
        /// <returns>返回查询后的数据</returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<BudgetChangeController>().CriteriaBudgetChanges(this);
        }
    }
}
