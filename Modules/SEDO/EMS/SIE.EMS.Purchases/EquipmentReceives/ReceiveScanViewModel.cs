using SIE.Domain;
using SIE.Equipments.Enums;
using SIE.Equipments.EquipAccounts;
using SIE.Equipments.EquipModels;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Purchases.EquipmentReceives
{
    /// <summary>
    /// 设备接收模型
    /// </summary>
    [RootEntity, Serializable]
    [Label("设备接收模型")]
    public class ReceiveScanViewModel : ViewModel
    {
        #region 接收id EquipmentReceiveId
        /// <summary>
        /// 接收id
        /// </summary>
        [Label("接收id")]
        public static readonly Property<double> EquipmentReceiveIdProperty = P<ReceiveScanViewModel>.Register(e => e.EquipmentReceiveId);

        /// <summary>
        /// 接收id
        /// </summary>
        public double EquipmentReceiveId
        {
            get { return this.GetProperty(EquipmentReceiveIdProperty); }
            set { this.SetProperty(EquipmentReceiveIdProperty, value); }
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

        #region 接收明细 EquipmentReceiveDetail
        /// <summary>
        /// 接收明细Id
        /// </summary>
        [Label("接收明细")]
        public static readonly IRefIdProperty EquipmentReceiveDetailIdProperty =
            P<ReceiveScanViewModel>.RegisterRefId(e => e.EquipmentReceiveDetailId, ReferenceType.Normal);

        /// <summary>
        /// 接收明细Id
        /// </summary>
        public double EquipmentReceiveDetailId
        {
            get { return (double)this.GetRefId(EquipmentReceiveDetailIdProperty); }
            set { this.SetRefId(EquipmentReceiveDetailIdProperty, value); }
        }

        /// <summary>
        /// 接收明细
        /// </summary>
        public static readonly RefEntityProperty<EquipmentReceiveDetail> EquipmentReceiveDetailProperty =
            P<ReceiveScanViewModel>.RegisterRef(e => e.EquipmentReceiveDetail, EquipmentReceiveDetailIdProperty);

        /// <summary>
        /// 接收明细
        /// </summary>
        public EquipmentReceiveDetail EquipmentReceiveDetail
        {
            get { return this.GetRefEntity(EquipmentReceiveDetailProperty); }
            set { this.SetRefEntity(EquipmentReceiveDetailProperty, value); }
        }
        #endregion

        #region 设备型号维护 EquipModel
        /// <summary>
        /// 设备型号维护Id
        /// </summary>
        public static readonly IRefIdProperty EquipModelIdProperty = P<ReceiveScanViewModel>.RegisterRefId(e => e.EquipModelId, ReferenceType.Normal);

        /// <summary>
        /// 设备型号维护Id
        /// </summary>
        public double EquipModelId
        {
            get { return (double)GetRefId(EquipModelIdProperty); }
            set { SetRefId(EquipModelIdProperty, value); }
        }

        /// <summary>
        /// 设备型号维护
        /// </summary>
        public static readonly RefEntityProperty<EquipModel> EquipModelProperty = P<ReceiveScanViewModel>.RegisterRef(e => e.EquipModel, EquipModelIdProperty);

        /// <summary>
        /// 设备型号维护
        /// </summary>
        public EquipModel EquipModel
        {
            get { return GetRefEntity(EquipModelProperty); }
            set { SetRefEntity(EquipModelProperty, value); }
        }
        #endregion

        #region 型号名称 EquipModelName
        /// <summary>
        /// 型号名称
        /// </summary>
        [Label("型号名称")]
        public static readonly Property<string> EquipModelNameProperty = P<ReceiveScanViewModel>.Register(e => e.EquipModelName);

        /// <summary>
        /// 型号名称
        /// </summary>
        public string EquipModelName
        {
            get { return this.GetProperty(EquipModelNameProperty); }
            set { this.SetProperty(EquipModelNameProperty, value); }
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

        #region 接收单行号 ReceiveLineNo
        /// <summary>
        /// 接收单行号
        /// </summary>
        [Label("接收单行号")]
        public static readonly Property<int> ReceiveLineNoProperty = P<ReceiveScanViewModel>.Register(e => e.ReceiveLineNo);

        /// <summary>
        /// 接收单行号
        /// </summary>
        public int ReceiveLineNo
        {
            get { return this.GetProperty(ReceiveLineNoProperty); }
            set { this.SetProperty(ReceiveLineNoProperty, value); }
        }
        #endregion

        #region 原已接收数量 OldRecivedQty
        /// <summary>
        /// 原已接收数量
        /// </summary>
        [Label("原已接收数量")]
        public static readonly Property<int> OldRecivedQtyProperty = P<ReceiveScanViewModel>.Register(e => e.OldRecivedQty);

        /// <summary>
        /// 原已接收数量
        /// </summary>
        public int OldRecivedQty
        {
            get { return this.GetProperty(OldRecivedQtyProperty); }
            set { this.SetProperty(OldRecivedQtyProperty, value); }
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

        #region 设备 EquipAccount
        /// <summary>
        /// 设备Id
        /// </summary>
        [Label("设备编码")]
        public static readonly IRefIdProperty EquipAccountIdProperty =
            P<ReceiveScanViewModel>.RegisterRefId(e => e.EquipAccountId, ReferenceType.Normal);

        /// <summary>
        /// 设备Id
        /// </summary>
        public double? EquipAccountId
        {
            get { return (double?)this.GetRefNullableId(EquipAccountIdProperty); }
            set { this.SetRefNullableId(EquipAccountIdProperty, value); }
        }

        /// <summary>
        /// 设备
        /// </summary>
        public static readonly RefEntityProperty<EquipAccountSelect> EquipAccountProperty =
            P<ReceiveScanViewModel>.RegisterRef(e => e.EquipAccount, EquipAccountIdProperty);

        /// <summary>
        /// 设备
        /// </summary>
        public EquipAccountSelect EquipAccount
        {
            get { return this.GetRefEntity(EquipAccountProperty); }
            set { this.SetRefEntity(EquipAccountProperty, value); }
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

        #region 扫描设备编码 ScanEquip
        /// <summary>
        /// 扫描设备编码
        /// </summary>
        [Label("扫描设备编码")]
        public static readonly Property<bool> ScanEquipProperty = P<ReceiveScanViewModel>.Register(e => e.ScanEquip);

        /// <summary>
        /// 扫描设备编码
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

        #region 扫描设备编码+原厂序列号 ScanEquipAndSn
        /// <summary>
        /// 扫描设备编码+原厂序列号
        /// </summary>
        [Label("扫描设备编码+原厂序列号")]
        public static readonly Property<bool> ScanEquipAndSnProperty = P<ReceiveScanViewModel>.Register(e => e.ScanEquipAndSn);

        /// <summary>
        /// 扫描设备编码+原厂序列号
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
