using SIE.Domain;
using SIE.MES.WorkOrders;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.WorkReportPlans
{
    /// <summary>
    /// 报工方案-工序信息
    /// </summary>
    [ChildEntity, Serializable]
    [Label("报工方案-工序信息")]
    public partial class ProcessInfo : DataEntity
    {
        #region 所属方案 WorkReportPlan
        /// <summary>
        /// 所属方案ID
        /// </summary>
        [Label("所属方案")]
        public static readonly IRefIdProperty WorkReportPlanIdProperty =
                    P<ProcessInfo>.RegisterRefId(e => e.WorkReportPlanId, ReferenceType.Parent);

        /// <summary>
        /// 所属方案ID
        /// </summary>
        public double WorkReportPlanId
        {
            get { return (double)this.GetRefId(WorkReportPlanIdProperty); }
            set { this.SetRefId(WorkReportPlanIdProperty, value); }
        }

        /// <summary>
        /// 所属方案
        /// </summary>
        public static readonly RefEntityProperty<WorkReportPlan> WorkReportPlanProperty =
            P<ProcessInfo>.RegisterRef(e => e.WorkReportPlan, WorkReportPlanIdProperty);

        /// <summary>
        /// 所属方案
        /// </summary>
        public WorkReportPlan WorkReportPlan
        {
            get { return this.GetRefEntity(WorkReportPlanProperty); }
            set { this.SetRefEntity(WorkReportPlanProperty, value); }
        }
        #endregion

        #region 工序 Process
        /// <summary>
        /// 工序Id
        /// </summary>
        [Label("工序")]
        [NotDuplicate]
        public static readonly IRefIdProperty ProcessIdProperty = P<ProcessInfo>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Process> ProcessProperty = P<ProcessInfo>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 工序
        /// </summary>
        public Process Process
        {
            get { return GetRefEntity(ProcessProperty); }
            set { SetRefEntity(ProcessProperty, value); }
        }
        #endregion

        #region 产品族小类 ProductFamilyName
        /// <summary>
        /// 产品族小类
        /// </summary>
        [Label("产品族小类")]
        public static readonly Property<string> ProductFamilyNameProperty = P<ProcessInfo>.RegisterView(e => e.ProductFamilyName, e => e.Process.ProductFamily.Name);

        /// <summary>
        /// 产品族小类
        /// </summary>
        public string ProductFamilyName
        {
            get { return GetProperty(ProductFamilyNameProperty); }
        }
        #endregion     
    }
    internal class ProcessInfoConfig : EntityConfig<ProcessInfo>
    {
        /// <summary>
        /// 对Meta属性的配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WO_REPORT_PLAN_PROCESS").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
