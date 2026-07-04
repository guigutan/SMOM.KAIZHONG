using SIE.Common.Signature;
using SIE.DataTrace.TraceMainDatas;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources;
using SIE.WorkFlow.Base.FlowInstances;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.DataTrace.DataTraceAudits
{
    /// <summary>
    /// 数据追溯签名
    /// </summary>
    [RootEntity, Serializable]
    [Label("数据追溯签名")]
    public class DataTraceSignature : DataEntity
    {
        #region 签名人 SignatureBy
        /// <summary>
        /// 签名人Id
        /// </summary>
        [Label("签名人")]
        public static readonly IRefIdProperty SignatureByIdProperty =
            P<DataTraceSignature>.RegisterRefId(e => e.SignatureById, ReferenceType.Normal);

        /// <summary>
        /// 签名人Id
        /// </summary>
        public double? SignatureById
        {
            get { return (double?)this.GetRefNullableId(SignatureByIdProperty); }
            set { this.SetRefNullableId(SignatureByIdProperty, value); }
        }

        /// <summary>
        /// 签名人
        /// </summary>
        public static readonly RefEntityProperty<Employee> SignatureByProperty =
            P<DataTraceSignature>.RegisterRef(e => e.SignatureBy, SignatureByIdProperty);

        /// <summary>
        /// 签名人
        /// </summary>
        public Employee SignatureBy
        {
            get { return this.GetRefEntity(SignatureByProperty); }
            set { this.SetRefEntity(SignatureByProperty, value); }
        }
        #endregion

        #region 签名记录 SignatureRecord
        /// <summary>
        /// 签名记录Id
        /// </summary>
        [Label("签名记录")]
        public static readonly IRefIdProperty SignatureRecordIdProperty =
            P<DataTraceSignature>.RegisterRefId(e => e.SignatureRecordId, ReferenceType.Normal);

        /// <summary>
        /// 签名记录Id
        /// </summary>
        public double? SignatureRecordId
        {
            get { return (double?)this.GetRefNullableId(SignatureRecordIdProperty); }
            set { this.SetRefNullableId(SignatureRecordIdProperty, value); }
        }

        /// <summary>
        /// 签名记录
        /// </summary>
        public static readonly RefEntityProperty<SignatureRecord> SignatureRecordProperty =
            P<DataTraceSignature>.RegisterRef(e => e.SignatureRecord, SignatureRecordIdProperty);

        /// <summary>
        /// 签名记录
        /// </summary>
        public SignatureRecord SignatureRecord
        {
            get { return this.GetRefEntity(SignatureRecordProperty); }
            set { this.SetRefEntity(SignatureRecordProperty, value); }
        }
        #endregion

        #region 追溯主数据 TraceMainData
        /// <summary>
        /// 追溯主数据Id
        /// </summary>
        [Label("追溯主数据")]
        public static readonly IRefIdProperty TraceMainDataIdProperty =
            P<DataTraceSignature>.RegisterRefId(e => e.TraceMainDataId, ReferenceType.Normal);

        /// <summary>
        /// 追溯主数据Id
        /// </summary>
        public double TraceMainDataId
        {
            get { return (double)this.GetRefId(TraceMainDataIdProperty); }
            set { this.SetRefId(TraceMainDataIdProperty, value); }
        }

        /// <summary>
        /// 追溯主数据
        /// </summary>
        public static readonly RefEntityProperty<TraceMainData> TraceMainDataProperty =
            P<DataTraceSignature>.RegisterRef(e => e.TraceMainData, TraceMainDataIdProperty);

        /// <summary>
        /// 追溯主数据
        /// </summary>
        public TraceMainData TraceMainData
        {
            get { return this.GetRefEntity(TraceMainDataProperty); }
            set { this.SetRefEntity(TraceMainDataProperty, value); }
        }
        #endregion

        #region 流程进度 FlowInstance
        /// <summary>
        /// 流程进度Id
        /// </summary>
        [Label("流程进度")]
        public static readonly IRefIdProperty FlowInstanceIdProperty =
            P<DataTraceSignature>.RegisterRefId(e => e.FlowInstanceId, ReferenceType.Normal);

        /// <summary>
        /// 流程进度Id
        /// </summary>
        public double FlowInstanceId
        {
            get { return (double)this.GetRefId(FlowInstanceIdProperty); }
            set { this.SetRefId(FlowInstanceIdProperty, value); }
        }

        /// <summary>
        /// 流程进度
        /// </summary>
        public static readonly RefEntityProperty<FlowInstance> FlowInstanceProperty =
            P<DataTraceSignature>.RegisterRef(e => e.FlowInstance, FlowInstanceIdProperty);

        /// <summary>
        /// 流程进度
        /// </summary>
        public FlowInstance FlowInstance
        {
            get { return this.GetRefEntity(FlowInstanceProperty); }
            set { this.SetRefEntity(FlowInstanceProperty, value); }
        }
        #endregion

        #region 节点ID ActivityId
        /// <summary>
        /// 节点ID
        /// </summary>
        [Label("节点ID")]
        public static readonly Property<string> ActivityIdProperty = P<DataTraceSignature>.Register(e => e.ActivityId);

        /// <summary>
        /// 节点ID
        /// </summary>
        public string ActivityId
        {
            get { return this.GetProperty(ActivityIdProperty); }
            set { this.SetProperty(ActivityIdProperty, value); }
        }
        #endregion

        #region 追溯节点ID DataTraceActivityId
        /// <summary>
        /// 追溯节点ID
        /// </summary>
        [Label("追溯节点ID")]
        public static readonly Property<string> DataTraceActivityIdProperty = P<DataTraceSignature>.Register(e => e.DataTraceActivityId);

        /// <summary>
        /// 追溯节点ID
        /// </summary>
        public string DataTraceActivityId
        {
            get { return this.GetProperty(DataTraceActivityIdProperty); }
            set { this.SetProperty(DataTraceActivityIdProperty, value); }
        }
        #endregion

        #region 追溯节点实体类型 TraceEntityType
        /// <summary>
        /// 追溯节点实体类型
        /// </summary>
        [Label("追溯节点实体类型")]
        public static readonly Property<string> TraceEntityTypeProperty = P<DataTraceSignature>.Register(e => e.TraceEntityType);

        /// <summary>
        /// 追溯节点实体类型
        /// </summary>
        public string TraceEntityType
        {
            get { return this.GetProperty(TraceEntityTypeProperty); }
            set { this.SetProperty(TraceEntityTypeProperty, value); }
        }
        #endregion


    }

    /// <summary>
    /// 数据追溯签名 实体配置
    /// </summary>
    public class TraceMainDataConfig : EntityConfig<DataTraceSignature>
    {
        /// <summary>
        /// 实体配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("TRACE_AUDIT_SIGN").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
