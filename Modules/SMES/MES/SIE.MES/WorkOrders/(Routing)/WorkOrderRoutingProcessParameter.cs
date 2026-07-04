using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Tech.Processs;
using System;

namespace SIE.MES.WorkOrders
{
    /// <summary>
    /// 工单工序清单参数
    /// </summary>
    [ChildEntity, Serializable]
    [Label("工单工序清单参数")]
    public partial class WorkOrderRoutingProcessParameter : DataEntity
    {
        #region 规则Id RuleId
        /// <summary>
        /// 规则Id
        /// </summary>
        [Required]
        [Label("规则Id")]
        [MaxLength(80)]
        public static readonly Property<string> RuleIdProperty = P<WorkOrderRoutingProcessParameter>.Register(e => e.RuleId);

        /// <summary>
        /// 规则Id
        /// </summary>
        public string RuleId
        {
            get { return this.GetProperty(RuleIdProperty); }
            set { this.SetProperty(RuleIdProperty, value); }
        }
        #endregion

        #region 描述 Description
        /// <summary>
        /// 描述
        /// </summary>
        [Required]
        [Label("描述")]
        public static readonly Property<string> DescriptionProperty = P<WorkOrderRoutingProcessParameter>.Register(e => e.Description);

        /// <summary>
        /// 描述
        /// </summary>
        public string Description
        {
            get { return GetProperty(DescriptionProperty); }
            set { SetProperty(DescriptionProperty, value); }
        }
        #endregion

        #region 结果 ResultType
        /// <summary>
        /// 结果
        /// </summary>
        [Label("结果")]
        public static readonly Property<ResultTypeForDesign> ResultTypeProperty = P<WorkOrderRoutingProcessParameter>.Register(e => e.ResultType);

        /// <summary>
        /// 结果
        /// </summary>
        public ResultTypeForDesign ResultType
        {
            get { return GetProperty(ResultTypeProperty); }
            set { SetProperty(ResultTypeProperty, value); }
        }
        #endregion

        #region 脚本 Expression
        /// <summary>
        /// 脚本
        /// </summary>
        [Label("脚本")]
        [MaxLength(4000)]
        public static readonly Property<string> ExpressionProperty = P<WorkOrderRoutingProcessParameter>.Register(e => e.Expression);

        /// <summary>
        /// 脚本
        /// </summary>
        public string Expression
        {
            get { return this.GetProperty(ExpressionProperty); }
            set { this.SetProperty(ExpressionProperty, value); }
        }
        #endregion

        #region 下一工序 NextProcess
        /// <summary>
        /// 下一工序Id
        /// </summary>
        [Label("下一工序")]
        public static readonly IRefIdProperty NextProcessIdProperty = P<WorkOrderRoutingProcessParameter>.RegisterRefId(e => e.NextProcessId, ReferenceType.Normal);

        /// <summary>
        /// 下一工序Id
        /// </summary>
        public double? NextProcessId
        {
            get { return (double?)GetRefNullableId(NextProcessIdProperty); }
            set { SetRefNullableId(NextProcessIdProperty, value); }
        }

        /// <summary>
        /// 下一工序
        /// </summary>
        [Label("下一工序")]
        public static readonly RefEntityProperty<WorkOrderRoutingProcess> NextProcessProperty = P<WorkOrderRoutingProcessParameter>.RegisterRef(e => e.NextProcess, NextProcessIdProperty);

        /// <summary>
        /// 下一工序
        /// </summary>
        public WorkOrderRoutingProcess NextProcess
        {
            get { return GetRefEntity(NextProcessProperty); }
            set { SetRefEntity(NextProcessProperty, value); }
        }
        #endregion

        #region 工单工序清单与参数关系 Process
        /// <summary>
        /// 工单工序清单与参数关系Id
        /// </summary>
        [Label("工单工序清单")]
        public static readonly IRefIdProperty ProcessIdProperty = P<WorkOrderRoutingProcessParameter>.RegisterRefId(e => e.ProcessId, ReferenceType.Parent);

        /// <summary>
        /// 工单工序清单与参数关系Id
        /// </summary>
        public double ProcessId
        {
            get { return (double)GetRefId(ProcessIdProperty); }
            set { SetRefId(ProcessIdProperty, value); }
        }

        /// <summary>
        /// 工单工序清单与参数关系
        /// </summary>
        [Label("工单工序清单")]
        public static readonly RefEntityProperty<WorkOrderRoutingProcess> ProcessProperty = P<WorkOrderRoutingProcessParameter>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 工单工序清单与参数关系
        /// </summary>
        public WorkOrderRoutingProcess Process
        {
            get { return GetRefEntity(ProcessProperty); }
            set { SetRefEntity(ProcessProperty, value); }
        }
        #endregion

        #region 跳转条件 Condition
        /// <summary>
        /// 跳转条件
        /// </summary>
        [Label("跳转条件")]
        public static readonly Property<string> ConditionProperty = P<WorkOrderRoutingProcessParameter>.Register(e => e.Condition);

        /// <summary>
        /// 跳转条件
        /// </summary>
        public string Condition
        {
            get { return this.GetProperty(ConditionProperty); }
            set { this.SetProperty(ConditionProperty, value); }
        }
        #endregion 

        #region 视图属性
        #region 下一工序名称 ProcessName
        /// <summary>
        /// 下一工序名称
        /// </summary>
        [Label("下一工序")]
        public static readonly Property<string> ProcessNameProperty = P<WorkOrderRoutingProcessParameter>.RegisterView(e => e.ProcessName, p => p.NextProcess.Name);

        /// <summary>
        /// 下一工序名称
        /// </summary>
        public string ProcessName
        {
            get { return this.GetProperty(ProcessNameProperty); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 工单工序清单参数 实体配置
    /// </summary>
    internal class WorkOrderRoutingProcessParameterConfig : EntityConfig<WorkOrderRoutingProcessParameter>
    {
        /// <summary>
        /// 属性元数据配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WO_RT_PROC_PARAM").MapAllProperties();
            Meta.EnablePhantoms();
            Meta.Property(WorkOrderRoutingProcessParameter.NextProcessIdProperty).ColumnMeta.IgnoreFK().IsNullable();
            Meta.Property(WorkOrderRoutingProcessParameter.ExpressionProperty).ColumnMeta.HasLength(4000);
            Meta.Property(WorkOrderRoutingProcessParameter.ConditionProperty).ColumnMeta.HasLength(4000);
            Meta.Property(WorkOrderRoutingProcessParameter.RuleIdProperty).ColumnMeta.HasLength(320);
            Meta.Property(WorkOrderRoutingProcessParameter.ProcessIdProperty).ColumnMeta.HasIndex();
        }
    }
}