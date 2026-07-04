using SIE.Core.Enums;
using SIE.Domain;
using SIE.Fixtures.FixtureTypes;
using SIE.Fixtures.Models;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.ProcessSegments;
using System;

namespace SIE.Fixtures.FixtureDemands
{
    /// <summary>
	/// 工治具需求明细
	/// </summary>
	[ChildEntity, Serializable]
    //[CriteriaQuery]
    [Label("需求明细")]
    public partial class FixtureDemandDetail : DataEntity
    {
        #region 需求数量 DemandQty
        /// <summary>
        /// 需求数量
        /// </summary>
        [Label("需求数量")]
        public static readonly Property<int> DemandQtyProperty = P<FixtureDemandDetail>.Register(e => e.DemandQty);

        /// <summary>
        /// 需求数量
        /// </summary>
        public int DemandQty
        {
            get { return GetProperty(DemandQtyProperty); }
            set { SetProperty(DemandQtyProperty, value); }
        }
        #endregion

        #region 出库数量 UnloadQty
        /// <summary>
        /// 出库数量
        /// </summary>
        [Label("出库数量")]
        public static readonly Property<int> UnloadQtyProperty = P<FixtureDemandDetail>.Register(e => e.UnloadQty);

        /// <summary>
        /// 出库数量
        /// </summary>
        public int UnloadQty
        {
            get { return GetProperty(UnloadQtyProperty); }
            set { SetProperty(UnloadQtyProperty, value); }
        }
        #endregion

        #region 工艺面 ProcessSurface
        /// <summary>
        /// 工艺面
        /// </summary>
        [Label("工艺面")]
        public static readonly Property<Deck?> ProcessSurfaceProperty = P<FixtureDemandDetail>.Register(e => e.ProcessSurface);

        /// <summary>
        /// 工艺面
        /// </summary>
        public Deck? ProcessSurface
        {
            get { return GetProperty(ProcessSurfaceProperty); }
            set { SetProperty(ProcessSurfaceProperty, value); }
        }
        #endregion


        #region 工治具类型 FixtureType
        /// <summary>
        /// 工治具类型Id
        /// </summary>
        [Label("工治具类型")]
        public static readonly IRefIdProperty FixtureTypeIdProperty =
            P<FixtureDemandDetail>.RegisterRefId(e => e.FixtureTypeId, ReferenceType.Normal);

        /// <summary>
        /// 工治具类型Id
        /// </summary>
        public double? FixtureTypeId
        {
            get { return (double?)this.GetRefNullableId(FixtureTypeIdProperty); }
            set { this.SetRefNullableId(FixtureTypeIdProperty, value); }
        }

        /// <summary>
        /// 工治具类型
        /// </summary>
        public static readonly RefEntityProperty<FixtureType> FixtureTypeProperty =
            P<FixtureDemandDetail>.RegisterRef(e => e.FixtureType, FixtureTypeIdProperty);

        /// <summary>
        /// 工治具类型
        /// </summary>
        public FixtureType FixtureType
        {
            get { return this.GetRefEntity(FixtureTypeProperty); }
            set { this.SetRefEntity(FixtureTypeProperty, value); }
        }
        #endregion

        #region 工治具编码 FixtureEncode
        [Label("工治具编码")]
        /// <summary>
        /// 工治具编码Id
        /// </summary>
        public static readonly IRefIdProperty FixtureEncodeIdProperty = P<FixtureDemandDetail>.RegisterRefId(e => e.FixtureEncodeId, ReferenceType.Normal);

        /// <summary>
        /// 工治具编码Id
        /// </summary>
        public double FixtureEncodeId
        {
            get { return (double)GetRefId(FixtureEncodeIdProperty); }
            set { SetRefId(FixtureEncodeIdProperty, value); }
        }

        /// <summary>
        /// 工治具编码
        /// </summary>
        public static readonly RefEntityProperty<FixtureEncode> FixtureEncodeProperty = P<FixtureDemandDetail>.RegisterRef(e => e.FixtureEncode, FixtureEncodeIdProperty);

