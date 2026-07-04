using SIE.Core.Enums;
using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.Fixtures.FixtureTypes;
using SIE.Fixtures.Repairs;
using SIE.Items;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using System;
using FixtureEncode = SIE.Fixtures.Models.FixtureEncode;
using FixtureModel = SIE.Fixtures.Models.FixtureModel;

namespace SIE.Fixtures.Querys.ViewModels
{
    /// <summary>
	/// 工治具查询查询体
	/// </summary>
	[QueryEntity, Serializable]
    [Label("工治具查询查询体")]
    public partial class FixtureQueryViewModelCriteria : Criteria
    {


        #region 工治具类型 FixtureType
        /// <summary>
        /// 工治具类型Id
        /// </summary>
        [Label("工治具类型")]
        public static readonly IRefIdProperty FixtureTypeIdProperty =
            P<FixtureQueryViewModelCriteria>.RegisterRefId(e => e.FixtureTypeId, ReferenceType.Normal);

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
            P<FixtureQueryViewModelCriteria>.RegisterRef(e => e.FixtureType, FixtureTypeIdProperty);

        /// <summary>
        /// 工治具类型
        /// </summary>
        public FixtureType FixtureType
        {
            get { return this.GetRefEntity(FixtureTypeProperty); }
            set { this.SetRefEntity(FixtureTypeProperty, value); }
        }
        #endregion



        #region 车间 WorkShop
        /// <summary>
        /// 车间Id
        /// </summary>
        public static readonly IRefIdProperty WorkShopIdProperty = P<FixtureQueryViewModelCriteria>.RegisterRefId(e => e.WorkShopId, ReferenceType.Normal);

        /// <summary>
        /// 车间Id
        /// </summary>
        public double? WorkShopId
        {
            get { return (double?)GetRefNullableId(WorkShopIdProperty); }
            set { SetRefNullableId(WorkShopIdProperty, value); }
        }

        /// <summary>
        /// 车间
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> WorkShopProperty = P<FixtureQueryViewModelCriteria>.RegisterRef(e => e.WorkShop, WorkShopIdProperty);

        /// <summary>
        /// 车间
        /// </summary>
        public Enterprise WorkShop
        {
            get { return GetRefEntity(WorkShopProperty); }
            set { SetRefEntity(WorkShopProperty, value); }
        }
        #endregion

        #region 产线 Resource
        /// <summary>
        /// 产线Id
        /// </summary>
        public static readonly IRefIdProperty ResourceIdProperty = P<FixtureQueryViewModelCriteria>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

        /// <summary>
        /// 产线Id
        /// </summary>
        public double? ResourceId
        {
            get { return (double?)GetRefNullableId(ResourceIdProperty); }
            set { SetRefNullableId(ResourceIdProperty, value); }
        }

        /// <summary>
        /// 产线
        /// </summary>
        public static readonly RefEntityProperty<WipResource> ResourceProperty = P<FixtureQueryViewModelCriteria>.RegisterRef(e => e.Resource, ResourceIdProperty);

        /// <summary>
        /// 产线
        /// </summary>
        public WipResource Resource
        {
            get { return GetRefEntity(ResourceProperty); }
            set { SetRefEntity(ResourceProperty, value); }
        }
        #endregion

        #region 工单 WorkOrder
        /// <summary>
        /// 工单Id
        /// </summary>
        public static readonly IRefIdProperty WorkOrderIdProperty = P<FixtureQueryViewModelCriteria>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

        /// <summary>
        /// 工单Id
        /// </summary>
        public double? WorkOrderId
        {
            get { return (double?)GetRefNullableId(WorkOrderIdProperty); }
            set { SetRefNullableId(WorkOrderIdProperty, value); }
        }

