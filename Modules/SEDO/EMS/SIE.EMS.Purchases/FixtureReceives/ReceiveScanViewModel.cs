using SIE.Domain;
using SIE.EMS.Purchases.FixtureReceives;
using SIE.Equipments.Enums;
using SIE.Fixtures;
using SIE.Fixtures.Enums;
using SIE.Fixtures.Fixtures.Accounts;
using SIE.Fixtures.Models;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Purchases.FixtureReceives
{
    /// <summary>
    /// 工治具编码接收模型
    /// </summary>
    [RootEntity, Serializable]
    [Label("工治具编码接收模型")]
    public class ReceiveScanViewModel : ViewModel
    {
        #region 接收id FixtureReceiveId
        /// <summary>
        /// 接收id
        /// </summary>
        [Label("接收id")]
        public static readonly Property<double> FixtureReceiveIdProperty = P<ReceiveScanViewModel>.Register(e => e.FixtureReceiveId);

        /// <summary>
        /// 接收id
        /// </summary>
        public double FixtureReceiveId
        {
            get { return this.GetProperty(FixtureReceiveIdProperty); }
            set { this.SetProperty(FixtureReceiveIdProperty, value); }
        }
        #endregion

        #region 接收单号 ReceiveNo
        /// <summary>
        /// 接收单号
        /// </summary>
        [Label("接收单号")]
        public static readonly Property<string> ReceiveNoProperty = P<ReceiveScanViewModel>.Register(e => e.ReceiveNo);

        /// <summary>
        /// 接收单号
        /// </summary>
        public string ReceiveNo
        {
            get { return GetProperty(ReceiveNoProperty); }
            set { SetProperty(ReceiveNoProperty, value); }
        }
        #endregion

        #region 工厂 FactoryName
        /// <summary>
        /// 工厂
        /// </summary>
        [Label("工厂")]
        public static readonly Property<string> FactoryNameProperty = P<ReceiveScanViewModel>.Register(e => e.FactoryName);

        /// <summary>
        /// 工厂
        /// </summary>
        public string FactoryName
        {
            get { return this.GetProperty(FactoryNameProperty); }
            set { this.SetProperty(FactoryNameProperty, value); }
        }
        #endregion

        #region 部门 DepartmentName
        /// <summary>
        /// 部门
        /// </summary>
        [Label("部门")]
        public static readonly Property<string> DepartmentNameProperty = P<ReceiveScanViewModel>.Register(e => e.DepartmentName);

        /// <summary>
        /// 部门
        /// </summary>
        public string DepartmentName
        {
            get { return this.GetProperty(DepartmentNameProperty); }
            set { this.SetProperty(DepartmentNameProperty, value); }
        }
        #endregion

        #region 接收类型 ReceiveType
        /// <summary>
        /// 接收类型
        /// </summary>
        [Label("接收类型")]
        public static readonly Property<ReceiveType> ReceiveTypeProperty = P<ReceiveScanViewModel>.Register(e => e.ReceiveType);

        /// <summary>
        /// 接收类型
        /// </summary>
        public ReceiveType ReceiveType
        {
            get { return GetProperty(ReceiveTypeProperty); }
            set { SetProperty(ReceiveTypeProperty, value); }
        }
        #endregion

        #region 工治具明细行 FixtureReceiveDetail
        /// <summary>
        /// 接收明细Id
        /// </summary>
        [Label("工治具明细行")]
        public static readonly IRefIdProperty FixtureReceiveDetailIdProperty =
            P<ReceiveScanViewModel>.RegisterRefId(e => e.FixtureReceiveDetailId, ReferenceType.Normal);

        /// <summary>
        /// 接收明细Id
        /// </summary>
        public double FixtureReceiveDetailId
        {
            get { return (double)this.GetRefId(FixtureReceiveDetailIdProperty); }
            set { this.SetRefId(FixtureReceiveDetailIdProperty, value); }
        }

        /// <summary>
        /// 接收明细
        /// </summary>
        public static readonly RefEntityProperty<FixtureReceiveDetail> FixtureReceiveDetailProperty =
            P<ReceiveScanViewModel>.RegisterRef(e => e.FixtureReceiveDetail, FixtureReceiveDetailIdProperty);

        /// <summary>
        /// 接收明细
        /// </summary>
        public FixtureReceiveDetail FixtureReceiveDetail
        {
            get { return this.GetRefEntity(FixtureReceiveDetailProperty); }
            set { this.SetRefEntity(FixtureReceiveDetailProperty, value); }
        }
        #endregion

        #region 接收数量 Qty
        /// <summary>
        /// 接收数量
        /// </summary>
        [Label("接收数量")]
        public static readonly Property<int> QtyProperty = P<ReceiveScanViewModel>.Register(e => e.Qty);

        /// <summary>
        /// 接收数量
        /// </summary>
        public int Qty
        {
            get { return GetProperty(QtyProperty); }
            set { SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 已接收数量 RecivedQty
        /// <summary>
        /// 已接收数量
        /// </summary>
        [Label("已接收数量")]
        public static readonly Property<int> RecivedQtyProperty = P<ReceiveScanViewModel>.Register(e => e.RecivedQty);

        /// <summary>
        /// 已接收数量
        /// </summary>
        public int RecivedQty
        {
            get { return GetProperty(RecivedQtyProperty); }
            set { SetProperty(RecivedQtyProperty, value); }
        }
        #endregion

        #region 本次接收数量 CurrentQty
        /// <summary>
        /// 本次接收数量
        /// </summary>
        [Label("本次接收数量")]
        public static readonly Property<int> CurrentQtyProperty = P<ReceiveScanViewModel>.Register(e => e.CurrentQty);

        /// <summary>
        /// 本次接收数量
        /// </summary>
        public int CurrentQty
        {
            get { return this.GetProperty(CurrentQtyProperty); }
            set { this.SetProperty(CurrentQtyProperty, value); }
        }
        #endregion

        #region 工治具编码 FixtureEncode
        /// <summary>
        /// 工治具编码Id
        /// </summary>
        [Label("工治具编码")]
        public static readonly IRefIdProperty FixtureEncodeIdProperty =
            P<ReceiveScanViewModel>.RegisterRefId(e => e.FixtureEncodeId, ReferenceType.Normal);

        /// <summary>
        /// 工治具编码Id
        /// </summary>
        public double? FixtureEncodeId
        {
            get { return (double?)this.GetRefNullableId(FixtureEncodeIdProperty); }
            set { this.SetRefNullableId(FixtureEncodeIdProperty, value); }
        }

        /// <summary>
        /// 工治具编码
        /// </summary>
        public static readonly RefEntityProperty<FixtureEncode> FixtureEncodeProperty =
            P<ReceiveScanViewModel>.RegisterRef(e => e.FixtureEncode, FixtureEncodeIdProperty);

        /// <summary>
        /// 工治具编码
        /// </summary>
        public FixtureEncode FixtureEncode
        {
            get { return this.GetRefEntity(FixtureEncodeProperty); }
            set { this.SetRefEntity(FixtureEncodeProperty, value); }
        }
        #endregion

        #region 消息框 Message
        /// <summary>
        /// 消息框
        /// </summary>
        [Label("消息框")]
        public static readonly Property<string> MessageProperty = P<ReceiveScanViewModel>.Register(e => e.Message);

        /// <summary>
        /// 消息框
        /// </summary>
        public string Message
        {
            get { return this.GetProperty(MessageProperty); }
            set { this.SetProperty(MessageProperty, value); }
        }
        #endregion

        #region 扫描框 Sn
        /// <summary>
        /// 扫描框
        /// </summary>
        [Label("扫描框")]
        public static readonly Property<string> SnProperty = P<ReceiveScanViewModel>.Register(e => e.Sn);

        /// <summary>
        /// 扫描框
        /// </summary>
        public string Sn
        {
            get { return this.GetProperty(SnProperty); }
            set { this.SetProperty(SnProperty, value); }
        }
        #endregion

        #region 扫描序列号编码 ScanSnCode
        /// <summary>
        /// 扫描序列号编码
        /// </summary>
        [Label("扫描序列号编码")]
        public static readonly Property<bool> ScanSnCodeProperty = P<ReceiveScanViewModel>.Register(e => e.ScanSnCode);

        /// <summary>
        /// 扫描序列号编码
        /// </summary>
        public bool ScanSnCode
        {
            get { return this.GetProperty(ScanSnCodeProperty); }
            set { this.SetProperty(ScanSnCodeProperty, value); }
        }
        #endregion

        #region 扫描原厂序列号 ScanSn
        /// <summary>
        /// 扫描原厂序列号
        /// </summary>
        [Label("扫描原厂序列号")]
        public static readonly Property<bool> ScanSnProperty = P<ReceiveScanViewModel>.Register(e => e.ScanSn);

        /// <summary>
        /// 扫描原厂序列号
        /// </summary>
        public bool ScanSn
        {
            get { return this.GetProperty(ScanSnProperty); }
            set { this.SetProperty(ScanSnProperty, value); }
        }
        #endregion

        #region 扫描工治具编码编码+原厂序列号 ScanSnCodeAndSn
        /// <summary>
        /// 扫描工治具编码编码+原厂序列号
        /// </summary>
        [Label("序列号编码+原厂序列号")]
        public static readonly Property<bool> ScanSnCodeAndSnProperty = P<ReceiveScanViewModel>.Register(e => e.ScanSnCodeAndSn);

        /// <summary>
        /// 扫描工治具编码+原厂序列号
        /// </summary>
        public bool ScanSnCodeAndSn
        {
            get { return this.GetProperty(ScanSnCodeAndSnProperty); }
            set { this.SetProperty(ScanSnCodeAndSnProperty, value); }
        }
        #endregion

        #region 扫描第一个条码 FirstSn
        /// <summary>
        /// 扫描第一个条码
        /// </summary>
        [Label("扫描第一个条码")]
        public static readonly Property<string> FirstSnProperty = P<ReceiveScanViewModel>.Register(e => e.FirstSn);

        /// <summary>
        /// 扫描第一个条码
        /// </summary>
        public string FirstSn
        {
            get { return this.GetProperty(FirstSnProperty); }
            set { this.SetProperty(FirstSnProperty, value); }
        }

        #region 型号编码 ModelCode
        /// <summary>
        /// 型号编码
        /// </summary>
        [Label("型号编码")]
        public static readonly Property<string> ModelCodeProperty = P<ReceiveScanViewModel>.RegisterView(e => e.ModelCode, p => p.FixtureEncode.FixtureModel.Code);

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
        /// 型号编码
        /// </summary>
        [Label("型号名称")]
        public static readonly Property<string> ModelNameProperty = P<ReceiveScanViewModel>.RegisterView(e => e.ModelName, p => p.FixtureEncode.FixtureModel.Name);

        /// <summary>
        /// 型号编码
        /// </summary>
        public string ModelName
        {
            get { return this.GetProperty(ModelNameProperty); }
        }
        #endregion


        #region 管控方式 ManageMode
        /// <summary>
        /// 管控方式
        /// </summary>
        [Label("管控方式")]
        public static readonly Property<ManageMode?> ManageModeProperty = P<ReceiveScanViewModel>.Register(e => e.ManageMode);

        /// <summary>
        /// 管控方式
        /// </summary>
        public ManageMode? ManageMode
        {
            get { return this.GetProperty(ManageModeProperty); }
            set { this.SetProperty(ManageModeProperty, value); }
        }
        #endregion


        #region 单位名称 UintName
        /// <summary>
        /// 单位名称
        /// </summary>
        [Label("单位")]
        public static readonly Property<string> UintNameProperty = P<ReceiveScanViewModel>.RegisterView(e => e.UintName, p => p.FixtureReceiveDetail.Unit.Name);

        /// <summary>
        /// 单位名称
        /// </summary>
        public string UintName
        {
            get { return this.GetProperty(UintNameProperty); }
        }
        #endregion

        #region 采购订单行号 PuOrderLineNo
        /// <summary>
        /// 采购订单行号
        /// </summary>
        [Label("采购订单行号")]
        public static readonly Property<string> PuOrderLineNoProperty = P<ReceiveScanViewModel>.Register(e => e.PuOrderLineNo);

        /// <summary>
        /// 采购订单行号
        /// </summary>
        public string PuOrderLineNo
        {
            get { return this.GetProperty(PuOrderLineNoProperty); }
            set { this.SetProperty(PuOrderLineNoProperty, value); }
        }
        #endregion

        #region 生产日期 ProductionDate
        /// <summary>
        /// 生产日期
        /// </summary>
        [Label("生产日期")]
        public static readonly Property<DateTime?> ProductionDateProperty = P<ReceiveScanViewModel>.Register(e => e.ProductionDate);

        /// <summary>
        /// 生产日期
        /// </summary>
        public DateTime? ProductionDate
        {
            get { return this.GetProperty(ProductionDateProperty); }
            set { this.SetProperty(ProductionDateProperty, value); }
        }
        #endregion


        #region 序列号编码 SnCode
        /// <summary>
        /// 序列号编码Id
        /// </summary>
        [Label("序列号编码")]
        public static readonly IRefIdProperty SnCodeIdProperty =
            P<ReceiveScanViewModel>.RegisterRefId(e => e.SnCodeId, ReferenceType.Normal);

        /// <summary>
        /// 序列号编码Id
        /// </summary>
        public double? SnCodeId
        {
            get { return (double?)this.GetRefNullableId(SnCodeIdProperty); }
            set { this.SetRefNullableId(SnCodeIdProperty, value); }
        }

        /// <summary>
        /// 序列号编码
        /// </summary>
        public static readonly RefEntityProperty<FixtureAccount> SnCodeProperty =
            P<ReceiveScanViewModel>.RegisterRef(e => e.SnCode, SnCodeIdProperty);

        /// <summary>
        /// 序列号编码
        /// </summary>
        public FixtureAccount SnCode
        {
            get { return this.GetRefEntity(SnCodeProperty); }
            set { this.SetRefEntity(SnCodeProperty, value); }
        }
        #endregion

        #region 生产厂家 Maker
        /// <summary>
        /// 生产厂家
        /// </summary>
        [Label("生产厂家")]
        public static readonly Property<string> MakerProperty = P<ReceiveScanViewModel>.Register(e => e.Maker);

        /// <summary>
        /// 生产厂家
        /// </summary>
        public string Maker
        {
            get { return this.GetProperty(MakerProperty); }
            set { this.SetProperty(MakerProperty, value); }
        }
        #endregion


        #endregion
    }
}