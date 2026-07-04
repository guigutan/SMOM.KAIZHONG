using SIE.Domain;
using SIE.ObjectModel;
using SIE.WorkFlow.Base.FlowInstances;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.DataTrace.TraceMainDatas
{
    /// <summary>
    /// 追溯主数据查询实体
    /// </summary>
    [Label("追溯主数据查询实体")]
    [QueryEntity,Serializable]
    public class TraceMainDataCriteria:Criteria
    {
        #region 单号 No
        /// <summary>
        /// 单号
        /// </summary>
        [Label("单号")]
        public static readonly Property<string> NoProperty = P<TraceMainDataCriteria>.Register(e => e.No);

        /// <summary>
        /// 单号
        /// </summary>
        public string No
        {
            get { return this.GetProperty(NoProperty); }
            set { this.SetProperty(NoProperty, value); }
        }
        #endregion

        #region 追溯流程类型 WorkFlowType
        /// <summary>
        /// 追溯流程类型
        /// </summary>
        [Label("追溯流程类型")]
        public static readonly Property<string> WorkFlowTypeProperty = P<TraceMainDataCriteria>.Register(e => e.WorkFlowType);

        /// <summary>
        /// 追溯流程类型
        /// </summary>
        public string WorkFlowType
        {
            get { return this.GetProperty(WorkFlowTypeProperty); }
            set { this.SetProperty(WorkFlowTypeProperty, value); }
        }
        #endregion

        #region 流程进度 FlowInstance
        /// <summary>
        /// 流程进度Id
        /// </summary>
        [Label("流程进度")]
        public static readonly IRefIdProperty FlowInstanceIdProperty =
            P<TraceMainDataCriteria>.RegisterRefId(e => e.FlowInstanceId, ReferenceType.Normal);

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
            P<TraceMainDataCriteria>.RegisterRef(e => e.FlowInstance, FlowInstanceIdProperty);

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
        public static readonly Property<string> ContextProperty = P<TraceMainDataCriteria>.Register(e => e.Context);

        /// <summary>
        /// 上下文
        /// </summary>
        public string Context
        {
            get { return this.GetProperty(ContextProperty); }
            set { this.SetProperty(ContextProperty, value); }
        }
        #endregion

        protected override EntityList Fetch()
        {
            return  RT.Service.Resolve<TraceMainDataController>().GetTraceMainDatas(this);
        }
    }
}
