using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Tech.Processs;
using System;

namespace SIE.Tech.Stations
{
    /// <summary>
    /// 工位工序
    /// </summary>
    [ChildEntity, Serializable]
    [Label("工位工序")]
    public partial class StationProcess : DataEntity
    {
        #region 工位 Station
        /// <summary>
        /// 工位Id
        /// </summary>
        public static readonly IRefIdProperty StationIdProperty = P<StationProcess>.RegisterRefId(e => e.StationId, ReferenceType.Parent);

        /// <summary>
        /// 工位Id
        /// </summary>
        public double StationId
        {
            get { return (double)GetRefId(StationIdProperty); }
            set { SetRefId(StationIdProperty, value); }
        }

        /// <summary>
        /// 工位
        /// </summary>
        public static readonly RefEntityProperty<Station> StationProperty = P<StationProcess>.RegisterRef(e => e.Station, StationIdProperty);

        /// <summary>
        /// 工位
        /// </summary>
        public Station Station
        {
            get { return GetRefEntity(StationProperty); }
            set { SetRefEntity(StationProperty, value); }
        }
        #endregion

        #region 工序 Process
        /// <summary>
        /// 工序Id
        /// </summary>
        [Label("工序")]
        public static readonly IRefIdProperty ProcessIdProperty =
            P<StationProcess>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

        /// <summary>
        /// 工序Id
        /// </summary>
        public double ProcessId
        {
            get { return (double)this.GetRefId(ProcessIdProperty); }
            set { this.SetRefId(ProcessIdProperty, value); }
        }

        /// <summary>
        /// 工序
        /// </summary>
        public static readonly RefEntityProperty<Process> ProcessProperty =
            P<StationProcess>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 工序
        /// </summary>
        public Process Process
        {
            get { return this.GetRefEntity(ProcessProperty); }
            set { this.SetRefEntity(ProcessProperty, value); }
        }
        #endregion

        #region 工步 WorkStep
        /// <summary>
        /// 工步Id
        /// </summary>
        [Label("关联工步")]
        public static readonly IRefIdProperty WorkStepIdProperty = P<StationProcess>.RegisterRefId(e => e.WorkStepId, ReferenceType.Normal);

        /// <summary>
        /// 工步Id
        /// </summary>
        public double? WorkStepId
        {
            get { return (double?)GetRefNullableId(WorkStepIdProperty); }
            set { SetRefNullableId(WorkStepIdProperty, value); }
        }

        /// <summary>
        /// 工步
        /// </summary>
        public static readonly RefEntityProperty<WorkStep> WorkStepProperty = P<StationProcess>.RegisterRef(e => e.WorkStep, WorkStepIdProperty);

        /// <summary>
        /// 工步
        /// </summary>
        public WorkStep WorkStep
        {
            get { return GetRefEntity(WorkStepProperty); }
            set { SetRefEntity(WorkStepProperty, value); }
        }
        #endregion

        #region 视图属性

        #region 产品族名称 ProductFamilyName
        /// <summary>
        /// 产品族名称
        /// </summary>
        [Label("产品族名称")]
        public static readonly Property<string> ProductFamilyNameProperty = P<StationProcess>.RegisterView(e => e.ProductFamilyName, p => p.Process.ProductFamily.Name);

        /// <summary>
        /// 产品族名称
        /// </summary>
        public string ProductFamilyName
        {
            get { return this.GetProperty(ProductFamilyNameProperty); }
        }
        #endregion

        #region 工序类型 ProcessType
        /// <summary>
        /// 工序类型
        /// </summary>
        [Label("工序类型")]
        public static readonly Property<ProcessType> ProcessTypeProperty = P<StationProcess>.RegisterView(e => e.ProcessType, p => p.Process.Type);

        /// <summary>
        /// 工序类型
        /// </summary>
        public ProcessType ProcessType
        {
            get { return this.GetProperty(ProcessTypeProperty); }
        }
        #endregion

        #region 工段 SegmentName
        /// <summary>
        /// 工段
        /// </summary>
        [Label("工段")]
        public static readonly Property<string> SegmentNameProperty = P<StationProcess>.RegisterView(e => e.SegmentName, p => p.Process.Segment.Name);

        /// <summary>
        /// 工段
        /// </summary>
        public string SegmentName
        {
            get { return this.GetProperty(SegmentNameProperty); }
        }
        #endregion

        #region 工位编码 StationCode
        /// <summary>
        /// 工位编码
        /// </summary>
        [Label("工位编码")]
        public static readonly Property<string> StationCodeProperty = P<StationProcess>.RegisterView(e => e.StationCode, p => p.Station.Code);

        /// <summary>
        /// 工位编码
        /// </summary>
        public string StationCode
        {
            get { return this.GetProperty(StationCodeProperty); }
        }
        #endregion

        #region 工序名称 StationName
        /// <summary>
        /// 工序名称
        /// </summary>
        [Label("工序名称")]
        public static readonly Property<string> StationNameProperty = P<StationProcess>.RegisterView(e => e.StationName, p => p.Station.Name);

        /// <summary>
        /// 工序名称
        /// </summary>
        public string StationName
        {
            get { return this.GetProperty(StationNameProperty); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 工位工序 实体配置
    /// </summary>
    internal class StationProcessConfig : EntityConfig<StationProcess>
    {
        /// <summary>
        /// 实体元数据配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("TECH_STATION_PROC").MapAllProperties();
            Meta.Property(StationProcess.StationIdProperty).ColumnMeta.HasIndex();
            Meta.EnablePhantoms();
        }
    }
}