        /// <summary>
        /// 工治具编码
        /// </summary>
        public FixtureEncode FixtureEncode
        {
            get { return GetRefEntity(FixtureEncodeProperty); }
            set { SetRefEntity(FixtureEncodeProperty, value); }
        }
        #endregion

        #region 工治具型号 FixtureModel
        /// <summary>
        /// 工治具型号Id
        /// </summary>
        [Label("型号编码")]
        public static readonly IRefIdProperty FixtureModelIdProperty = P<FixtureDemandDetail>.RegisterRefId(e => e.FixtureModelId, ReferenceType.Normal);

        /// <summary>
        /// 工治具型号Id
        /// </summary>
        public double? FixtureModelId
        {
            get { return (double?)GetRefNullableId(FixtureModelIdProperty); }
            set { SetRefNullableId(FixtureModelIdProperty, value); }
        }

        /// <summary>
        /// 工治具型号
        /// </summary>
        public static readonly RefEntityProperty<FixtureModel> FixtureModelProperty = P<FixtureDemandDetail>.RegisterRef(e => e.FixtureModel, FixtureModelIdProperty);

        /// <summary>
        /// 工治具型号
        /// </summary>
        public FixtureModel FixtureModel
        {
            get { return GetRefEntity(FixtureModelProperty); }
            set { SetRefEntity(FixtureModelProperty, value); }
        }
        #endregion

        #region 工治具需求明细 FixtureDemand
        /// <summary>
        /// 工治具需求明细Id
        /// </summary>
        public static readonly IRefIdProperty FixtureDemandIdProperty = P<FixtureDemandDetail>.RegisterRefId(e => e.FixtureDemandId, ReferenceType.Parent);

        /// <summary>
        /// 工治具需求明细Id
        /// </summary>
        public double FixtureDemandId
        {
            get { return (double)GetRefId(FixtureDemandIdProperty); }
            set { SetRefId(FixtureDemandIdProperty, value); }
        }

        /// <summary>
        /// 工治具需求明细
        /// </summary>
        public static readonly RefEntityProperty<FixtureDemand> FixtureDemandProperty = P<FixtureDemandDetail>.RegisterRef(e => e.FixtureDemand, FixtureDemandIdProperty);

        /// <summary>
        /// 工治具需求明细
        /// </summary>
        public FixtureDemand FixtureDemand
        {
            get { return GetRefEntity(FixtureDemandProperty); }
            set { SetRefEntity(FixtureDemandProperty, value); }
        }
        #endregion


        #region 工段 ProcessSegmen
        /// <summary>
        /// 工段Id
        /// </summary>
        [Label("工段")]
        public static readonly IRefIdProperty ProcessSegmentIdProperty =
            P<FixtureDemandDetail>.RegisterRefId(e => e.ProcessSegmentId, ReferenceType.Normal);

        /// <summary>
        /// 工段Id
        /// </summary>
        public double? ProcessSegmentId
        {
            get { return (double?)this.GetRefNullableId(ProcessSegmentIdProperty); }
            set { this.SetRefNullableId(ProcessSegmentIdProperty, value); }
        }

        /// <summary>
        /// 工段
        /// </summary>
        public static readonly RefEntityProperty<ProcessSegment> ProcessSegmentProperty =
            P<FixtureDemandDetail>.RegisterRef(e => e.ProcessSegment, ProcessSegmentIdProperty);

        /// <summary>
        /// 工段
        /// </summary>
        public ProcessSegment ProcessSegment
        {
            get { return this.GetRefEntity(ProcessSegmentProperty); }
            set { this.SetRefEntity(ProcessSegmentProperty, value); }
        }
        #endregion

        #region 视图属性

        #region 工治具编码 EncodeCode
        /// <summary>
        /// 工治具编码
        /// </summary>
        [Label("工治具编码")]
        public static readonly Property<string> EncodeCodeProperty = P<FixtureDemandDetail>.RegisterView(e => e.EncodeCode, p => p.FixtureEncode.Code);

