using SIE.Domain;
using SIE.ObjectModel;
using SIE.Warehouses;
using System;

namespace SIE.EMS.SpareParts
{
    /// <summary>
    /// 仓库明细
    /// </summary>
    [ChildEntity, Serializable]
    [Label("仓库明细")]
    public class StoreSummaryWarehouse : ViewModel 
    {
        #region 仓库 Warehouse
        /// <summary>
        /// 仓库Id
        /// </summary>
        [Label("仓库")]
        public static readonly IRefIdProperty WarehouseIdProperty =
            P<StoreSummaryWarehouse>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

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
            P<StoreSummaryWarehouse>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

        /// <summary>
        /// 仓库
        /// </summary>
        public Warehouse Warehouse
        {
            get { return this.GetRefEntity(WarehouseProperty); }
            set { this.SetRefEntity(WarehouseProperty, value); }
        }
        #endregion

        #region 仓库编码 WarehouseCode
        /// <summary>
        /// 仓库编码
        /// </summary>
        [Label("仓库编码")]
        public static readonly Property<string> WarehouseCodeProperty = P<StoreSummaryWarehouse>.Register(e => e.WarehouseCode);

        /// <summary>
        /// 仓库编码
        /// </summary>
        public string WarehouseCode
        {
            get { return this.GetProperty(WarehouseCodeProperty); }
            set { this.SetProperty(WarehouseCodeProperty, value); }
        }
        #endregion

        #region 仓库名称 WarehouseName
        /// <summary>
        /// 仓库名称
        /// </summary>
        [Label("仓库名称")]
        public static readonly Property<string> WarehouseNameProperty = P<StoreSummaryWarehouse>.Register(e => e.WarehouseName);

        /// <summary>
        /// 仓库名称
        /// </summary>
        public string WarehouseName
        {
            get { return this.GetProperty(WarehouseNameProperty); }
            set { this.SetProperty(WarehouseNameProperty, value); }
        }
        #endregion

        #region 分类 LibraryType
        /// <summary>
        /// 分类
        /// </summary>
        [Label("分类")]
        public static readonly Property<LibraryType> LibraryTypeProperty = P<StoreSummaryWarehouse>.Register(e => e.LibraryType);

        /// <summary>
        /// 分类
        /// </summary>
        public LibraryType LibraryType
        {
            get { return this.GetProperty(LibraryTypeProperty); }
            set { this.SetProperty(LibraryTypeProperty, value); }
        }
        #endregion

        #region 零成本仓 IsZeroCost
        /// <summary>
        /// 零成本仓
        /// </summary>
        [Label("零成本仓")]
        public static readonly Property<bool> IsZeroCostProperty = P<StoreSummaryWarehouse>.Register(e => e.IsZeroCost);

        /// <summary>
        /// 零成本仓
        /// </summary>
        public bool IsZeroCost
        {
            get { return this.GetProperty(IsZeroCostProperty); }
            set { this.SetProperty(IsZeroCostProperty, value); }
        }
        #endregion

        #region 不良品数 RotNumber
        /// <summary>
        /// 不良品数
        /// </summary>
        [Label("不良品数")]
        public static readonly Property<int> RotNumberProperty = P<StoreSummaryWarehouse>.Register(e => e.RotNumber);

        /// <summary>
        /// 不良品数
        /// </summary>
        public int RotNumber
        {
            get { return this.GetProperty(RotNumberProperty); }
            set { this.SetProperty(RotNumberProperty, value); }
        }
        #endregion

        #region 可用库存 GoodNumber
        /// <summary>
        /// 可用库存
        /// </summary>
        [Label("可用库存")]
        public static readonly Property<int> GoodNumberProperty = P<StoreSummaryWarehouse>.Register(e => e.GoodNumber);

        /// <summary>
        /// 可用库存
        /// </summary>
        public int GoodNumber
        {
            get { return this.GetProperty(GoodNumberProperty); }
            set { this.SetProperty(GoodNumberProperty, value); }
        }
        #endregion

        #region 总库存 SumNumber
        /// <summary>
        /// 总库存
        /// </summary>
        [Label("总库存")]
        public static readonly Property<int> SumNumberProperty = P<StoreSummaryWarehouse>.Register(e => e.SumNumber);

        /// <summary>
        /// 总库存
        /// </summary>
        public int SumNumber
        {
            get { return this.GetProperty(SumNumberProperty); }
            set { this.SetProperty(SumNumberProperty, value); }
        }
        #endregion

        #region 备件Id SpartpartId
        /// <summary>
        /// 备件Id 用于统计
        /// </summary>
        [Label("备件Id")]
        public static readonly Property<double> SpartpartIdProperty = P<StoreSummaryWarehouse>.Register(e => e.SpartpartId);

        /// <summary>
        /// 备件Id
        /// </summary>
        public double SpartpartId
        {
            get { return this.GetProperty(SpartpartIdProperty); }
            set { this.SetProperty(SpartpartIdProperty, value); }
        }
        #endregion

    }
}
