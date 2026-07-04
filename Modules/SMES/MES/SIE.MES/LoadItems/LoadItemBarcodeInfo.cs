using SIE.Domain;
using SIE.Items;
using SIE.MES.SingleLabels;
using SIE.ObjectModel;
using System;

namespace SIE.MES.LoadItems
{
    /// <summary>
    /// 换料条码信息
    /// </summary>
    [RootEntity, Serializable]
    public class LoadItemBarcodeInfo : ViewModel
    {

        #region 条码 Barcode
        /// <summary>
        /// 条码
        /// </summary>
        [Label("条码")]
        public static readonly Property<string> BarcodeProperty = P<LoadItemBarcodeInfo>.Register(e => e.Barcode);

        /// <summary>
        /// 条码
        /// </summary>
        public string Barcode
        {
            get { return GetProperty(BarcodeProperty); }
            set { SetProperty(BarcodeProperty, value); }
        }
        #endregion

        #region 配送单号 BillNo
        /// <summary>
        /// 配送单号（配送上料物料对应的条码）
        /// 来源类型为配送时有值
        /// </summary>
        [Label("配送单号")]
        public static readonly Property<string> BillNoProperty = P<LoadItemBarcodeInfo>.Register(e => e.BillNo);

        /// <summary>
        /// 配送单号
        /// </summary>
        public string BillNo
        {
            get { return GetProperty(BillNoProperty); }
            set { SetProperty(BillNoProperty, value); }
        }
        #endregion

        #region 物料Id ItemId
        /// <summary>
        /// 物料Id
        /// </summary>
        [Label("物料Id")]
        public static readonly Property<double> ItemIdProperty = P<LoadItemBarcodeInfo>.Register(e => e.ItemId);

        /// <summary>
        /// 物料Id
        /// </summary>
        public double ItemId
        {
            get { return GetProperty(ItemIdProperty); }
            set { SetProperty(ItemIdProperty, value); }
        }
        #endregion

        #region 标签 Label
        /// <summary>
        /// 标签
        /// </summary>
        [Label("标签")]
        public static readonly Property<string> LabelProperty = P<LoadItemBarcodeInfo>.Register(e => e.Label);

        /// <summary>
        /// 标签
        /// </summary>
        public string Label
        {
            get { return GetProperty(LabelProperty); }
            set { SetProperty(LabelProperty, value); }
        }
        #endregion

        #region 条码来源类型 Type
        /// <summary>
        /// 条码来源类型
        /// </summary>
        [Label("条码来源类型")]
        public static readonly Property<LoadItemSourceType> TypeProperty = P<LoadItemBarcodeInfo>.Register(e => e.Type);

        /// <summary>
        /// 条码来源类型
        /// </summary>
        public LoadItemSourceType Type
        {
            get { return GetProperty(TypeProperty); }
            set { SetProperty(TypeProperty, value); }
        }
        #endregion

        #region 条码来源Id SourceId
        /// <summary>   
        /// 条码来源Id
        /// </summary>
        [Label("条码来源Id")]
        public static readonly Property<double> SourceIdProperty = P<LoadItemBarcodeInfo>.Register(e => e.SourceId);

        /// <summary>
        /// 条码来源Id
        /// </summary>
        public double SourceId
        {
            get { return GetProperty(SourceIdProperty); }
            set { SetProperty(SourceIdProperty, value); }
        }
        #endregion

        #region 可用数量 Qty
        /// <summary>
        /// 可用数量
        /// </summary>
        [Label("可用数量")]
        public static readonly Property<decimal> QtyProperty = P<LoadItemBarcodeInfo>.Register(e => e.Qty);

        /// <summary>
        /// 可用数量
        /// </summary>
        public decimal Qty
        {
            get { return GetProperty(QtyProperty); }
            set { SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<LoadItemBarcodeInfo>.Register(e => e.ItemCode);

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode
        {
            get { return GetProperty(ItemCodeProperty); }
            set { SetProperty(ItemCodeProperty, value); }
        }
        #endregion

        #region 物料名称 ItemName
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("物料名称")]
        public static readonly Property<string> ItemNameProperty = P<LoadItemBarcodeInfo>.Register(e => e.ItemName);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
            set { this.SetProperty(ItemNameProperty, value); }
        }
        #endregion


        #region 属性id ItemExtProp
        /// <summary>
        /// 属性id
        /// </summary>
        [Label("属性id")]
        public static readonly Property<string> ItemExtPropProperty = P<LoadItemBarcodeInfo>.Register(e => e.ItemExtProp);

        /// <summary>
        /// 属性id
        /// </summary>
        public string ItemExtProp
        {
            get { return GetProperty(ItemExtPropProperty); }
            set { SetProperty(ItemExtPropProperty, value); }
        }
        #endregion

        #region 属性名称 ItemExtPropName
        /// <summary>
        /// 属性名称
        /// </summary>
        [Label("属性名称")]
        public static readonly Property<string> ItemExtPropNameProperty = P<LoadItemBarcodeInfo>.Register(e => e.ItemExtPropName);

