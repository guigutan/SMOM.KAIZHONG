using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Tech.Routings
{
    /// <summary>
    /// 工艺路线工序工治具需求
    /// </summary>
    [RootEntity, Serializable]
    [Label("工艺路线工序工治具需求")]
    public class RoutingProcessFixture : DataEntity
    {
        #region 工治具编码ID FixtureId
        /// <summary>
        /// 工治具编码ID
        /// </summary>
        [Label("工治具编码ID")]
        public static readonly Property<double> FixtureIdProperty = P<RoutingProcessFixture>.Register(e => e.FixtureId);

        /// <summary>
        /// 工治具编码ID
        /// </summary>
        public double FixtureId
        {
            get { return this.GetProperty(FixtureIdProperty); }
            set { this.SetProperty(FixtureIdProperty, value); }
        }
        #endregion

        #region 工序清单 RoutingProcess
        /// <summary>
        /// 工序清单Id
        /// </summary>
        [Label("工序清单")]
        public static readonly IRefIdProperty RoutingProcessIdProperty =
            P<RoutingProcessFixture>.RegisterRefId(e => e.RoutingProcessId, ReferenceType.Normal);

        /// <summary>
        /// 工序清单Id
        /// </summary>
        public double RoutingProcessId
        {
            get { return (double)this.GetRefId(RoutingProcessIdProperty); }
            set { this.SetRefId(RoutingProcessIdProperty, value); }
        }

        /// <summary>
        /// 工序清单
        /// </summary>
        public static readonly RefEntityProperty<RoutingProcess> RoutingProcessProperty =
            P<RoutingProcessFixture>.RegisterRef(e => e.RoutingProcess, RoutingProcessIdProperty);

        /// <summary>
        /// 工序清单
        /// </summary>
        public RoutingProcess RoutingProcess
        {
            get { return this.GetRefEntity(RoutingProcessProperty); }
            set { this.SetRefEntity(RoutingProcessProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 工艺路线工序工治具需求 实体配置
    /// </summary>
    internal class RoutingProcessFixtureConfig : EntityConfig<RoutingProcessFixture>
    {
        /// <summary>
        /// 元数据配置
        /// </summary>
		protected override void ConfigMeta()
        {
            Meta.MapTable("TECH_RT_PROC_FIXTURE").MapAllProperties();
            Meta.Property(RoutingProcessFixture.FixtureIdProperty).ColumnMeta.HasIndex();
            Meta.EnablePhantoms();
        }
    }
}