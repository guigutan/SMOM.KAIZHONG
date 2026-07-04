using SIE.Domain;
using SIE.Equipments.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using SIE.Resources.Employees;

namespace SIE.Equipments.WorkFlows
{
    /// <summary>
    /// 审核记录
    /// </summary>
    [RootEntity, Serializable]
    [Label("审核记录")]
    public partial class WorkFlowRecord : DataEntity
    {
        #region 来源类型 SourceType
        /// <summary>
        /// 来源类型
        /// </summary>
        [Label("来源类型")]
        public static readonly Property<string> SourceTypeProperty = P<WorkFlowRecord>.Register(e => e.SourceType);

        /// <summary>
        /// 来源类型
        /// </summary>
        public string SourceType
        {
            get { return GetProperty(SourceTypeProperty); }
            set { SetProperty(SourceTypeProperty, value); }
        }
        #endregion

        #region 来源ID SourceId
        /// <summary>
        /// 来源ID
        /// </summary>
        [Label("来源ID")]
        public static readonly Property<double> SourceIdProperty = P<WorkFlowRecord>.Register(e => e.SourceId);

        /// <summary>
        /// 来源ID
        /// </summary>
        public double SourceId
        {
            get { return GetProperty(SourceIdProperty); }
            set { SetProperty(SourceIdProperty, value); }
        }
        #endregion

        #region 审核时间 ApprovalDatetime
        /// <summary>
        /// 审核时间
        /// </summary>
        [Label("审核时间")]
        public static readonly Property<DateTime> ApprovalDatetimeProperty = P<WorkFlowRecord>.Register(e => e.ApprovalDatetime);

        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime ApprovalDatetime
        {
            get { return GetProperty(ApprovalDatetimeProperty); }
            set { SetProperty(ApprovalDatetimeProperty, value); }
        }
        #endregion

        #region 审核意见 Remark
        /// <summary>
        /// 审核意见
        /// </summary>
        [MaxLength(1000)]
        [Label("审核意见")]
        public static readonly Property<string> RemarkProperty = P<WorkFlowRecord>.Register(e => e.Remark);

        /// <summary>
        /// 审核意见
        /// </summary>
        public string Remark
        {
            get { return GetProperty(RemarkProperty); }
            set { SetProperty(RemarkProperty, value); }
        }
        #endregion

        #region 审核结果 ApprovalResult
        /// <summary>
        /// 审核结果
        /// </summary>
        [Label("审核结果")]
        public static readonly Property<ApprovalResult> ApprovalResultProperty = P<WorkFlowRecord>.Register(e => e.ApprovalResult);

        /// <summary>
        /// 审核结果
        /// </summary>
        public ApprovalResult ApprovalResult
        {
            get { return GetProperty(ApprovalResultProperty); }
            set { SetProperty(ApprovalResultProperty, value); }
        }
        #endregion

        #region 审核人 Approver
        /// <summary>
        /// 审核人Id
        /// </summary>
        [Label("审核人")]
        public static readonly IRefIdProperty ApproverIdProperty = P<WorkFlowRecord>.RegisterRefId(e => e.ApproverId, ReferenceType.Normal);

        /// <summary>
        /// 审核人Id
        /// </summary>
        public double ApproverId
        {
            get { return (double)GetRefId(ApproverIdProperty); }
            set { SetRefId(ApproverIdProperty, value); }
        }

        /// <summary>
        /// 审核人
        /// </summary>
        public static readonly RefEntityProperty<Employee> ApproverProperty = P<WorkFlowRecord>.RegisterRef(e => e.Approver, ApproverIdProperty);

        /// <summary>
        /// 审核人
        /// </summary>
        public Employee Approver
        {
            get { return GetRefEntity(ApproverProperty); }
            set { SetRefEntity(ApproverProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 审核记录 实体配置
    /// </summary>
    internal class WorkFlowRecordConfig : EntityConfig<WorkFlowRecord>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EME_WF_RECORD").MapAllProperties();
            Meta.Property(WorkFlowRecord.RemarkProperty).ColumnMeta.HasLength(2000);
            Meta.EnablePhantoms();
        }
    }
}