        /// <summary>
        /// 工单
        /// </summary>
        public static readonly RefEntityProperty<WorkOrder> WorkOrderProperty = P<FixtureQueryViewModelCriteria>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 工单
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return GetRefEntity(WorkOrderProperty); }
            set { SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion

        #region 产品 Product
        /// <summary>
        /// 产品Id
        /// </summary>
        public static readonly IRefIdProperty ProductIdProperty = P<FixtureQueryViewModelCriteria>.RegisterRefId(e => e.ProductId, ReferenceType.Normal);

        /// <summary>
        /// 产品Id
        /// </summary>
        public double? ProductId
        {
            get { return (double?)GetRefNullableId(ProductIdProperty); }
            set { SetRefNullableId(ProductIdProperty, value); }
        }

        /// <summary>
        /// 产品
        /// </summary>
        public static readonly RefEntityProperty<Item> ProductProperty = P<FixtureQueryViewModelCriteria>.RegisterRef(e => e.Product, ProductIdProperty);

        /// <summary>
        /// 产品
        /// </summary>
        public Item Product
        {
            get { return GetRefEntity(ProductProperty); }
            set { SetRefEntity(ProductProperty, value); }
        }
        #endregion

        #region 工艺面 Deck
        /// <summary>
        /// 工艺面
        /// </summary>
        [Label("工艺面")]
        public static readonly Property<Deck?> DeckProperty = P<FixtureQueryViewModelCriteria>.Register(e => e.Deck);

        /// <summary>
        /// 工艺面
        /// </summary>
        public Deck? Deck
        {
            get { return GetProperty(DeckProperty); }
            set { SetProperty(DeckProperty, value); }
        }
        #endregion

        #region 位置状态 RepairBeforeState
        /// <summary>
        /// 位置状态
        /// </summary>
        [Label("位置状态")]
        public static readonly Property<RepairBeforeState?> RepairBeforeStateProperty = P<FixtureQueryViewModelCriteria>.Register(e => e.RepairBeforeState);

        /// <summary>
        /// 位置状态
        /// </summary>
        public RepairBeforeState? RepairBeforeState
        {
            get { return GetProperty(RepairBeforeStateProperty); }
            set { SetProperty(RepairBeforeStateProperty, value); }
        }
        #endregion

        #region 工治具治具型号 FixtureModel
        /// <summary>
        /// 工治具治具型号Id
        /// </summary>
        [Label("型号编码")]
        [Required]
        public static readonly IRefIdProperty FixtureModelIdProperty = P<FixtureQueryViewModelCriteria>.RegisterRefId(e => e.FixtureModelId, ReferenceType.Normal);

        /// <summary>
        /// 工治具治具型号Id
        /// </summary>
        public double? FixtureModelId
        {
            get { return (double?)GetRefNullableId(FixtureModelIdProperty); }
            set { SetRefNullableId(FixtureModelIdProperty, value); }
        }

        /// <summary>
        /// 工治具治具型号
        /// </summary>
        public static readonly RefEntityProperty<FixtureModel> FixtureModelProperty = P<FixtureQueryViewModelCriteria>.RegisterRef(e => e.FixtureModel, FixtureModelIdProperty);

        /// <summary>
        /// 工治具治具型号
        /// </summary>
        public FixtureModel FixtureModel
        {
            get { return GetRefEntity(FixtureModelProperty); }
            set { SetRefEntity(FixtureModelProperty, value); }
        }
        #endregion

        #region 工治具治具编码 FixtureEncode
        /// <summary>
        /// 工治具治具编码Id
        /// </summary>
        public static readonly IRefIdProperty FixtureEncodeIdProperty = P<FixtureQueryViewModelCriteria>.RegisterRefId(e => e.FixtureEncodeId, ReferenceType.Normal);

        /// <summary>
        /// 工治具治具编码Id
        /// </summary>
        public double? FixtureEncodeId
        {
            get { return (double?)GetRefNullableId(FixtureEncodeIdProperty); }
            set { SetRefNullableId(FixtureEncodeIdProperty, value); }
        }

        /// <summary>
        /// 工治具治具编码
        /// </summary>
        public static readonly RefEntityProperty<FixtureEncode> FixtureEncodeProperty = P<FixtureQueryViewModelCriteria>.RegisterRef(e => e.FixtureEncode, FixtureEncodeIdProperty);

        /// <summary>
        /// 工治具治具编码
        /// </summary>
        public FixtureEncode FixtureEncode
        {
            get { return GetRefEntity(FixtureEncodeProperty); }
            set { SetRefEntity(FixtureEncodeProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 产线编码 ResourceCode
        /// <summary>
        /// 产线编码
        /// </summary>
        [Label("产线编码")]
        public static readonly Property<string> ResourceCodeProperty = P<FixtureQueryViewModelCriteria>.RegisterView(e => e.ResourceCode, p => p.Resource.Code);

        /// <summary>
        /// 产线编码
        /// </summary>
        public string ResourceCode
        {
            get { return this.GetProperty(ResourceCodeProperty); }
        }
        #endregion

        #region 工单号 WorkOrderNo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WorkOrderNoProperty = P<FixtureQueryViewModelCriteria>.RegisterView(e => e.WorkOrderNo, p => p.WorkOrder.No);

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo
        {
            get { return this.GetProperty(WorkOrderNoProperty); }
        }
        #endregion

        #region 产品编码 WorkOrderProductCode
        /// <summary>
        /// 产品编码
        /// </summary>
        [Label("产品编码")]
        public static readonly Property<string> WorkOrderProductCodeProperty = P<FixtureQueryViewModelCriteria>.RegisterView(e => e.WorkOrderProductCode, p => p.Product.Code);

        /// <summary>
        /// 产品编码
        /// </summary>
        public string WorkOrderProductCode
        {
            get { return this.GetProperty(WorkOrderProductCodeProperty); }
        }
        #endregion

        #region 产品名称 WorkOrderProductName
        /// <summary>
        /// 产品名称
        /// </summary>
        [Label("产品名称")]
        public static readonly Property<string> WorkOrderProductNameProperty = P<FixtureQueryViewModelCriteria>.RegisterView(e => e.WorkOrderProductName, p => p.Product.Name);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string WorkOrderProductName
        {
            get { return this.GetProperty(WorkOrderProductNameProperty); }
            set { this.SetProperty(WorkOrderProductNameProperty, value); }
        }
        #endregion

        #endregion

        /// <summary>
        /// 获取工治具查询列表
        /// </summary>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<ElecFixtureController>().GetFixtureQueryVMs(this);
        }
    }
}
