using SIE.Defects;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Tech.Routings
{
    /// <summary>
    /// 工序清单与缺陷关系
    /// </summary>
    [ChildEntity, Serializable]
    [Label("工序清单与缺陷关系")]
    public partial class RoutingProcessDefect : DataEntity
    {
        #region 缺陷 Defect
        /// <summary>
        /// 缺陷Id
        /// </summary>
        public static readonly IRefIdProperty DefectIdProperty = P<RoutingProcessDefect>.RegisterRefId(e => e.DefectId, ReferenceType.Normal);

        /// <summary>
        /// 缺陷Id
        /// </summary>
        public double DefectId
        {
            get { return (double)GetRefId(DefectIdProperty); }
            set { SetRefId(DefectIdProperty, value); }
        }

        /// <summary>
        /// 缺陷
        /// </summary>
        public static readonly RefEntityProperty<Defect> DefectProperty = P<RoutingProcessDefect>.RegisterRef(e => e.Defect, DefectIdProperty);

        /// <summary>
        /// 缺陷
        /// </summary>
        public Defect Defect
        {
            get { return GetRefEntity(DefectProperty); }
            set { SetRefEntity(DefectProperty, value); }
        }
        #endregion

        #region 工序清单 RoutingProcess
        /// <summary>
        /// 工序清单Id
        /// </summary>
        public static readonly IRefIdProperty RoutingProcessIdProperty = P<RoutingProcessDefect>.RegisterRefId(e => e.RoutingProcessId, ReferenceType.Parent);

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
        public static readonly RefEntityProperty<RoutingProcess> RoutingProcessProperty = P<RoutingProcessDefect>.RegisterRef(e => e.RoutingProcess, RoutingProcessIdProperty);

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
    /// 工序清单与缺陷关系 实体配置
    /// </summary>
    internal class RoutingProcessDefectConfig : EntityConfig<RoutingProcessDefect>
    {
        /// <summary>
        /// 元数据配置
        /// </summary>
		protected override void ConfigMeta()
        {
            Meta.MapTable("TECH_RT_PROC_DEF").MapAllProperties();
            Meta.Property(RoutingProcessDefect.RoutingProcessIdProperty).ColumnMeta.HasIndex();
            Meta.EnablePhantoms();
        }
    }
}