using SIE.Common;
using SIE.Domain;
using SIE.MES.InspectionStandards;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources;
using SIE.Resources.ShiftTypes;
using SIE.Tech.Processs;
using SIE.Tech.Stations;
using System;

namespace SIE.MES.WIP.Products
{
    /// <summary>
    /// 产品检验记录
    /// </summary>
    [ChildEntity, Serializable]
    [Label("产品检验记录")]
    public partial class WipProductInspectionItem : DataEntity
    {
        #region 项目名称 Name
        /// <summary>
        /// 项目名称
        /// </summary>
        [Label("项目名称")]
        public static readonly Property<string> NameProperty = P<WipProductInspectionItem>.Register(e => e.Name);

        /// <summary>
        /// 项目名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 规范下限 LimitLow
        /// <summary>
        /// 规范下限
        /// </summary>
        [Label("规范下限")]
        public static readonly Property<decimal?> LimitLowProperty = P<WipProductInspectionItem>.Register(e => e.LimitLow);

        /// <summary>
        /// 规范下限
        /// </summary>
        public decimal? LimitLow
        {
            get { return GetProperty(LimitLowProperty); }
            set { SetProperty(LimitLowProperty, value); }
        }
        #endregion

        #region 规范上限 LimitMax
        /// <summary>
        /// 规范上限
        /// </summary>
        [Label("规范上限")]
        public static readonly Property<decimal?> LimitMaxProperty = P<WipProductInspectionItem>.Register(e => e.LimitMax);

        /// <summary>
        /// 规范上限
        /// </summary>
        public decimal? LimitMax
        {
            get { return GetProperty(LimitMaxProperty); }
            set { SetProperty(LimitMaxProperty, value); }
        }
        #endregion

        #region 测试值 InspectionValue
        /// <summary>
        /// 测试值
        /// </summary>
        [Label("测试值")]
        public static readonly Property<decimal?> InspectionValueProperty = P<WipProductInspectionItem>.Register(e => e.InspectionValue);

        /// <summary>
        /// 测试值
        /// </summary>
        public decimal? InspectionValue
        {
            get { return GetProperty(InspectionValueProperty); }
            set { SetProperty(InspectionValueProperty, value); }
        }
        #endregion

        #region 备注 Remarks
        /// <summary>
        /// 备注
        /// </summary>
        [Label("备注")]
        public static readonly Property<string> RemarksProperty = P<WipProductInspectionItem>.Register(e => e.Remarks);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks
        {
            get { return GetProperty(RemarksProperty); }
            set { SetProperty(RemarksProperty, value); }
        }
        #endregion

        #region 工序 Process
        /// <summary>
        /// 工序Id
        /// </summary>
        [Label("工序")]
        public static readonly IRefIdProperty ProcessIdProperty = P<WipProductInspectionItem>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Process> ProcessProperty = P<WipProductInspectionItem>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 工序
        /// </summary>
        public Process Process
        {
            get { return GetRefEntity(ProcessProperty); }
            set { SetRefEntity(ProcessProperty, value); }
        }
        #endregion

        #region 检验结果 Result
        /// <summary>
        /// 检验结果
        /// </summary>
        [Label("检验结果")]
        public static readonly Property<ResultType> ResultProperty = P<WipProductInspectionItem>.Register(e => e.Result);

        /// <summary>
        /// 检验结果
        /// </summary>
        public ResultType Result
        {
            get { return GetProperty(ResultProperty); }
            set { SetProperty(ResultProperty, value); }
        }
        #endregion

        #region 检验项目 InspectionItem
        /// <summary>
        /// 检验项目Id
        /// </summary>
        [Label("检验项目")]
        public static readonly IRefIdProperty InspectionItemIdProperty = P<WipProductInspectionItem>.RegisterRefId(e => e.InspectionItemId, ReferenceType.Normal);

        /// <summary>
        /// 检验项目Id
        /// </summary>
        public double InspectionItemId
        {
            get { return (double)GetRefId(InspectionItemIdProperty); }
            set { SetRefId(InspectionItemIdProperty, value); }
        }

        /// <summary>
        /// 检验项目
        /// </summary>
        public static readonly RefEntityProperty<ModelInspectionItem> InspectionItemProperty = P<WipProductInspectionItem>.RegisterRef(e => e.InspectionItem, InspectionItemIdProperty);

        /// <summary>
        /// 检验项目
        /// </summary>
        public ModelInspectionItem InspectionItem
        {
            get { return GetRefEntity(InspectionItemProperty); }
            set { SetRefEntity(InspectionItemProperty, value); }
        }
        #endregion

