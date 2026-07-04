using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Tech.Processs;
using System;

namespace SIE.MES.WorkOrders
{
    /// <summary>
    /// 工单工序对应包装单位
    /// </summary>
    [ChildEntity, Serializable]
    [Label("工单工序对应包装单位")]
    [DisplayMember(nameof(Id))]
    public class WorkOrderProcessPackingUnit : DataEntity
    {
        #region 工单包装规则
        /// <summary>
        /// 工单包装规则ID
        /// </summary>
        [Label("工单包装规则")]
        public static readonly IRefIdProperty PackageRuleIdProperty =
            P<WorkOrderProcessPackingUnit>.RegisterRefId(e => e.PackageRuleId, ReferenceType.Parent);

        /// <summary>
        /// 工单包装规则ID
        /// </summary>
        public double PackageRuleId
        {
            get { return (double)this.GetRefId(PackageRuleIdProperty); }
            set { this.SetRefId(PackageRuleIdProperty, value); }
        }

        /// <summary>
        /// 工单包装规则
        /// </summary>
        public static readonly RefEntityProperty<WorkOrderPackageRuleDetail> PackageRuleProperty =
            P<WorkOrderProcessPackingUnit>.RegisterRef(e => e.PackageRule, PackageRuleIdProperty);

        /// <summary>
        /// 工单包装规则
        /// </summary>
        public WorkOrderPackageRuleDetail PackageRule
        {
            get { return this.GetRefEntity(PackageRuleProperty); }
            set { this.SetRefEntity(PackageRuleProperty, value); }
        }
        #endregion

        #region 工序
        /// <summary>
        /// 工序
        /// </summary>
        [Label("工序")]
        public static readonly IRefIdProperty ProcessIdProperty =
            P<WorkOrderProcessPackingUnit>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

        /// <summary>
        /// 工序
        /// </summary>
        public double ProcessId
        {
            get { return (double)this.GetRefId(ProcessIdProperty); }
            set { this.SetRefId(ProcessIdProperty, value); }
        }

        /// <summary>
        /// 工序
        /// </summary>
        public static readonly RefEntityProperty<Process> ProcessProperty =
            P<WorkOrderProcessPackingUnit>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 工序
        /// </summary>
        public Process Process
        {
            get { return this.GetRefEntity(ProcessProperty); }
            set { this.SetRefEntity(ProcessProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 工单工序对应包装单位
    /// </summary>
    internal class WorkOrderProcessPackingUnitConfig : EntityConfig<WorkOrderProcessPackingUnit>
    {
        /// <summary>
        /// 配置数据库映射
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WO_PKG_RULE_DTL_UNIT").MapAllProperties();
            Meta.EnablePhantoms();
            Meta.EnableSort();
            Meta.Property(WorkOrderProcessPackingUnit.PackageRuleIdProperty).ColumnMeta.HasIndex();
        }
    }
}