        /// <summary>
        /// 属性名称
        /// </summary>
        public string ItemExtPropName
        {
            get { return GetProperty(ItemExtPropNameProperty); }
            set { SetProperty(ItemExtPropNameProperty, value); }
        }
        #endregion

        #region 批次 LotNo
        /// <summary>
        /// 批次
        /// </summary>
        [Label("批次")]
        public static readonly Property<string> LotNoProperty = P<LoadItemBarcodeInfo>.Register(e => e.LotNo);

        /// <summary>
        /// 批次
        /// </summary>
        public string LotNo
        {
            get { return GetProperty(LotNoProperty); }
            set { SetProperty(LotNoProperty, value); }
        }
        #endregion

        #region 项目号 ProjectNo
        /// <summary>
        /// 项目号
        /// </summary>
        [Label("项目号")]
        public static readonly Property<string> ProjectNoProperty = P<LoadItemBarcodeInfo>.Register(e => e.ProjectNo);

        /// <summary>
        /// 项目号
        /// </summary>
        public string ProjectNo
        {
            get { return this.GetProperty(ProjectNoProperty); }
            set { this.SetProperty(ProjectNoProperty, value); }
        }
        #endregion

        #region 仓库 WarehouseName
        /// <summary>
        /// 仓库
        /// </summary>
        [Label("仓库")]
        public static readonly Property<string> WarehouseNameProperty = P<LoadItemBarcodeInfo>.Register(e => e.WarehouseName);

        /// <summary>
        /// 仓库
        /// </summary>
        public string WarehouseName
        {
            get { return GetProperty(WarehouseNameProperty); }
            set { SetProperty(WarehouseNameProperty, value); }
        }
        #endregion

        #region 库位 Localtion
        /// <summary>
        /// 库位
        /// </summary>
        [Label("库位")]
        public static readonly Property<string> StorageLocationCodeProperty = P<LoadItemBarcodeInfo>.Register(e => e.StorageLocationCode);

        /// <summary>
        /// 库位
        /// </summary>
        public string StorageLocationCode
        {
            get { return GetProperty(StorageLocationCodeProperty); }
            set { SetProperty(StorageLocationCodeProperty, value); }
        }
        #endregion

        #region 当前在制工单 WipWorkOrderId
        /// <summary>
        /// 当前在制工单
        /// </summary>
        [Label("当前在制工单")]
        public static readonly Property<double> WipWorkOrderIdProperty = P<LoadItemBarcodeInfo>.Register(e => e.WipWorkOrderId);

        /// <summary>
        /// 当前在制工单
        /// </summary>
        public double WipWorkOrderId
        {
            get { return GetProperty(WipWorkOrderIdProperty); }
            set { SetProperty(WipWorkOrderIdProperty, value); }
        }
        #endregion

        #region 在制工单号 WipWorkOrderNo
        /// <summary>
        /// 在制工单号
        /// </summary>
        [Label("在制工单号")]
        public static readonly Property<string> WipWorkOrderNoProperty = P<LoadItemBarcodeInfo>.Register(e => e.WipWorkOrderNo);

        /// <summary>
        /// 在制工单号
        /// </summary>
        public string WipWorkOrderNo
        {
            get { return GetProperty(WipWorkOrderNoProperty); }
            set { SetProperty(WipWorkOrderNoProperty, value); }
        }
        #endregion

        #region 是否序列号管理 IsSerialNumber
        /// <summary>
        /// 是否序列号管理
        /// </summary>
        [Label("是否序列号管理")]
        public static readonly Property<bool?> IsSerialNumberProperty = P<LoadItemBarcodeInfo>.Register(e => e.IsSerialNumber);

        /// <summary>
        /// 是否序列号管理
        /// </summary>
        public bool? IsSerialNumber
        {
            get { return GetProperty(IsSerialNumberProperty); }
            set { SetProperty(IsSerialNumberProperty, value); }
        }
        #endregion

        #region 物料消耗类型 ConsumeMode
        /// <summary>
        /// 物料消耗类型
        /// </summary>
        [Label("物料消耗类型")]
        public static readonly Property<ConsumeMode> ConsumeModeProperty = P<LoadItemBarcodeInfo>.Register(e => e.ConsumeMode);

        /// <summary>
        /// 物料消耗类型
        /// </summary>
        public ConsumeMode ConsumeMode
        {
            get { return GetProperty(ConsumeModeProperty); }
            set { SetProperty(ConsumeModeProperty, value); }
        }
        #endregion

        #region 显示条码 DisplayBarCode
        /// <summary>
        /// 显示条码/产品条码
        /// </summary>
        [Label("显示条码")]
        public static readonly Property<string> DisplayBarCodeProperty = P<LoadItemBarcodeInfo>.Register(e => e.DisplayBarCode);

        /// <summary>
        /// 显示条码/产品条码
        /// </summary>
        public string DisplayBarCode
        {
            get { return this.GetProperty(DisplayBarCodeProperty); }
            set { this.SetProperty(DisplayBarCodeProperty, value); }
        }
        #endregion
    }
}
