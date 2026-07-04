using SIE.Domain;
using SIE.ObjectModel;
using SIE.Warehouses;
using System;

namespace SIE.EMS.SpareParts
{
    /// <summary>
    /// 库位明细
    /// </summary>
    [RootEntity, Serializable]
    [Label("库位明细")]
    public class StoreSummaryStock : ViewModel
    {
        #region 仓库编码 WarehouseCode
        /// <summary>
        /// 仓库编码
        /// </summary>
        [Label("仓库编码")]
        public static readonly Property<string> WarehouseCodeProperty = P<StoreSummaryStock>.Register(e => e.WarehouseCode);

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
        public static readonly Property<string> WarehouseNameProperty = P<StoreSummaryStock>.Register(e => e.WarehouseName);

        /// <summary>
        /// 仓库名称
        /// </summary>
        public string WarehouseName
        {
            get { return this.GetProperty(WarehouseNameProperty); }
            set { this.SetProperty(WarehouseNameProperty, value); }
        }
        #endregion

        #region 库位编码 StorageLocationCode
        /// <summary>
        /// 库位编码
        /// </summary>
        [Label("库位编码")]
        public static readonly Property<string> StorageLocationCodeProperty = P<StoreSummaryStock>.Register(e => e.StorageLocationCode);

        /// <summary>
        /// 库位编码
        /// </summary>
        public string StorageLocationCode
        {
            get { return this.GetProperty(StorageLocationCodeProperty); }
            set { this.SetProperty(StorageLocationCodeProperty, value); }
        }
        #endregion

        #region 库位名称 StorageLocationName
        /// <summary>
        /// 库位名称
        /// </summary>
        [Label("库位名称")]
        public static readonly Property<string> StorageLocationNameProperty = P<StoreSummaryStock>.Register(e => e.StorageLocationName);

        /// <summary>
        /// 库位名称
        /// </summary>
        public string StorageLocationName
        {
            get { return this.GetProperty(StorageLocationNameProperty); }
            set { this.SetProperty(StorageLocationNameProperty, value); }
        }
        #endregion

        #region 分类 LibraryType
        /// <summary>
        /// 分类
        /// </summary>
        [Label("分类")]
        public static readonly Property<LibraryType> LibraryTypeProperty = P<StoreSummaryStock>.Register(e => e.LibraryType);

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
        public static readonly Property<bool> IsZeroCostProperty = P<StoreSummaryStock>.Register(e => e.IsZeroCost);

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
        public static readonly Property<int> RotNumberProperty = P<StoreSummaryStock>.Register(e => e.RotNumber);

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
        public static readonly Property<int> GoodNumberProperty = P<StoreSummaryStock>.Register(e => e.GoodNumber);

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
        public static readonly Property<int> SumNumberProperty = P<StoreSummaryStock>.Register(e => e.SumNumber);

        /// <summary>
        /// 总库存
        /// </summary>
        public int SumNumber
        {
            get { return this.GetProperty(SumNumberProperty); }
            set { this.SetProperty(SumNumberProperty, value); }
        }
        #endregion

    }
}
