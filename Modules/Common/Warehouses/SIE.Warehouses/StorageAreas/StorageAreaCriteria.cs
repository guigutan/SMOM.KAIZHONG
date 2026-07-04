using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.Warehouses
{
    /// <summary>
    /// 库区查询
    /// </summary>
    [QueryEntity, Serializable]
    [Label("库区查询")]
    public partial class StorageAreaCriteria : Criteria
    {
        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Label("编码")]
        [MaxLength(80)]
        public static readonly Property<string> CodeProperty = P<StorageAreaCriteria>.Register(e => e.Code);

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
        [MaxLength(80)]
        public static readonly Property<string> NameProperty = P<StorageAreaCriteria>.Register(e => e.Name);

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
        public static readonly Property<LibraryType?> LibraryTypeProperty = P<StorageAreaCriteria>.Register(e => e.LibraryType);

        /// <summary>
        /// 类型
        /// </summary>
        public LibraryType? LibraryType
        {
            get { return GetProperty(LibraryTypeProperty); }
            set { SetProperty(LibraryTypeProperty, value); }
        }
        #endregion

        #region 仓库 Warehouse
        /// <summary>
        /// 仓库
        /// </summary>
        [Label("仓库")]
        public static readonly Property<string> WarehouseProperty = P<StorageAreaCriteria>.Register(e => e.Warehouse);

        /// <summary>
        /// 仓库
        /// </summary>
        public string Warehouse
        {
            get { return GetProperty(WarehouseProperty); }
            set { SetProperty(WarehouseProperty, value); }
        }
        #endregion

        #region 是否冻结 IsFrozen
        /// <summary>
        /// 是否冻结
        /// </summary>
        [Label("是否冻结")]
        public static readonly Property<bool?> IsFrozenProperty = P<StorageAreaCriteria>.Register(e => e.IsFrozen);

        /// <summary>
        /// 是否冻结
        /// </summary>
        public bool? IsFrozen
        {
            get { return GetProperty(IsFrozenProperty); }
            set { SetProperty(IsFrozenProperty, value); }
        }
        #endregion

        #region 仓库(用于多选联动查询) Warehouses
        /// <summary>
        /// 仓库
        /// </summary>
        [Label("仓库")]
        public static readonly Property<string> WarehousesProperty = P<StorageAreaCriteria>.Register(e => e.Warehouses);

        /// <summary>
        /// 仓库
        /// </summary>
        public string Warehouses
        {
            get { return this.GetProperty(WarehousesProperty); }
            set { this.SetProperty(WarehousesProperty, value); }
        }
        #endregion

        #region 是否非立库 IsNotAutomated
        /// <summary>
        /// 是否非立库
        /// </summary>
        [Label("是否非立库")]
        public static readonly Property<bool> IsNotAutomatedProperty = P<StorageAreaCriteria>.Register(e => e.IsNotAutomated);

        /// <summary>
        /// 是否非立库
        /// </summary>
        public bool IsNotAutomated
        {
            get { return GetProperty(IsNotAutomatedProperty); }
            set { SetProperty(IsNotAutomatedProperty, value); }
        }
        #endregion

        #region 状态 State
        /// <summary>
        /// 状态
        /// </summary>
        public static readonly Property<State?> StateProperty = P<StorageAreaCriteria>.Register(e => e.State);

        /// <summary>
        /// 状态
        /// </summary>
        public State? State
        {
            get { return this.GetProperty(StateProperty); }
            set { this.SetProperty(StateProperty, value); }
        }
        #endregion

        /// <summary>
        /// 查询逻辑
        /// </summary>
        /// <returns>返回查询后的数据</returns>
        protected override EntityList Fetch()
        {
            var result = RT.Service.Resolve<WarehouseController>().GetStorageAreaData(this);
            return result;
        }
    }
}
