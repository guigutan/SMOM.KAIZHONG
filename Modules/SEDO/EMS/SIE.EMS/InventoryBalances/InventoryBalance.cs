using SIE.Common.Configs;
using SIE.DataAuth;
using SIE.Domain;
using SIE.EMS.InventoryBalances.Configs;
using SIE.EMS.InventoryTasks;
using SIE.Equipments.Configs;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using System;

namespace SIE.EMS.InventoryBalances
{
    /// <summary>
    /// 盘点平账
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(InventoryBalanceCriteria))]
    [Label("盘点平账")]
    [DisplayMember(nameof(TaskNo))]
    [EntityWithConfig(typeof(ApprovalConfig))]
    [EntityWithConfig(typeof(BalanceScrapTypeConfig))]
    [EntityDataAuth(typeof(EmployeeEnterprise), nameof(FactoryId), true)]
    public partial class InventoryBalance : InventoryTask
    {
        #region 附件列表 AttachmentList
        /// <summary>
        /// 附件列表
        /// </summary>
        [Label("附件列表")]
        public static readonly ListProperty<EntityList<InventoryBalanceAttachment>> AttachmentListProperty = P<InventoryBalance>.RegisterList(e => e.AttachmentList);

        /// <summary>
        /// 附件列表
        /// </summary>
        public EntityList<InventoryBalanceAttachment> AttachmentList
        {
            get { return this.GetLazyList(AttachmentListProperty); }
        }
        #endregion


       
    }
}