        #region 检验人 InspectBy
        /// <summary>
        /// 检验人Id
        /// </summary>
        [Label("检验人")]
        public static readonly IRefIdProperty InspectByIdProperty = P<WipProductInspectionItem>.RegisterRefId(e => e.InspectById, ReferenceType.Normal);

        /// <summary>
        /// 检验人Id
        /// </summary>
        public double? InspectById
        {
            get { return (double?)GetRefNullableId(InspectByIdProperty); }
            set { SetRefNullableId(InspectByIdProperty, value); }
        }

        /// <summary>
        /// 检验人
        /// </summary>
        public static readonly RefEntityProperty<Employee> InspectByProperty = P<WipProductInspectionItem>.RegisterRef(e => e.InspectBy, InspectByIdProperty);

        /// <summary>
        /// 检验人
        /// </summary>
        public Employee InspectBy
        {
            get { return GetRefEntity(InspectByProperty); }
            set { SetRefEntity(InspectByProperty, value); }
        }
        #endregion

        #region 班次 Shift
        /// <summary>
        /// 班次Id
        /// </summary>
        [Label("班次")]
        public static readonly IRefIdProperty ShiftIdProperty = P<WipProductInspectionItem>.RegisterRefId(e => e.ShiftId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Shift> ShiftProperty = P<WipProductInspectionItem>.RegisterRef(e => e.Shift, ShiftIdProperty);

        /// <summary>
        /// 班次
        /// </summary>
        public Shift Shift
        {
            get { return GetRefEntity(ShiftProperty); }
            set { SetRefEntity(ShiftProperty, value); }
        }
        #endregion

        #region 工位 Station
        /// <summary>
        /// 工位Id
        /// </summary>
        [Label("工位")]
        public static readonly IRefIdProperty StationIdProperty = P<WipProductInspectionItem>.RegisterRefId(e => e.StationId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Station> StationProperty = P<WipProductInspectionItem>.RegisterRef(e => e.Station, StationIdProperty);

        /// <summary>
        /// 工位
        /// </summary>
        public Station Station
        {
            get { return GetRefEntity(StationProperty); }
            set { SetRefEntity(StationProperty, value); }
        }
        #endregion

        #region 检验记录 Version
        /// <summary>
        /// 检验记录Id
        /// </summary>
        [Label("检验记录")]
        public static readonly IRefIdProperty VersionIdProperty = P<WipProductInspectionItem>.RegisterRefId(e => e.VersionId, ReferenceType.Parent);

        /// <summary>
        /// 检验记录Id
        /// </summary>
        public double VersionId
        {
            get { return (double)GetRefId(VersionIdProperty); }
            set { SetRefId(VersionIdProperty, value); }
        }

        /// <summary>
        /// 检验记录
        /// </summary>
        public static readonly RefEntityProperty<WipProductVersion> VersionProperty = P<WipProductInspectionItem>.RegisterRef(e => e.Version, VersionIdProperty);

        /// <summary>
        /// 检验记录
        /// </summary>
        public WipProductVersion Version
        {
            get { return GetRefEntity(VersionProperty); }
            set { SetRefEntity(VersionProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 检验人名称 InspectByName
        /// <summary>
        /// 检验人名称
        /// </summary>
        [Label("检验人")]
        public static readonly Property<string> InspectByNameProperty = P<WipProductInspectionItem>.RegisterView(e => e.InspectByName, p => p.InspectBy.Name);

        /// <summary>
        /// 检验人名称
        /// </summary>
        public string InspectByName
        {
            get { return this.GetProperty(InspectByNameProperty); }
        }
        #endregion

        #region 检验项目名称 InspectionItemName
        /// <summary>
        /// 检验项目名称
        /// </summary>
        [Label("检验项目名称")]
        public static readonly Property<string> InspectionItemNameProperty = P<WipProductInspectionItem>.RegisterView(e => e.InspectionItemName, p => p.InspectionItem.Name);

        /// <summary>
        /// 检验项目名称
        /// </summary>
        public string InspectionItemName
        {
            get { return this.GetProperty(InspectionItemNameProperty); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 产品检验记录 实体配置
    /// </summary>
    internal class WipProductInspectionItemConfig : EntityConfig<WipProductInspectionItem>
    {
        /// <summary>
        /// 数据库的映射
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WIP_PROD_INSPECT_ITEM").MapAllProperties();
            Meta.Property(WipProductInspectionItem.VersionIdProperty).ColumnMeta.HasIndex();
            Meta.EnablePhantoms();
        }
    }
}