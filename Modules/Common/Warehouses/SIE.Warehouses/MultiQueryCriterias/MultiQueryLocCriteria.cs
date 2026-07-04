using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;

namespace SIE.Warehouses
{
    /// <summary>
    /// 库位多选查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("多选库位查询")]
    public class MultiQueryLocCriteria : Criteria
    {
        #region 排除Id FilterId
        /// <summary>
        /// 排除Id
        /// </summary>
        public static readonly Property<List<double>> FilterIdProperty = P<MultiQueryLocCriteria>.Register(e => e.FilterId);

        /// <summary>
        /// 排除Id
        /// </summary>
        public List<double> FilterId
        {
            get { return this.GetProperty(FilterIdProperty); }
            set { this.SetProperty(FilterIdProperty, value); }
        }
        #endregion

        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Label("编码")]
        public static readonly Property<string> CodeProperty = P<MultiQueryLocCriteria>.Register(e => e.Code);

        /// <summary>
        /// 编码
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 名称 Name
        /// <summary>
        /// 名称
        /// </summary>
        [Label("名称")]
        public static readonly Property<string> NameProperty = P<MultiQueryLocCriteria>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 类型 LibraryType
        /// <summary>
        /// 类型
        /// </summary>
        [Label("类型")]
        public static readonly Property<LibraryType?> LibraryTypeProperty = P<MultiQueryLocCriteria>.Register(e => e.LibraryType);

        /// <summary>
        /// 类型
        /// </summary>
        public LibraryType? LibraryType
        {
            get { return GetProperty(LibraryTypeProperty); }
            set { SetProperty(LibraryTypeProperty, value); }
        }
        #endregion

        #region 仓库 Warehouses
        /// <summary>
        /// 仓库
        /// </summary>
        [Label("仓库")]
        public static readonly Property<string> WarehousesProperty = P<MultiQueryLocCriteria>.Register(e => e.Warehouses);

        /// <summary>
        /// 仓库
        /// </summary>
        public string Warehouses
        {
            get { return GetProperty(WarehousesProperty); }
            set { SetProperty(WarehousesProperty, value); }
        }
        #endregion

        #region 仓库 Warehouse
        /// <summary>
        /// 仓库
        /// </summary>
        [Label("仓库")]
        public static readonly Property<string> WarehouseProperty = P<MultiQueryLocCriteria>.Register(e => e.Warehouse);

        /// <summary>
        /// 仓库
        /// </summary>
        public string Warehouse
        {
            get { return this.GetProperty(WarehouseProperty); }
            set { this.SetProperty(WarehouseProperty, value); }
        }
        #endregion

        #region 库区 Area
        /// <summary>
        /// 库区
        /// </summary>
        [Label("库区")]
        public static readonly Property<string> AreaProperty = P<MultiQueryLocCriteria>.Register(e => e.Area);

        /// <summary>
        /// 库区
        /// </summary>
        public string Area
        {
            get { return this.GetProperty(AreaProperty); }
            set { this.SetProperty(AreaProperty, value); }
        }
        #endregion

        #region 库区 AreaIds
        /// <summary>
        /// 库区
        /// </summary>
        [Label("库区")]
        public static readonly Property<string> AreaIdsProperty = P<MultiQueryLocCriteria>.Register(e => e.AreaIds);

        /// <summary>
        /// 库区
        /// </summary>
        public string AreaIds
        {
            get { return GetProperty(AreaIdsProperty); }
            set { SetProperty(AreaIdsProperty, value); }
        }
        #endregion

        #region 是否冻结 IsFrozen
        /// <summary>
        /// 是否冻结
        /// </summary>
        [Label("是否冻结")]
        public static readonly Property<bool?> IsFrozenProperty = P<MultiQueryLocCriteria>.Register(e => e.IsFrozen);

        /// <summary>
        /// 是否冻结
        /// </summary>
        public bool? IsFrozen
        {
            get { return GetProperty(IsFrozenProperty); }
            set { SetProperty(IsFrozenProperty, value); }
        }
        #endregion

        /// <summary>
        /// 查询
        /// </summary>
        /// <returns>库位集合</returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<WarehouseController>().GetStorageLocations(this);
        }
    }
}
