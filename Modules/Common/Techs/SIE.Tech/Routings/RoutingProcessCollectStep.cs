using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Tech.Routings
{
    /// <summary>
    /// 工序清单采集步骤
    /// </summary>
    [ChildEntity, Serializable]
    [Label("工序清单采集步骤")]
    public partial class RoutingProcessCollectStep : DataEntity
    {
        #region 步骤 Step
        /// <summary>
        /// 步骤
        /// </summary>
        public static readonly Property<int> StepProperty = P<RoutingProcessCollectStep>.Register(e => e.Step);

        /// <summary>
        /// 步骤
        /// </summary>
        public int Step
        {
            get { return GetProperty(StepProperty); }
            set { SetProperty(StepProperty, value); }
        }
        #endregion

        #region 是否解绑 IsUnbound
        /// <summary>
        /// 是否解绑
        /// </summary>
        public static readonly Property<bool> IsUnboundProperty = P<RoutingProcessCollectStep>.Register(e => e.IsUnbound);

        /// <summary>
        /// 是否解绑
        /// </summary>
        public bool IsUnbound
        {
            get { return GetProperty(IsUnboundProperty); }
            set { SetProperty(IsUnboundProperty, value); }
        }
        #endregion

        #region 工序清单 RoutingProcess
        /// <summary>
        /// 工序清单Id
        /// </summary>
        public static readonly IRefIdProperty RoutingProcessIdProperty = P<RoutingProcessCollectStep>.RegisterRefId(e => e.RoutingProcessId, ReferenceType.Parent);

        /// <summary>
        /// 工序清单Id
        /// </summary>
        public double RoutingProcessId
        {
            get { return (double)GetRefId(RoutingProcessIdProperty); }
            set { SetRefId(RoutingProcessIdProperty, value); }
        }

        /// <summary>
        /// 工序清单
        /// </summary>
        public static readonly RefEntityProperty<RoutingProcess> RoutingProcessProperty = P<RoutingProcessCollectStep>.RegisterRef(e => e.RoutingProcess, RoutingProcessIdProperty);

        /// <summary>
        /// 工序清单
        /// </summary>
        public RoutingProcess RoutingProcess
        {
            get { return GetRefEntity(RoutingProcessProperty); }
            set { SetRefEntity(RoutingProcessProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 工序清单采集步骤 实体配置
    /// </summary>
    internal class RoutingProcessCollectStepConfig : EntityConfig<RoutingProcessCollectStep>
    {
        /// <summary>
        /// 元数据配置
        /// </summary>
		protected override void ConfigMeta()
        {
            Meta.MapTable("TECH_RT_PROC_STEP").MapAllProperties();
            Meta.Property(RoutingProcessCollectStep.RoutingProcessIdProperty).ColumnMeta.HasIndex();
            Meta.EnablePhantoms();
        }
    }
}