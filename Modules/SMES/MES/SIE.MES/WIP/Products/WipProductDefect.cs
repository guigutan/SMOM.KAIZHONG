using SIE.Common;
using SIE.Defects;
using SIE.Domain;
using SIE.MES.InspectionStandards;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources;
using SIE.Resources.ShiftTypes;
using SIE.Resources.WipResources;
using SIE.Tech.Processs;
using SIE.Tech.Stations;
using System;

namespace SIE.MES.WIP.Products
{
    /// <summary>
    /// 产品缺陷记录
    /// </summary>
    [ChildEntity, Serializable]
    [Label("产品缺陷记录")]
    public partial class WipProductDefect : DataEntity
    {
        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [Label("备注")]
        [MaxLength(240)]
        public static readonly Property<string> RemarkProperty = P<WipProductDefect>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return GetProperty(RemarkProperty); }
            set { SetProperty(RemarkProperty, value); }
        }
        #endregion

        #region 缺陷位置 Location
        /// <summary>
        /// 缺陷位置
        /// </summary>
        [Label("缺陷位置")]
        public static readonly Property<string> LocationProperty = P<WipProductDefect>.Register(e => e.Location);

        /// <summary>
        /// 缺陷位置
        /// </summary>
        public string Location
        {
            get { return GetProperty(LocationProperty); }
            set { SetProperty(LocationProperty, value); }
        }
        #endregion

        #region 是否维修过 IsFixed
        /// <summary>
        /// 是否维修过
        /// </summary>
        [Label("是否维修过")]
        public static readonly Property<bool> IsFixedProperty = P<WipProductDefect>.Register(e => e.IsFixed);

        /// <summary>
        /// 是否维修过
        /// </summary>
        public bool IsFixed
        {
            get { return GetProperty(IsFixedProperty); }
            set { SetProperty(IsFixedProperty, value); }
        }
        #endregion

        #region 维修时间 FixedDate
        /// <summary>
        /// 维修时间
        /// </summary>
        [Label("维修时间")]
        public static readonly Property<DateTime?> FixedDateProperty = P<WipProductDefect>.Register(e => e.FixedDate);

        /// <summary>
        /// 维修时间
        /// </summary>
        public DateTime? FixedDate
        {
            get { return GetProperty(FixedDateProperty); }
            set { SetProperty(FixedDateProperty, value); }
        }
        #endregion

        #region 采集结果 Result
        /// <summary>
        /// 采集结果
        /// </summary>
        [Label("采集结果")]
        public static readonly Property<ResultType> ResultProperty = P<WipProductDefect>.Register(e => e.Result);

        /// <summary>
        /// 采集结果
        /// </summary>
        public ResultType Result
        {
            get { return GetProperty(ResultProperty); }
            set { SetProperty(ResultProperty, value); }
        }
        #endregion

        #region 工序 Process
        /// <summary>
        /// 工序Id
        /// </summary>
        [Label("工序")]
        public static readonly IRefIdProperty ProcessIdProperty = P<WipProductDefect>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

        /// <summary>
        /// 工序Id
        /// </summary>
        public double ProcessId
        {
            get { return (double)GetRefId(ProcessIdProperty); }
            set { SetRefId(ProcessIdProperty, value); }
        }

        /// <summary>
        /// 工序
        /// </summary>
        public static readonly RefEntityProperty<Process> ProcessProperty = P<WipProductDefect>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 工序
        /// </summary>
        public Process Process
        {
            get { return GetRefEntity(ProcessProperty); }
            set { SetRefEntity(ProcessProperty, value); }
        }
        #endregion

        #region 资源 Resource
        /// <summary>
        /// 资源
        /// </summary>
        [Label("资源")]
        public static readonly IRefIdProperty ResourceIdProperty = P<WipProductDefect>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

        /// <summary>
        /// 资源
        /// </summary>
        public double ResourceId
        {
            get { return (double)GetRefId(ResourceIdProperty); }
            set { SetRefId(ResourceIdProperty, value); }
        }

        /// <summary>
        /// 资源
        /// </summary>
        public static readonly RefEntityProperty<WipResource> ResourceProperty = P<WipProductDefect>.RegisterRef(e => e.Resource, ResourceIdProperty);

        /// <summary>
        /// 资源
        /// </summary>
        public WipResource Resource
        {
            get { return GetRefEntity(ResourceProperty); }
            set { SetRefEntity(ResourceProperty, value); }
        }
        #endregion

        #region 工位 Station
        /// <summary>
        /// 工位Id
        /// </summary>
        [Label("工位")]
        public static readonly IRefIdProperty StationIdProperty = P<WipProductDefect>.RegisterRefId(e => e.StationId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Station> StationProperty = P<WipProductDefect>.RegisterRef(e => e.Station, StationIdProperty);

        /// <summary>
        /// 工位
        /// </summary>
        public Station Station
        {
            get { return GetRefEntity(StationProperty); }
            set { SetRefEntity(StationProperty, value); }
        }
        #endregion

        #region 维修人 FixedBy
        /// <summary>
        /// 维修人Id
        /// </summary>
        [Label("维修人")]
        public static readonly IRefIdProperty FixedByIdProperty = P<WipProductDefect>.RegisterRefId(e => e.FixedById, ReferenceType.Normal);

        /// <summary>
        /// 维修人Id
        /// </summary>
        public double? FixedById
        {
            get { return (double?)GetRefNullableId(FixedByIdProperty); }
            set { SetRefNullableId(FixedByIdProperty, value); }
        }

        /// <summary>
        /// 维修人
        /// </summary>
        public static readonly RefEntityProperty<Employee> FixedByProperty = P<WipProductDefect>.RegisterRef(e => e.FixedBy, FixedByIdProperty);

        /// <summary>
        /// 维修人
        /// </summary>
        public Employee FixedBy
        {
            get { return GetRefEntity(FixedByProperty); }
            set { SetRefEntity(FixedByProperty, value); }
        }
        #endregion

        #region 班次 Shift
        /// <summary>
        /// 班次Id
        /// </summary>
        [Label("班次")]
        public static readonly IRefIdProperty ShiftIdProperty = P<WipProductDefect>.RegisterRefId(e => e.ShiftId, ReferenceType.Normal);

        /// <summary>
        /// 班次Id
        /// </summary>
        public double ShiftId
        {
            get { return (double)GetRefId(ShiftIdProperty); }
            set { SetRefId(ShiftIdProperty, value); }
        }

        /// <summary>
        /// 班次
        /// </summary>
        public static readonly RefEntityProperty<Shift> ShiftProperty = P<WipProductDefect>.RegisterRef(e => e.Shift, ShiftIdProperty);

        /// <summary>
        /// 班次
        /// </summary>
        public Shift Shift
        {
            get { return GetRefEntity(ShiftProperty); }
            set { SetRefEntity(ShiftProperty, value); }
        }
        #endregion

        #region 缺陷 Defect
        /// <summary>
        /// 缺陷Id
        /// </summary>
        [Label("缺陷")]
        public static readonly IRefIdProperty DefectIdProperty = P<WipProductDefect>.RegisterRefId(e => e.DefectId, ReferenceType.Normal);

        /// <summary>
        /// 缺陷Id
        /// </summary>
        public double? DefectId
        {
            get { return (double?)GetRefNullableId(DefectIdProperty); }
            set { SetRefNullableId(DefectIdProperty, value); }
        }

        /// <summary>
        /// 缺陷
        /// </summary>
        public static readonly RefEntityProperty<Defect> DefectProperty = P<WipProductDefect>.RegisterRef(e => e.Defect, DefectIdProperty);

        /// <summary>
        /// 缺陷
        /// </summary>
        public Defect Defect
        {
            get { return GetRefEntity(DefectProperty); }
            set { SetRefEntity(DefectProperty, value); }
        }
        #endregion

        #region 缺陷描述 DefectDescription
        /// <summary>
        /// 缺陷描述
        /// </summary>
        [Label("缺陷描述")]
        public static readonly Property<string> DefectDescriptionProperty = P<WipProductDefect>.RegisterView(e => e.DefectDescription, p => p.Defect.Description);

        /// <summary>
        /// 缺陷描述
        /// </summary>
        public string DefectDescription
        {
            get { return this.GetProperty(DefectDescriptionProperty); }
        }
        #endregion

        #region 产品版本 Version
        /// <summary>
        /// 产品版本
        /// </summary>
        [Label("产品版本")]
        public static readonly IRefIdProperty VersionIdProperty = P<WipProductDefect>.RegisterRefId(e => e.VersionId, ReferenceType.Parent);

        /// <summary>
        /// 产品版本
        /// </summary>
        public double VersionId
        {
            get { return (double)GetRefId(VersionIdProperty); }
            set { SetRefId(VersionIdProperty, value); }
        }

        /// <summary>
        /// 产品版本
        /// </summary>
        public static readonly RefEntityProperty<WipProductVersion> VersionProperty = P<WipProductDefect>.RegisterRef(e => e.Version, VersionIdProperty);

        /// <summary>
        /// 产品版本
        /// </summary>
        public WipProductVersion Version
        {
            get { return GetRefEntity(VersionProperty); }
            set { SetRefEntity(VersionProperty, value); }
        }
        #endregion

        #region 不良数量 NgQty
        /// <summary>
        /// 不良数量
        /// </summary>
        [Label("不良数量")]
        public static readonly Property<decimal> NgQtyProperty = P<WipProductDefect>.Register(e => e.NgQty);

        /// <summary>
        /// 不良数量
        /// </summary>
        public decimal NgQty
        {
            get { return this.GetProperty(NgQtyProperty); }
            set { this.SetProperty(NgQtyProperty, value); }
        }
        #endregion 

        #region 检验项目 InspectionItem
        /// <summary>
        /// 检验项目Id
        /// </summary>
        [Label("检验项目")]
        public static readonly IRefIdProperty InspectionItemIdProperty = P<WipProductDefect>.RegisterRefId(e => e.InspectionItemId, ReferenceType.Normal);

        /// <summary>
        /// 检验项目Id
        /// </summary>
        public double? InspectionItemId
        {
            get { return (double?)GetRefNullableId(InspectionItemIdProperty); }
            set { SetRefNullableId(InspectionItemIdProperty, value); }
        }

        /// <summary>
        /// 检验项目
        /// </summary>
        public static readonly RefEntityProperty<ModelInspectionItem> InspectionItemProperty = P<WipProductDefect>.RegisterRef(e => e.InspectionItem, InspectionItemIdProperty);

        /// <summary>
        /// 检验项目
        /// </summary>
        public ModelInspectionItem InspectionItem
        {
            get { return GetRefEntity(InspectionItemProperty); }
            set { SetRefEntity(InspectionItemProperty, value); }
        }
        #endregion

        #region 是否误判 IsMisjudgment
        /// <summary>
        /// 是否误判
        /// </summary>
        [Label("是否误判")]
        public static readonly Property<bool> IsMisjudgmentProperty = P<WipProductDefect>.Register(e => e.IsMisjudgment);

        /// <summary>
        /// 是否误判
        /// </summary>
        public bool IsMisjudgment
        {
            get { return this.GetProperty(IsMisjudgmentProperty); }
            set { this.SetProperty(IsMisjudgmentProperty, value); }
        }
        #endregion

        #region 板号 BoardNo
        /// <summary>
        /// 板号
        /// </summary>
        [Label("板号")]
        public static readonly Property<int?> BoardNoProperty = P<WipProductDefect>.Register(e => e.BoardNo);

        /// <summary>
        /// 板号
        /// </summary>
        public int? BoardNo
        {
            get { return this.GetProperty(BoardNoProperty); }
            set { this.SetProperty(BoardNoProperty, value); }
        }
        #endregion

        #region 条码 Sn
        /// <summary>
        /// 条码
        /// </summary>
        [MaxLength(80)]
        [Label("条码")]
        public static readonly Property<string> SnProperty = P<WipProductDefect>.Register(e => e.Sn);

        /// <summary>
        /// 条码
        /// </summary>
        public string Sn
        {
            get { return GetProperty(SnProperty); }
            set { SetProperty(SnProperty, value); }
        }
        #endregion

        #region 缺陷责任 ResponsibilityList
        /// <summary>
        /// 缺陷责任
        /// </summary>
        [Label("缺陷责任")]
        public static readonly ListProperty<EntityList<WipDefectResponsibility>> ResponsibilityListProperty = P<WipProductDefect>.RegisterList(e => e.ResponsibilityList);

        /// <summary>
        /// 缺陷责任
        /// </summary>
        public EntityList<WipDefectResponsibility> ResponsibilityList
        {
            get { return this.GetLazyList(ResponsibilityListProperty); }
        }
        #endregion

        #region 维修措施 MeasureList
        /// <summary>
        /// 维修措施
        /// </summary>
        [Label("维修措施")]
        public static readonly ListProperty<EntityList<WipDefectMeasure>> MeasureListProperty = P<WipProductDefect>.RegisterList(e => e.MeasureList);

        /// <summary>
        /// 维修措施
        /// </summary>
        public EntityList<WipDefectMeasure> MeasureList
        {
            get { return this.GetLazyList(MeasureListProperty); }
        }
        #endregion

        #region 视图属性
        #region 工位名称 StationName
        /// <summary>
        /// 工位名称
        /// </summary>
        [Label("工位")]
        public static readonly Property<string> StationNameProperty = P<WipProductDefect>.RegisterView(e => e.StationName, p => p.Station.Name);

        /// <summary>
        /// 工位名称
        /// </summary>
        public string StationName
        {
            get { return this.GetProperty(StationNameProperty); }
        }
        #endregion

        #region 工序名称 ProcessName
        /// <summary>
        /// 工序名称
        /// </summary>
        [Label("工序")]
        public static readonly Property<string> ProcessNameProperty = P<WipProductDefect>.RegisterView(e => e.ProcessName, p => p.Process.Name);

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessName
        {
            get { return this.GetProperty(ProcessNameProperty); }
        }
        #endregion

        #region 资源名称 ResourceName
        /// <summary>
        /// 资源名称
        /// </summary>
        [Label("资源")]
        public static readonly Property<string> ResourceNameProperty = P<WipProductDefect>.RegisterView(e => e.ResourceName, p => p.Resource.Name);

        /// <summary>
        /// 资源名称
        /// </summary>
        public string ResourceName
        {
            get { return this.GetProperty(ResourceNameProperty); }
        }
        #endregion

        #region 维修人名称 EmployeeName
        /// <summary>
        /// 维修人名称
        /// </summary>
        [Label("维修人")]
        public static readonly Property<string> EmployeeNameProperty = P<WipProductDefect>.RegisterView(e => e.EmployeeName, p => p.FixedBy.Name);

        /// <summary>
        /// 维修人名称
        /// </summary>
        public string EmployeeName
        {
            get { return this.GetProperty(EmployeeNameProperty); }
        }
        #endregion

        #region 缺陷编码 DefectCode
        /// <summary>
        /// 缺陷编码
        /// </summary>
        [Label("缺陷编码")]
        public static readonly Property<string> DefectCodeProperty = P<WipProductDefect>.RegisterView(e => e.DefectCode, p => p.Defect.Code);

        /// <summary>
        /// 缺陷编码
        /// </summary>
        public string DefectCode
        {
            get { return this.GetProperty(DefectCodeProperty); }
        }
        #endregion

        #region 缺陷描述 DefectDesc
        /// <summary>
        /// 缺陷描述
        /// </summary>
        [Label("缺陷描述")]
        public static readonly Property<string> DefectDescProperty = P<WipProductDefect>.RegisterView(e => e.DefectDesc, p => p.Defect.Description);

        /// <summary>
        /// 缺陷描述
        /// </summary>
        public string DefectDesc
        {
            get { return this.GetProperty(DefectDescProperty); }
        }
        #endregion

        #region 检验项描述 InspItemName
        /// <summary>
        /// 检验项描述
        /// </summary>
        [Label("检验项名称")]
        public static readonly Property<string> InspItemNameProperty = P<WipProductDefect>.RegisterView(e => e.InspItemName, p => p.InspectionItem.Name);

        /// <summary>
        /// 检验项描述
        /// </summary>
        public string InspItemName
        {
            get { return this.GetProperty(InspItemNameProperty); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 产品缺陷记录 实体配置
    /// </summary>
    internal class WipProductDefectConfig : EntityConfig<WipProductDefect>
    {
        /// <summary>
        /// 配置数据库映射
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WIP_PROD_DEFECT").MapAllProperties();
            Meta.Property(WipProductDefect.VersionIdProperty).ColumnMeta.HasIndex();
            Meta.Property(WipProductDefect.RemarkProperty).ColumnMeta.HasLength("960");
            Meta.EnablePhantoms();
        }
    }
}