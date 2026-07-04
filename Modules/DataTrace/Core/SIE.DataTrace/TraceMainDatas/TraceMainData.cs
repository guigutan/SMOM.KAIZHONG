using SIE.Common.Configs;
using SIE.Common.Configs.CommonConfigs;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.WorkFlow.Base.FlowInstances;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.DataTrace.TraceMainDatas
{
    /// <summary>
    /// 追溯主数据
    /// </summary>
    [EntityWithConfig(typeof(NoConfig), "追溯主数据单号配置项", "追溯主数据的单号配置规则")]
    [ConditionQueryType(typeof(TraceMainDataCriteria))]
    [Label("追溯主数据")]
    [DisplayMember(nameof(No))]
    [RootEntity, Serializable]
    public class TraceMainData : DataEntity
    {
        #region 单号 No
        /// <summary>
        /// 单号
        /// </summary>
        [Label("单号")]
        [Required]
        [NotDuplicate]
        public static readonly Property<string> NoProperty = P<TraceMainData>.Register(e => e.No);

        /// <summary>
        /// 单号
        /// </summary>
        public string No
        {
            get { return this.GetProperty(NoProperty); }
            set { this.SetProperty(NoProperty, value); }
        }
        #endregion

        #region 流程进度 FlowInstance
        /// <summary>
        /// 流程进度Id
        /// </summary>
        [Label("流程进度")]
        public static readonly IRefIdProperty FlowInstanceIdProperty =
            P<TraceMainData>.RegisterRefId(e => e.FlowInstanceId, ReferenceType.Normal);

        /// <summary>
        /// 流程进度Id
        /// </summary>
        public double? FlowInstanceId
        {
            get { return (double?)this.GetRefNullableId(FlowInstanceIdProperty); }
            set { this.SetRefNullableId(FlowInstanceIdProperty, value); }
        }

        /// <summary>
        /// 流程进度
        /// </summary>
        public static readonly RefEntityProperty<FlowInstance> FlowInstanceProperty =
            P<TraceMainData>.RegisterRef(e => e.FlowInstance, FlowInstanceIdProperty);

        /// <summary>
        /// 流程进度
        /// </summary>
        public FlowInstance FlowInstance
        {
            get { return this.GetRefEntity(FlowInstanceProperty); }
            set { this.SetRefEntity(FlowInstanceProperty, value); }
        }
        #endregion

        #region 上下文 Context
        /// <summary>
        /// 上下文
        /// </summary>
        [Label("上下文")]
        public static readonly Property<string> ContextProperty = P<TraceMainData>.Register(e => e.Context);

        /// <summary>
        /// 上下文
        /// </summary>
        public string Context
        {
            get { return this.GetProperty(ContextProperty); }
            set { this.SetProperty(ContextProperty, value); }
        }
        #endregion

        #region 附件 AttachmentList
        /// <summary>
        /// 附件
        /// </summary>
        [Label("附件")]
        public static readonly ListProperty<EntityList<TraceMainDataAttachment>> AttachmentListProperty = P<TraceMainData>.RegisterList(e => e.AttachmentList);

        /// <summary>
        /// 附件
        /// </summary>
        public EntityList<TraceMainDataAttachment> AttachmentList
        {
            get { return this.GetLazyList(AttachmentListProperty); }
        }
        #endregion

        #region 追溯流程类型 WorkFlowType
        /// <summary>
        /// 追溯流程类型
        /// </summary>
        [Label("追溯流程类型")]
        public static readonly Property<string> WorkFlowTypeProperty = P<TraceMainData>.Register(e => e.WorkFlowType);

        /// <summary>
        /// 追溯流程类型
        /// </summary>
        public string WorkFlowType
        {
            get { return this.GetProperty(WorkFlowTypeProperty); }
            set { this.SetProperty(WorkFlowTypeProperty, value); }
        }
        #endregion

        #region 关联ID CorrelationId
        /// <summary>
        /// 关联ID
        /// </summary>
        [Label("关联ID")]
        public static readonly Property<string> CorrelationIdProperty = P<TraceMainData>.Register(e => e.CorrelationId);

        /// <summary>
        /// 关联ID
        /// </summary>
        public string CorrelationId
        {
            get { return this.GetProperty(CorrelationIdProperty); }
            set { this.SetProperty(CorrelationIdProperty, value); }
        }
        #endregion

        #region 流程进度状态 WorkFlowStatus
        /// <summary>
        /// 流程进度状态
        /// </summary>
        [Label("流程进度状态")]
        public static readonly Property<WorkFlowStatus> WorkFlowStatusProperty = P<TraceMainData>.RegisterView(e => e.WorkFlowStatus, p => p.FlowInstance.WorkFlowStatus);

        /// <summary>
        /// 流程进度状态
        /// </summary>
        public WorkFlowStatus WorkFlowStatus
        {
            get { return this.GetProperty(WorkFlowStatusProperty); }
        }
        #endregion

    }

    /// <summary>
    /// 追溯主数据 实体配置
    /// </summary>
    public class TraceMainDataConfig : EntityConfig<TraceMainData>
    {
        /// <summary>
        /// 实体配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("TRACE_MAIN").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
