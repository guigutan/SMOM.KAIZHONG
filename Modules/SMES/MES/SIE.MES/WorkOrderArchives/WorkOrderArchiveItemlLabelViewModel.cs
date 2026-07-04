using SIE.Domain;
using SIE.MES.WorkOrders;
using SIE.ObjectModel;
using SIE.Packages.ItemLabels;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.MES.WorkOrderArchives
{
    /// <summary>
    /// 工单制造档案待用标签
    /// </summary>
    [RootEntity, Serializable]
    [Label("工单制造档案待用标签")]
    public class WorkOrderArchiveItemlLabelViewModel : ViewModel
    {
        #region 标签号 Label
        /// <summary>
        /// 标签号
        /// </summary>
        [Label("标签号")]
        public static readonly Property<string> LabelProperty = P<WorkOrderArchiveItemlLabelViewModel>.Register(e => e.Label);

        /// <summary>
        /// 标签号
        /// </summary>
        public string Label
        {
            get { return this.GetProperty(LabelProperty); }
            set { this.SetProperty(LabelProperty, value); }
        }
        #endregion

        #region 批次号 BatchNo
        /// <summary>
        /// 批次号
        /// </summary>
        [Label("批次号")]
        public static readonly Property<string> BatchNoProperty = P<WorkOrderArchiveItemlLabelViewModel>.Register(e => e.BatchNo);

        /// <summary>
        /// 批次号
        /// </summary>
        public string BatchNo
        {
            get { return this.GetProperty(BatchNoProperty); }
            set { this.SetProperty(BatchNoProperty, value); }
        }
        #endregion

        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<WorkOrderArchiveItemlLabelViewModel>.Register(e => e.ItemCode);

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode
        {
            get { return this.GetProperty(ItemCodeProperty); }
            set { this.SetProperty(ItemCodeProperty, value); }
        }
        #endregion

        #region 物料名称 ItemName
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("物料名称")]
        public static readonly Property<string> ItemNameProperty = P<WorkOrderArchiveItemlLabelViewModel>.Register(e => e.ItemName);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
            set { this.SetProperty(ItemNameProperty, value); }
        }
        #endregion

        #region 物料拓展属性 ItemExPro
        /// <summary>
        /// 物料拓展属性
        /// </summary>
        [Label("物料拓展属性")]
        public static readonly Property<string> ItemExProProperty = P<WorkOrderArchiveItemlLabelViewModel>.Register(e => e.ItemExPro);

        /// <summary>
        /// 物料拓展属性
        /// </summary>
        public string ItemExPro
        {
            get { return this.GetProperty(ItemExProProperty); }
            set { this.SetProperty(ItemExProProperty, value); }
        }
        #endregion

        #region 仓库 Warehouse
        /// <summary>
        /// 仓库
        /// </summary>
        [Label("仓库")]
        public static readonly Property<string> WarehouseProperty = P<WorkOrderArchiveItemlLabelViewModel>.Register(e => e.Warehouse);

        /// <summary>
        /// 仓库
        /// </summary>
        public string Warehouse
        {
            get { return this.GetProperty(WarehouseProperty); }
            set { this.SetProperty(WarehouseProperty, value); }
        }
        #endregion

        #region 库位 Storage
        /// <summary>
        /// 库位
        /// </summary>
        [Label("库位")]
        public static readonly Property<string> StorageProperty = P<WorkOrderArchiveItemlLabelViewModel>.Register(e => e.Storage);

        /// <summary>
        /// 库位
        /// </summary>
        public string Storage
        {
            get { return this.GetProperty(StorageProperty); }
            set { this.SetProperty(StorageProperty, value); }
        }
        #endregion

        #region 可用数量 Qty
        /// <summary>
        /// 可用数量
        /// </summary>
        [Label("可用数量")]
        public static readonly Property<decimal> QtyProperty = P<WorkOrderArchiveItemlLabelViewModel>.Register(e => e.Qty);

        /// <summary>
        /// 可用数量
        /// </summary>
        public decimal Qty
        {
            get { return this.GetProperty(QtyProperty); }
            set { this.SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 序列号管理 IsSerialNumber
        /// <summary>
        /// 序列号管理
        /// </summary>
        [Label("序列号管理")]
        public static readonly Property<bool?> IsSerialNumberProperty = P<WorkOrderArchiveItemlLabelViewModel>.Register(e => e.IsSerialNumber);

        /// <summary>
        /// 序列号管理
        /// </summary>
        public bool? IsSerialNumber
        {
            get { return this.GetProperty(IsSerialNumberProperty); }
            set { this.SetProperty(IsSerialNumberProperty, value); }
        }
        #endregion

    }
}
