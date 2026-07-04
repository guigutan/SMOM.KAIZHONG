using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Purchases.EquipmentReceives
{
    /// <summary>
    /// 序列号明细
    /// </summary>
    [ChildEntity, Serializable]
    [Label("序列号明细")]
    public partial class EquipmentReceiveSn : DataEntity
    {
        #region 接收明细 EquipmentReceiveDetail
        /// <summary>
        /// 接收明细Id
        /// </summary>
        public static readonly IRefIdProperty EquipmentReceiveDetailIdProperty = P<EquipmentReceiveSn>.RegisterRefId(e => e.EquipmentReceiveDetailId, ReferenceType.Parent);

        /// <summary>
        /// 接收明细Id
        /// </summary>
        public double EquipmentReceiveDetailId
        {
            get { return (double)GetRefId(EquipmentReceiveDetailIdProperty); }
            set { SetRefId(EquipmentReceiveDetailIdProperty, value); }
        }

        /// <summary>
        /// 接收明细
        /// </summary>
        public static readonly RefEntityProperty<EquipmentReceiveDetail> EquipmentReceiveDetailProperty = P<EquipmentReceiveSn>.RegisterRef(e => e.EquipmentReceiveDetail, EquipmentReceiveDetailIdProperty);

        /// <summary>
        /// 接收明细
        /// </summary>
        public EquipmentReceiveDetail EquipmentReceiveDetail
        {
            get { return GetRefEntity(EquipmentReceiveDetailProperty); }
            set { SetRefEntity(EquipmentReceiveDetailProperty, value); }
        }
        #endregion

        #region 设备编码 EquipmentCode
        /// <summary>
        /// 设备编码
        /// </summary>
        [Label("设备编码")]
        public static readonly Property<string> EquipmentCodeProperty = P<EquipmentReceiveSn>.Register(e => e.EquipmentCode);

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
        public static readonly Property<string> OriginalSnProperty = P<EquipmentReceiveSn>.Register(e => e.OriginalSn);

        /// <summary>
        /// 原厂序列号
        /// </summary>
        public string OriginalSn
        {
            get { return GetProperty(OriginalSnProperty); }
            set { SetProperty(OriginalSnProperty, value); }
        }
        #endregion

        #region 是否原有设备 IsOriginalEquipment
        /// <summary>
        /// 是否原有设备
        /// </summary>
        [Label("是否原有设备")]
        public static readonly Property<bool> IsOriginalEquipmentProperty = P<EquipmentReceiveSn>.Register(e => e.IsOriginalEquipment);

        /// <summary>
        /// 是否原有设备
        /// </summary>
        public bool IsOriginalEquipment
        {
            get { return GetProperty(IsOriginalEquipmentProperty); }
            set { SetProperty(IsOriginalEquipmentProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 接收行号 ReceiveLineNo
        /// <summary>
        /// 接收行号
        /// </summary>
        [Label("接收行号")]
        public static readonly Property<int> ReceiveLineNoProperty = P<EquipmentReceiveSn>.RegisterView(e => e.ReceiveLineNo, p => p.EquipmentReceiveDetail.LineNo);

        /// <summary>
        /// 接收行号
        /// </summary>
        public int ReceiveLineNo
        {
            get { return this.GetProperty(ReceiveLineNoProperty); }
        }
        #endregion

        #region 采购单号 PurchaseOrderNo
        /// <summary>
        /// 采购单号
        /// </summary>
        [Label("采购单号")]
        public static readonly Property<string> PurchaseOrderNoProperty = P<EquipmentReceiveSn>.RegisterView(e => e.PurchaseOrderNo, p => p.EquipmentReceiveDetail.PurchaseOrder.OrderNo);

        /// <summary>
        /// 采购单号
        /// </summary>
        public string PurchaseOrderNo
        {
            get { return this.GetProperty(PurchaseOrderNoProperty); }
        }
        #endregion

        #region 采购单行号 OrderItemNo
        /// <summary>
        /// 采购单行号
        /// </summary>
        [Label("采购单行号")]
        public static readonly Property<int?> OrderItemNoProperty = P<EquipmentReceiveSn>.RegisterView(e => e.OrderItemNo, p => p.EquipmentReceiveDetail.PurchaseOrderItem.LineNo);

        /// <summary>
        /// 采购单行号
        /// </summary>
        public int? OrderItemNo
        {
            get { return this.GetProperty(OrderItemNoProperty); }
        }
        #endregion

        #region 设备型号编码 EquipModelCode
        /// <summary>
        /// 设备型号编码
        /// </summary>
        [Label("设备型号编码")]
        public static readonly Property<string> EquipModelCodeProperty = P<EquipmentReceiveSn>.RegisterView(e => e.EquipModelCode, p => p.EquipmentReceiveDetail.EquipModel.Code);

        /// <summary>
        /// 设备型号编码
        /// </summary>
        public string EquipModelCode
        {
            get { return this.GetProperty(EquipModelCodeProperty); }
        }
        #endregion

        #region 设备型号名称 EquipModelName
        /// <summary>
        /// 设备型号名称
        /// </summary>
        [Label("设备型号名称")]
        public static readonly Property<string> EquipModelNameProperty = P<EquipmentReceiveSn>.RegisterView(e => e.EquipModelName, p => p.EquipmentReceiveDetail.EquipModel.Name);

        /// <summary>
        /// 设备型号名称
        /// </summary>
        public string EquipModelName
        {
            get { return this.GetProperty(EquipModelNameProperty); }
        }
        #endregion

        #region 赠品 Giveaway
        /// <summary>
        /// 赠品
        /// </summary>
        [Label("赠品")]
        public static readonly Property<bool> GiveawayProperty = P<EquipmentReceiveSn>.RegisterView(e => e.Giveaway, p => p.EquipmentReceiveDetail.Giveaway);

        /// <summary>
        /// 赠品
        /// </summary>
        public bool Giveaway
        {
            get { return this.GetProperty(GiveawayProperty); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 序列号明细 实体配置
    /// </summary>
    internal class EquipmentReceiveSnConfig : EntityConfig<EquipmentReceiveSn>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_EQP_RECV_SN").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}