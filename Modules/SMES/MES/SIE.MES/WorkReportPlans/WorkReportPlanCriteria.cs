using SIE.Common;
using SIE.Domain;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using SIE.Tech.Processs;
using SIE.Tech.Routings;
using System;

namespace SIE.MES.WorkReportPlans
{
    /// <summary>
    /// 报工方案配置查询
    /// </summary>
    [QueryEntity, Serializable]
    [Label("报工方案配置查询")]
    public partial class WorkReportPlanCriteria : Criteria
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public WorkReportPlanCriteria()
        {
            this.EnableStatus = false;
        }

        #region 方案编码 PlanCode
        /// <summary>
        /// 方案编码
        /// </summary>
        [Label("方案编码")]
        public static readonly Property<string> PlanCodeProperty = P<WorkReportPlanCriteria>.Register(e => e.PlanCode);

        /// <summary>
        /// 方案编码
        /// </summary>
        public string PlanCode
        {
            get { return this.GetProperty(PlanCodeProperty); }
            set { this.SetProperty(PlanCodeProperty, value); }
        }
        #endregion

        #region 方案名称 PlanName
        /// <summary>
        /// 方案名称
        /// </summary>
        [Label("方案名称")]
        public static readonly Property<string> PlanNameProperty = P<WorkReportPlanCriteria>.Register(e => e.PlanName);

        /// <summary>
        /// 方案名称
        /// </summary>
        public string PlanName
        {
            get { return this.GetProperty(PlanNameProperty); }
            set { this.SetProperty(PlanNameProperty, value); }
        }
        #endregion

        #region 启用 EnableStatus
        /// <summary>
        /// 启用 
        /// </summary>
        [Label("启用")]
        [Required]
        public static readonly Property<bool?> EnableStatusProperty = P<WorkReportPlanCriteria>.Register(e => e.EnableStatus);

        /// <summary>
        /// 启用
        /// </summary>
        public bool? EnableStatus
        {
            get { return this.GetProperty(EnableStatusProperty); }
            set { this.SetProperty(EnableStatusProperty, value); }
        }
        #endregion

        #region 工序 Process
        /// <summary>
        /// 工序Id
        /// </summary>
        [Label("工序")]
        public static readonly IRefIdProperty ProcessIdProperty = P<WorkReportPlanCriteria>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

        /// <summary>
        /// 工序Id
        /// </summary>
        public double? ProcessId
        {
            get { return (double?)GetRefNullableId(ProcessIdProperty); }
            set { SetRefNullableId(ProcessIdProperty, value); }
        }

        /// <summary>
        /// 工序
        /// </summary>
        public static readonly RefEntityProperty<Process> ProcessProperty = P<WorkReportPlanCriteria>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 工序
        /// </summary>
        public Process Process
        {
            get { return GetRefEntity(ProcessProperty); }
            set { SetRefEntity(ProcessProperty, value); }
        }
        #endregion

        /// <summary>
        /// 重写此方法实现查询
        /// </summary>
        /// <returns>工单列表</returns>
        protected override EntityList Fetch()
        {

           return RT.Service.Resolve<WorkReportPlanController>().GetWorkReportPlans(this);
        }
    }
}
