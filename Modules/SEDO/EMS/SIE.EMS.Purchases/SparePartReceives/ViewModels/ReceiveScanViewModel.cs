using SIE.Domain;
using SIE.EMS.SpareParts;
using SIE.EMS.SpareParts.Enums;
using SIE.Equipments.Enums;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Purchases.SparePartReceives.ViewModels
{
    /// <summary>
    /// 备件接收模型
    /// </summary>
    [RootEntity, Serializable]
    [Label("备件接收模型")]
    public class ReceiveScanViewModel : ViewModel
    {
        #region 接收id SparePartReceiveId
        /// <summary>
        /// 接收id
        /// </summary>
        [Label("接收id")]
        public static readonly Property<double> SparePartReceiveIdProperty = P<ReceiveScanViewModel>.Register(e => e.SparePartReceiveId);

        /// <summary>
        /// 接收id
        /// </summary>
        public double SparePartReceiveId
        {
            get { return this.GetProperty(SparePartReceiveIdProperty); }
            set { this.SetProperty(SparePartReceiveIdProperty, value); }
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

        #region 备件明细行 SparePartReceiveDetail
        /// <summary>
        /// 备件明细行Id
        /// </summary>
        [Label("备件明细行")]
        public static readonly IRefIdProperty SparePartReceiveDetailIdProperty =
            P<ReceiveScanViewModel>.RegisterRefId(e => e.SparePartReceiveDetailId, ReferenceType.Normal);

        /// <summary>
        /// 备件明细行Id
        /// </summary>
        public double SparePartReceiveDetailId
        {
            get { return (double)this.GetRefId(SparePartReceiveDetailIdProperty); }
            set { this.SetRefId(SparePartReceiveDetailIdProperty, value); }
        }

        /// <summary>
        /// 备件明细行
        /// </summary>
        public static readonly RefEntityProperty<SparePartReceiveDetail> SparePartReceiveDetailProperty =
            P<ReceiveScanViewModel>.RegisterRef(e => e.SparePartReceiveDetail, SparePartReceiveDetailIdProperty);

        /// <summary>
        /// 备件明细行
        /// </summary>
        public SparePartReceiveDetail SparePartReceiveDetail
        {
            get { return this.GetRefEntity(SparePartReceiveDetailProperty); }
            set { this.SetRefEntity(SparePartReceiveDetailProperty, value); }
        }
        #endregion

        #region 备件基础数据 SparePart
        /// <summary>
        /// 备件基础数据Id
        /// </summary>
        [Label("备件基础数据")]
        public static readonly IRefIdProperty SparePartIdProperty = P<ReceiveScanViewModel>.RegisterRefId(e => e.SparePartId, ReferenceType.Normal);

        /// <summary>
        /// 备件基础数据Id
        /// </summary>
        public double SparePartId
        {
            get { return (double)GetRefId(SparePartIdProperty); }
            set { SetRefId(SparePartIdProperty, value); }
        }

        /// <summary>
        /// 备件基础数据
        /// </summary>
        public static readonly RefEntityProperty<SparePart> SparePartProperty = P<ReceiveScanViewModel>.RegisterRef(e => e.SparePart, SparePartIdProperty);

        /// <summary>
        /// 备件基础数据
        /// </summary>
        public SparePart SparePart
        {
            get { return GetRefEntity(SparePartProperty); }
            set { SetRefEntity(SparePartProperty, value); }
        }
        #endregion

        #region 备件名称 SparePartName
        /// <summary>
        /// 备件名称
        /// </summary>
        [Label("备件名称")]
        public static readonly Property<string> SparePartNameProperty = P<ReceiveScanViewModel>.Register(e => e.SparePartName);

        /// <summary>
        /// 备件名称
        /// </summary>
        public string SparePartName
        {
            get { return this.GetProperty(SparePartNameProperty); }
            set { this.SetProperty(SparePartNameProperty, value); }
        }
        #endregion

        #region 采购订单行号 PurchaseOrderLineNo
        /// <summary>
        /// 采购订单行号
        /// </summary>
        [Label("采购订单")]
        public static readonly Property<string> PurchaseOrderLineNoProperty = P<ReceiveScanViewModel>.Register(e => e.PurchaseOrderLineNo);

        /// <summary>
        /// 采购订单行号
        /// </summary>
        public string PurchaseOrderLineNo
        {
            get { return this.GetProperty(PurchaseOrderLineNoProperty); }
            set { this.SetProperty(PurchaseOrderLineNoProperty, value); }
        }
        #endregion

        #region 管控方式 ControlMethod
        /// <summary>
        /// 管控方式
        /// </summary>
        [Label("管控方式")]
        public static readonly Property<ControlMethod?> ControlMethodProperty = P<ReceiveScanViewModel>.Register(e => e.ControlMethod);

        /// <summary>
        /// 管控方式
        /// </summary>
        public ControlMethod? ControlMethod
        {
            get { return this.GetProperty(ControlMethodProperty); }
            set { this.SetProperty(ControlMethodProperty, value); }
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

        #region 批次个数 LotCount
        /// <summary>
        /// 批次个数
        /// </summary>
        [Label("批次个数")]
        public static readonly Property<int> LotCountProperty = P<ReceiveScanViewModel>.Register(e => e.LotCount);

        /// <summary>
        /// 批次个数
        /// </summary>
        public int LotCount
        {
            get { return this.GetProperty(LotCountProperty); }
            set { this.SetProperty(LotCountProperty, value); }
        }
        #endregion

        #region 批次数量 LotQty
        /// <summary>
        /// 批次数量
        /// </summary>
        [Label("批次数量")]
        public static readonly Property<int> LotQtyProperty = P<ReceiveScanViewModel>.Register(e => e.LotQty);

        /// <summary>
        /// 批次数量
        /// </summary>
        public int LotQty
        {
            get { return this.GetProperty(LotQtyProperty); }
            set { this.SetProperty(LotQtyProperty, value); }
        }
        #endregion

        #region 序列号编码 StoreSummaryDetail
        /// <summary>
        /// 序列号编码Id
        /// </summary>
        [Label("序列号编码")]
        public static readonly IRefIdProperty StoreSummaryDetailIdProperty =
            P<ReceiveScanViewModel>.RegisterRefId(e => e.StoreSummaryDetailId, ReferenceType.Normal);

        /// <summary>
        /// 序列号编码Id
        /// </summary>
        public double? StoreSummaryDetailId
        {
            get { return (double?)this.GetRefNullableId(StoreSummaryDetailIdProperty); }
            set { this.SetRefNullableId(StoreSummaryDetailIdProperty, value); }
        }

        /// <summary>
        /// 序列号编码
        /// </summary>
        public static readonly RefEntityProperty<StoreSummaryDetail> StoreSummaryDetailProperty =
            P<ReceiveScanViewModel>.RegisterRef(e => e.StoreSummaryDetail, StoreSummaryDetailIdProperty);

        /// <summary>
        /// 序列号编码
        /// </summary>
        public StoreSummaryDetail StoreSummaryDetail
        {
            get { return this.GetRefEntity(StoreSummaryDetailProperty); }
            set { this.SetRefEntity(StoreSummaryDetailProperty, value); }
        }
        #endregion

        #region 备件出库单行id PartOutDepotDetailId
        /// <summary>
        /// 备件出库单行id
        /// </summary>
        [Label("备件出库单行id")]
        public static readonly Property<double?> PartOutDepotDetailIdProperty = P<ReceiveScanViewModel>.Register(e => e.PartOutDepotDetailId);

        /// <summary>
        /// 备件出库单行id
        /// </summary>
        public double? PartOutDepotDetailId
        {
            get { return this.GetProperty(PartOutDepotDetailIdProperty); }
            set { this.SetProperty(PartOutDepotDetailIdProperty, value); }
        }
        #endregion

        #region 备件出库单 PartOutDepotDetailLineNo
        /// <summary>
        /// 备件出库单
        /// </summary>
        [Label("备件出库单")]
        public static readonly Property<string> PartOutDepotDetailLineNoProperty = P<ReceiveScanViewModel>.Register(e => e.PartOutDepotDetailLineNo);

        /// <summary>
        /// 备件出库单
        /// </summary>
        public string PartOutDepotDetailLineNo
        {
            get { return this.GetProperty(PartOutDepotDetailLineNoProperty); }
            set { this.SetProperty(PartOutDepotDetailLineNoProperty, value); }
        }
        #endregion

        #region 备件出库单 SnPartOutDepotDetailLineNo
        /// <summary>
        /// 备件出库单
        /// </summary>
        [Label("备件出库单")]
        public static readonly Property<string> SnPartOutDepotDetailLineNoProperty = P<ReceiveScanViewModel>.Register(e => e.SnPartOutDepotDetailLineNo);

        /// <summary>
        /// 备件出库单
        /// </summary>
        public string SnPartOutDepotDetailLineNo
        {
            get { return this.GetProperty(SnPartOutDepotDetailLineNoProperty); }
            set { this.SetProperty(SnPartOutDepotDetailLineNoProperty, value); }
        }
        #endregion

        #region 单位 UnitName
        /// <summary>
        /// 单位
        /// </summary>
        [Label("单位")]
        public static readonly Property<string> UnitNameProperty = P<ReceiveScanViewModel>.Register(e => e.UnitName);

        /// <summary>
        /// 单位
        /// </summary>
        public string UnitName
        {
            get { return this.GetProperty(UnitNameProperty); }
            set { this.SetProperty(UnitNameProperty, value); }
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

        #region 批次扫描框 LotSn
        /// <summary>
        /// 批次扫描框
        /// </summary>
        [Label("批次扫描框")]
        public static readonly Property<string> LotSnProperty = P<ReceiveScanViewModel>.Register(e => e.LotSn);

        /// <summary>
        /// 批次扫描框
        /// </summary>
        public string LotSn
        {
            get { return this.GetProperty(LotSnProperty); }
            set { this.SetProperty(LotSnProperty, value); }
        }
        #endregion

        #region 固定批次数量 FixedQty
        /// <summary>
        /// 固定批次数量
        /// </summary>
        [Label("固定批次数量")]
        public static readonly Property<bool> FixedQtyProperty = P<ReceiveScanViewModel>.Register(e => e.FixedQty);

        /// <summary>
        /// 固定批次数量
        /// </summary>
        public bool FixedQty
        {
            get { return this.GetProperty(FixedQtyProperty); }
            set { this.SetProperty(FixedQtyProperty, value); }
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

        #region 扫描序列号编码 ScanEquip
        /// <summary>
        /// 扫描序列号编码
        /// </summary>
        [Label("扫描序列号编码")]
        public static readonly Property<bool> ScanEquipProperty = P<ReceiveScanViewModel>.Register(e => e.ScanEquip);

        /// <summary>
        /// 扫描序列号编码
        /// </summary>
        public bool ScanEquip
        {
            get { return this.GetProperty(ScanEquipProperty); }
            set { this.SetProperty(ScanEquipProperty, value); }
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

        #region 扫描序列号编码+原厂序列号 ScanEquipAndSn
        /// <summary>
        /// 扫描序列号编码+原厂序列号
        /// </summary>
        [Label("扫描序列号编码+原厂序列号")]
        public static readonly Property<bool> ScanEquipAndSnProperty = P<ReceiveScanViewModel>.Register(e => e.ScanEquipAndSn);

        /// <summary>
        /// 扫描序列号编码+原厂序列号
        /// </summary>
        public bool ScanEquipAndSn
        {
            get { return this.GetProperty(ScanEquipAndSnProperty); }
            set { this.SetProperty(ScanEquipAndSnProperty, value); }
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
        #endregion
    }
}
