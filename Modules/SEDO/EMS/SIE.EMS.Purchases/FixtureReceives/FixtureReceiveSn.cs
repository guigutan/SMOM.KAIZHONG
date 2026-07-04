using SIE;
using SIE.Domain;
using SIE.Fixtures;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Purchases.FixtureReceives
{
    /// <summary>
    /// 工治具接收序列号明细
    /// </summary>
    [ChildEntity, Serializable]
    //[CriteriaQuery]
    [Label("工治具接收序列号明细")]
    public partial class FixtureReceiveSn : DataEntity
    {
        #region 序列号 Sn
        /// <summary>
        /// 序列号
        /// </summary>
        [Label("序列号")]
        public static readonly Property<string> SnProperty = P<FixtureReceiveSn>.Register(e => e.Sn);

        /// <summary>
        /// 序列号
        /// </summary>
        public string Sn
        {
            get { return GetProperty(SnProperty); }
            set { SetProperty(SnProperty, value); }
        }
        #endregion

        #region 原厂序列号 OriginalSn
        /// <summary>
        /// 原厂序列号
        /// </summary>
        [Label("原厂序列号")]
        public static readonly Property<string> OriginalSnProperty = P<FixtureReceiveSn>.Register(e => e.OriginalSn);

        /// <summary>
        /// 原厂序列号
        /// </summary>
        public string OriginalSn
        {
            get { return GetProperty(OriginalSnProperty); }
            set { SetProperty(OriginalSnProperty, value); }
        }
        #endregion

        #region 生产日期 ProductionDate
        /// <summary>
        /// 生产日期
        /// </summary>
        [Label("生产日期")]
        public static readonly Property<DateTime?> ProductionDateProperty = P<FixtureReceiveSn>.Register(e => e.ProductionDate);

        /// <summary>
        /// 生产日期
        /// </summary>
        public DateTime? ProductionDate
        {
            get { return GetProperty(ProductionDateProperty); }
            set { SetProperty(ProductionDateProperty, value); }
        }
        #endregion

        #region 厂商名称 Maker
        /// <summary>
        /// 厂商名称
        /// </summary>
        [Label("厂商名称")]
        public static readonly Property<string> MakerProperty = P<FixtureReceiveSn>.Register(e => e.Maker);

        /// <summary>
        /// 厂商名称
        /// </summary>
        public string Maker
        {
            get { return GetProperty(MakerProperty); }
            set { SetProperty(MakerProperty, value); }
        }
        #endregion

        #region 工治具接收明细 FixtureReceiveDetail
        /// <summary>
        /// 工治具接收明细Id
        /// </summary>
        public static readonly IRefIdProperty FixtureReceiveDetailIdProperty = P<FixtureReceiveSn>.RegisterRefId(e => e.FixtureReceiveDetailId, ReferenceType.Parent);

        /// <summary>
        /// 工治具接收明细Id
        /// </summary>
        public double FixtureReceiveDetailId
        {
            get { return (double)GetRefId(FixtureReceiveDetailIdProperty); }
            set { SetRefId(FixtureReceiveDetailIdProperty, value); }
        }

        /// <summary>
        /// 工治具接收明细
        /// </summary>
        public static readonly RefEntityProperty<FixtureReceiveDetail> FixtureReceiveDetailProperty = P<FixtureReceiveSn>.RegisterRef(e => e.FixtureReceiveDetail, FixtureReceiveDetailIdProperty);

        /// <summary>
        /// 工治具接收明细
        /// </summary>
        public FixtureReceiveDetail FixtureReceiveDetail
        {
            get { return GetRefEntity(FixtureReceiveDetailProperty); }
            set { SetRefEntity(FixtureReceiveDetailProperty, value); }
        }
        #endregion


        #region 是否已经创建过工治具台账 IsCreatedFixtureAccount
        /// <summary>
        /// 是否已经创建过工治具台账
        /// </summary>
        [Label("是否已经创建过工治具台账")]
        public static readonly Property<bool> IsCreatedFixtureAccountProperty = P<FixtureReceiveSn>.Register(e => e.IsCreatedFixtureAccount);

        /// <summary>
        /// 是否已经创建过工治具台账
        /// </summary>
        public bool IsCreatedFixtureAccount
        {
            get { return this.GetProperty(IsCreatedFixtureAccountProperty); }
            set { this.SetProperty(IsCreatedFixtureAccountProperty, value); }
        }
        #endregion


        #region 视图属性


        #region 工治具编码 FixtureEncodeCode
        /// <summary>
        /// 工治具编码
        /// </summary>
        [Label("工治具编码")]
        public static readonly Property<string> FixtureEncodeCodeProperty = P<FixtureReceiveSn>.RegisterView(e => e.FixtureEncodeCode, p => p.FixtureReceiveDetail.FixtureEncode.Code);

        /// <summary>
        /// 工治具编码
        /// </summary>
        public string FixtureEncodeCode
        {
            get { return this.GetProperty(FixtureEncodeCodeProperty); }
            set { SetProperty(FixtureEncodeCodeProperty, value); }
        }
        #endregion

        #region 供应商编码 SupplierCode
        /// <summary>
        /// 供应商编码
        /// </summary>
        [Label("供应商编码")]
        public static readonly Property<string> SupplierCodeProperty = P<FixtureReceiveSn>.RegisterView(e => e.SupplierCode, p => p.FixtureReceiveDetail.Supplier.Code);

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string SupplierCode
        {
            get { return this.GetProperty(SupplierCodeProperty); }
            set { SetProperty(SupplierCodeProperty, value); }
        }
        #endregion

        #region 供应商名称 SupplierName	
        /// <summary>
        /// 供应商名称
        /// </summary>
        [Label("供应商名称")]
        public static readonly Property<string> SupplierNameProperty = P<FixtureReceiveSn>.RegisterView(e => e.SupplierName, p => p.FixtureReceiveDetail.Supplier.Name);

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string SupplierName
        {
            get { return this.GetProperty(SupplierNameProperty); }
            set { SetProperty(SupplierNameProperty, value); }
        }
        #endregion

        #region 客户编码 CustomerCode
        /// <summary>
        /// 客户名称
        /// </summary>
        [Label("客户编码")]
        public static readonly Property<string> CustomerCodeProperty = P<FixtureReceiveSn>.RegisterView(e => e.CustomerCode, p => p.FixtureReceiveDetail.Customer.Code);

        /// <summary>
        /// 客户名称
        /// </summary>
        public string CustomerCode
        {
            get { return this.GetProperty(CustomerCodeProperty); }
            set { SetProperty(CustomerCodeProperty, value); }
        }
        #endregion

        #region 客户名称 CustomerName	
        /// <summary>
        /// 客户名称
        /// </summary>
        [Label("客户名称")]
        public static readonly Property<string> CustomerNameProperty = P<FixtureReceiveSn>.RegisterView(e => e.CustomerName, p => p.FixtureReceiveDetail.Customer.Name);

        /// <summary>
        /// 客户名称
        /// </summary>
        public string CustomerName
        {
            get { return this.GetProperty(CustomerNameProperty); }
            set { SetProperty(CustomerNameProperty, value); }
        }
        #endregion

        #region 采购单号 PurOrderNo
        /// <summary>
        /// 采购单号
        /// </summary>
        [Label("采购单号")]
        public static readonly Property<string> PurOrderNoProperty = P<FixtureReceiveSn>.RegisterView(e => e.PurOrderNo, p => p.FixtureReceiveDetail.PurchaseOrder.OrderNo);

        /// <summary>
        /// 采购单号
        /// </summary>
        public string PurOrderNo
        {
            get { return this.GetProperty(PurOrderNoProperty); }
            set { SetProperty(PurOrderNoProperty, value); }
        }
        #endregion



        #region 采购单行号 OrderLineNo
        /// <summary>
        /// 采购单行号
        /// </summary>
        [Label("采购单行号")]
        public static readonly Property<string> OrderLineNoProperty = P<FixtureReceiveSn>.RegisterView(e => e.OrderLineNo, p => p.FixtureReceiveDetail.PurchaseOrderItem.LineNo);

        /// <summary>
        /// 采购单行号
        /// </summary>
        public string OrderLineNo
        {
            get { return this.GetProperty(OrderLineNoProperty); }
            set { SetProperty(OrderLineNoProperty, value); }
        }
        #endregion



        #region 行号 LineNo
        /// <summary>
        /// 行号
        /// </summary>
        [Label("行号")]
        public static readonly Property<string> LineNoProperty = P<FixtureReceiveSn>.RegisterView(e => e.LineNo, p => p.FixtureReceiveDetail.LineNo);

        /// <summary>
        /// 行号
        /// </summary>
        public string LineNo
        {
            get { return this.GetProperty(LineNoProperty); }
            set { SetProperty(LineNoProperty, value); }
        }
        #endregion





        #region 型号编码 ModelCode
        /// <summary>
        /// 型号编码
        /// </summary>
        [Label("型号编码")]
        public static readonly Property<string> ModelCodeProperty = P<FixtureReceiveSn>.RegisterView(e => e.ModelCode, p => p.FixtureReceiveDetail.FixtureEncode.FixtureModel.Code);

        /// <summary>
        /// 型号编码
        /// </summary>
        public string ModelCode
        {
            get { return this.GetProperty(ModelCodeProperty); }
            set { SetProperty(ModelCodeProperty, value); }
        }
        #endregion


        #region 型号名称 ModelName
        /// <summary>
        /// 型号名称
        /// </summary>
        [Label("型号名称")]
        public static readonly Property<string> ModelNameProperty = P<FixtureReceiveSn>.RegisterView(e => e.ModelName, p => p.FixtureReceiveDetail.FixtureEncode.FixtureModel.Name);

        /// <summary>
        /// 型号编码
        /// </summary>
        public string ModelName
        {
            get { return this.GetProperty(ModelNameProperty); }
            set { SetProperty(ModelNameProperty, value); }
        }
        #endregion


        #region 管控模式 ManageMode
        /// <summary>
        /// 管理模式
        /// </summary>
        [Label("管控模式")]
        public static readonly Property<ManageMode> ManageModeProperty = P<FixtureReceiveSn>.RegisterView(e => e.ManageMode, p => p.FixtureReceiveDetail.FixtureEncode.FixtureModel.ManageMode);

        /// <summary>
        /// 
        /// </summary>
        public ManageMode ManageMode
        {
            get { return this.GetProperty(ManageModeProperty); }
            set { SetProperty(ManageModeProperty, value); }
        }
        #endregion

        #region 免检 ExemptionInspect
        /// <summary>
        /// 免检
        /// </summary>
        [Label("免检")]
        public static readonly Property<bool> ExemptionInspectProperty = P<FixtureReceiveSn>.RegisterView(e => e.ExemptionInspect, p => p.FixtureReceiveDetail.FixtureEncode.Exemption);

        /// <summary>
        /// 免检
        /// </summary>
        public bool ExemptionInspect
        {
            get { return this.GetProperty(ExemptionInspectProperty); }
            set { SetProperty(ExemptionInspectProperty, value); }
        }
        #endregion

        #endregion

    }

    /// <summary>
    /// 工治具接收序列号明细 实体配置
    /// </summary>
    internal class FixtureReceiveSnConfig : EntityConfig<FixtureReceiveSn>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_FIXT_RECV_SN").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}