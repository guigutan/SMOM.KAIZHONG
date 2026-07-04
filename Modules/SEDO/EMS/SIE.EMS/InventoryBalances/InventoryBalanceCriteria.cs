using SIE.Domain;
using SIE.EMS.InventoryTasks;
using SIE.Equipments.Enums;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.InventoryBalances
{
    /// <summary>
    /// 盘点平账查询
    /// </summary>
    [QueryEntity, Serializable]
    [Label("盘点平账查询")]
    public partial class InventoryBalanceCriteria : InventoryTaskCriteria
    {
        #region 审核状态 ApprovalStatus
        /// <summary>
        /// 审核状态
        /// </summary>
        [Label("审核状态")]
        public static readonly Property<ApprovalStatus?> ApprovalStatusProperty = P<InventoryBalanceCriteria>.Register(e => e.ApprovalStatus);

        /// <summary>
        /// 审核状态
        /// </summary>
        public ApprovalStatus? ApprovalStatus
        {
            get { return GetProperty(ApprovalStatusProperty); }
            set { SetProperty(ApprovalStatusProperty, value); }
        }
        #endregion

        /// <summary>
        /// 查询逻辑
        /// </summary>
        /// <returns>返回查询后的数据</returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<InventoryBalanceController>().CriteriaInventoryBalances(this);
        }
    }
}