        /// <summary>
        /// 工治具编码
        /// </summary>
        public string EncodeCode
        {
            get { return this.GetProperty(EncodeCodeProperty); }
        }
        #endregion

        #region 型号编码 ModelCode
        /// <summary>
        /// 型号编码
        /// </summary>
        [Label("型号编码")]
        public static readonly Property<string> ModelCodeProperty = P<FixtureDemandDetail>.RegisterView(e => e.ModelCode, p => p.FixtureModel.Code);

        /// <summary>
        /// 型号编码
        /// </summary>
        public string ModelCode
        {
            get { return this.GetProperty(ModelCodeProperty); }
        }
        #endregion

        #region 型号名称 ModelName
        /// <summary>
        /// 型号名称
        /// </summary>
        [Label("型号名称")]
        public static readonly Property<string> ModelNameProperty = P<FixtureDemandDetail>.RegisterView(e => e.ModelName, p => p.FixtureModel.Name);

        /// <summary>
        /// 型号名称
        /// </summary>
        public string ModelName
        {
            get { return this.GetProperty(ModelNameProperty); }
        }
        #endregion


        #region 工治具编码 FixtureEncodeCode
        /// <summary>
        /// 工治具编码
        /// </summary>
        [Label("工治具编码")]
        public static readonly Property<string> FixtureEncodeCodeProperty = P<FixtureDemandDetail>.RegisterView(e => e.FixtureEncodeCode, p => p.FixtureEncode.Code);

        /// <summary>
        /// 工治具编码
        /// </summary>
        public string FixtureEncodeCode
        {
            get { return this.GetProperty(FixtureEncodeCodeProperty); }
            set { this.SetProperty(FixtureEncodeCodeProperty, value); }
        }
        #endregion


        #region 工段编码 ProcessSegmentCode
        /// <summary>
        /// 工段编码
        /// </summary>
        [Label("工段编码")]
        public static readonly Property<string> ProcessSegmentCodeProperty = P<FixtureDemandDetail>.RegisterView(e => e.ProcessSegmentCode, p => p.ProcessSegment.Code);

        /// <summary>
        /// 工段编码
        /// </summary>
        public string ProcessSegmentCode
        {
            get { return this.GetProperty(ProcessSegmentCodeProperty); }
            set { this.SetProperty(ProcessSegmentCodeProperty, value); }
        }
        #endregion


        #region 表头工段编码 ParentProcessSegmentId
        /// <summary>
        /// 表头工段编码
        /// </summary>
        [Label("表头工段编码")]
        public static readonly Property<double?> ParentProcessSegmentIdProperty = P<FixtureDemandDetail>.RegisterView(e => e.ParentProcessSegmentId, p => p.FixtureDemand.ProcessSegmentId);

        /// <summary>
        /// 工段编码
        /// </summary>
        public double? ParentProcessSegmentId
        {
            get { return this.GetProperty(ParentProcessSegmentIdProperty); }
            set { this.SetProperty(ParentProcessSegmentIdProperty, value); }
        }
        #endregion

        #region FixtureTypeCode FixtureTypeCode
        /// <summary>
        /// 工治具类型编码
        /// </summary>
        [Label("工治具类型编码")]
        public static readonly Property<string> FixtureTypeCodeProperty = P<FixtureDemandDetail>.RegisterView(e => e.FixtureTypeCode, p => p.FixtureType.Code);

        /// <summary>
        /// 工治具类型编码
        /// </summary>
        public string FixtureTypeCode
        {
            get { return this.GetProperty(FixtureTypeCodeProperty); }
        }
        #endregion


        #endregion
    }

    /// <summary>
    /// 工治具需求明细 实体配置
    /// </summary>
    internal class FixtureDemandDetailConfig : EntityConfig<FixtureDemandDetail>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_FIX_DEMAND_DTL").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
