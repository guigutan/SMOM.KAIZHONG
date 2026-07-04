using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Warehouses;
using System;

namespace SIE.EMS.SpareParts
{
    /// <summary>
    /// 备件库统计数据实体
    /// </summary>
    [RootEntity, Serializable]
    [Label("备件库统计数据实体")]
    public class SparePartMixReportRecord : DataEntity
    {

        #region 入库数量 ReceiptQty
        /// <summary>
        /// 入库数量
        /// </summary>
        [Label("入库数量")]
        public static readonly Property<int> ReceiptQtyProperty = P<SparePartMixReportRecord>.Register(e => e.ReceiptQty);

        /// <summary>
        /// 入库数量
        /// </summary>
        public int ReceiptQty
        {
            get { return this.GetProperty(ReceiptQtyProperty); }
            set { this.SetProperty(ReceiptQtyProperty, value); }
        }
        #endregion


        #region 入库金额 ReceiptAmount
        /// <summary>
        /// 入库金额
        /// </summary>
        [Label("入库金额")]
        public static readonly Property<decimal> ReceiptAmountProperty = P<SparePartMixReportRecord>.Register(e => e.ReceiptAmount);

        /// <summary>
        /// 入库金额
        /// </summary>
        public decimal ReceiptAmount
        {
            get { return this.GetProperty(ReceiptAmountProperty); }
            set { this.SetProperty(ReceiptAmountProperty, value); }
        }
        #endregion


        #region 出库金额 ExWarehouseAmount
        /// <summary>
        /// 出库金额
        /// </summary>
        [Label("出库金额")]
        public static readonly Property<decimal> ExWarehouseAmountProperty = P<SparePartMixReportRecord>.Register(e => e.ExWarehouseAmount);

        /// <summary>
        /// 出库金额
        /// </summary>
        public decimal ExWarehouseAmount
        {
            get { return this.GetProperty(ExWarehouseAmountProperty); }
            set { this.SetProperty(ExWarehouseAmountProperty, value); }
        }
        #endregion

        #region 出库数量 ExWarehouseQty
        /// <summary>
        /// 出库数量
        /// </summary>
        [Label("出库数量")]
        public static readonly Property<int> ExWarehouseQtyProperty = P<SparePartMixReportRecord>.Register(e => e.ExWarehouseQty);

        /// <summary>
        /// 出库数量
        /// </summary>
        public int ExWarehouseQty
        {
            get { return this.GetProperty(ExWarehouseQtyProperty); }
            set { this.SetProperty(ExWarehouseQtyProperty, value); }
        }
        #endregion

        #region 仓库结余数 SurplusQty
        /// <summary>
        /// 仓库结余数
        /// </summary>
        [Label("仓库结余数")]
        public static readonly Property<int> SurplusQtyProperty = P<SparePartMixReportRecord>.Register(e => e.SurplusQty);

        /// <summary>
        /// 月末结余数
        /// </summary>
        public int SurplusQty
        {
            get { return this.GetProperty(SurplusQtyProperty); }
            set { this.SetProperty(SurplusQtyProperty, value); }
        }
        #endregion

        #region 仓库结余金额 SurplusAmount
        /// <summary>
        /// 月末结余金额
        /// </summary>
        [Label("仓库结余金额")]
        public static readonly Property<decimal> SurplusAmountProperty = P<SparePartMixReportRecord>.Register(e => e.SurplusAmount);

        /// <summary>
        /// 月末结余金额
        /// </summary>
        public decimal SurplusAmount
        {
            get { return this.GetProperty(SurplusAmountProperty); }
            set { this.SetProperty(SurplusAmountProperty, value); }
        }
        #endregion


        #region 调度日期  SchedulingDate
        /// <summary>
        /// 调度日期 
        /// </summary>
        [Label("调度日期")]
        public static readonly Property<DateTime> SchedulingDateProperty = P<SparePartMixReportRecord>.Register(e => e.SchedulingDate);

        /// <summary>
        /// 调度日期 
        /// </summary>
        public DateTime SchedulingDate
        {
            get { return this.GetProperty(SchedulingDateProperty); }
            set { this.SetProperty(SchedulingDateProperty, value); }
        }
        #endregion


        #region 仓库 Warehouse
        /// <summary>
        /// 仓库Id
        /// </summary>
        [Label("仓库")]
        public static readonly IRefIdProperty WarehouseIdProperty =
            P<SparePartMixReportRecord>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

        /// <summary>
        /// 仓库Id
        /// </summary>
        public double WarehouseId
        {
            get { return (double)this.GetRefId(WarehouseIdProperty); }
            set { this.SetRefId(WarehouseIdProperty, value); }
        }

        /// <summary>
        /// 仓库
        /// </summary>
        public static readonly RefEntityProperty<Warehouse> WarehouseProperty =
            P<SparePartMixReportRecord>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

        /// <summary>
        /// 仓库
        /// </summary>
        public Warehouse Warehouse
        {
            get { return this.GetRefEntity(WarehouseProperty); }
            set { this.SetRefEntity(WarehouseProperty, value); }
        }
        #endregion
    }
    internal class SpartPartMitReportRecordConfig : EntityConfig<SparePartMixReportRecord>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_SPARE_PART_Mix_Rep_Rec").MapAllProperties();
            Meta.Property(SparePartMixReportRecord.WarehouseIdProperty).ColumnMeta.HasIndex();
            //Meta.Property(SparePartMixReportRecord.SchedulingDateProperty).ColumnMeta.HasIndex();
            Meta.EnablePhantoms();
        }
    }
}
