using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Purchases.EquipmentReceives
{
    /// <summary>
    /// 设备接收序列号模型
    /// </summary>
    [RootEntity, Serializable]
    [Label("设备接收序列号模型")]
    public class ReceiveScanSnViewModel : ViewModel
    {
        #region 接收明细 EquipmentReceiveDetailId
        /// <summary>
        /// 接收明细
        /// </summary>
        [Label("接收明细")]
        public static readonly Property<double> EquipmentReceiveDetailIdProperty = P<ReceiveScanSnViewModel>.Register(e => e.EquipmentReceiveDetailId);

        /// <summary>
        /// 接收明细
        /// </summary>
        public double EquipmentReceiveDetailId
        {
            get { return this.GetProperty(EquipmentReceiveDetailIdProperty); }
            set { this.SetProperty(EquipmentReceiveDetailIdProperty, value); }
        }
        #endregion

        #region 接收行号 ReceiveLineNo
        /// <summary>
        /// 接收行号
        /// </summary>
        [Label("接收行号")]
        public static readonly Property<int> ReceiveLineNoProperty = P<ReceiveScanSnViewModel>.Register(e => e.ReceiveLineNo);

        /// <summary>
        /// 接收行号
        /// </summary>
        public int ReceiveLineNo
        {
            get { return this.GetProperty(ReceiveLineNoProperty); }
            set { this.SetProperty(ReceiveLineNoProperty, value); }
        }
        #endregion

        #region 采购单号 PurchaseOrderNo
        /// <summary>
        /// 采购单号
        /// </summary>
        [Label("采购单号")]
        public static readonly Property<string> PurchaseOrderNoProperty = P<ReceiveScanSnViewModel>.Register(e => e.PurchaseOrderNo);

        /// <summary>
        /// 采购单号
        /// </summary>
        public string PurchaseOrderNo
        {
            get { return this.GetProperty(PurchaseOrderNoProperty); }
            set { this.SetProperty(PurchaseOrderNoProperty, value); }
        }
        #endregion

        #region 采购单行号 OrderItemNo
        /// <summary>
        /// 采购单行号
        /// </summary>
        [Label("采购单行号")]
        public static readonly Property<int?> OrderItemNoProperty = P<ReceiveScanSnViewModel>.Register(e => e.OrderItemNo);

        /// <summary>
        /// 采购单行号
        /// </summary>
        public int? OrderItemNo
        {
            get { return this.GetProperty(OrderItemNoProperty); }
            set { this.SetProperty(OrderItemNoProperty, value); }
        }
        #endregion

        #region 设备型号编码 EquipModelCode
        /// <summary>
        /// 设备型号编码
        /// </summary>
        [Label("设备型号编码")]
        public static readonly Property<string> EquipModelCodeProperty = P<ReceiveScanSnViewModel>.Register(e => e.EquipModelCode);

        /// <summary>
        /// 设备型号编码
        /// </summary>
        public string EquipModelCode
        {
            get { return this.GetProperty(EquipModelCodeProperty); }
            set { this.SetProperty(EquipModelCodeProperty, value); }
        }
        #endregion

        #region 设备型号名称 EquipModelName
        /// <summary>
        /// 设备型号名称
        /// </summary>
        [Label("设备型号名称")]
        public static readonly Property<string> EquipModelNameProperty = P<ReceiveScanSnViewModel>.Register(e => e.EquipModelName);

        /// <summary>
        /// 设备型号名称
        /// </summary>
        public string EquipModelName
        {
            get { return this.GetProperty(EquipModelNameProperty); }
            set { this.SetProperty(EquipModelNameProperty, value); }
        }
        #endregion

        #region 技术规格 TechnicalNorm
        /// <summary>
        /// 技术规格
        /// </summary>
        [Label("技术规格")]
        public static readonly Property<string> TechnicalNormProperty = P<ReceiveScanSnViewModel>.Register(e => e.TechnicalNorm);

        /// <summary>
        /// 技术规格
        /// </summary>
        public string TechnicalNorm
        {
            get { return this.GetProperty(TechnicalNormProperty); }
            set { this.SetProperty(TechnicalNormProperty, value); }
        }
        #endregion

        #region 设备编码 EquipmentCode
        /// <summary>
        /// 设备编码
        /// </summary>
        [Label("设备编码")]
        public static readonly Property<string> EquipmentCodeProperty = P<ReceiveScanSnViewModel>.Register(e => e.EquipmentCode);

        /// <summary>
        /// 设备编码
        /// </summary>
        public string EquipmentCode
        {
            get { return GetProperty(EquipmentCodeProperty); }
            set { SetProperty(EquipmentCodeProperty, value); }
        }
        #endregion

        #region 原厂序列号 OriginalSn
        /// <summary>
        /// 原厂序列号
        /// </summary>
        [Label("原厂序列号")]
        public static readonly Property<string> OriginalSnProperty = P<ReceiveScanSnViewModel>.Register(e => e.OriginalSn);

        /// <summary>
        /// 原厂序列号
        /// </summary>
        public string OriginalSn
        {
            get { return GetProperty(OriginalSnProperty); }
            set { SetProperty(OriginalSnProperty, value); }
        }
        #endregion
    }
}
