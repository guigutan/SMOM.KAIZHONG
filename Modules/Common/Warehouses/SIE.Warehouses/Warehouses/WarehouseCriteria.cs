using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.Warehouses
{
    /// <summary>
    /// 仓库查询
    /// </summary>
    [QueryEntity, Serializable]
    [Label("仓库查询")]
    public partial class WarehouseCriteria : Criteria
    {
        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Label("编码")]
        public static readonly Property<string> CodeProperty = P<WarehouseCriteria>.Register(e => e.Code);

        /// <summary>
        /// 编码
        /// </summary>
        public string Code
        {
            get { return this.GetProperty(CodeProperty); }
            set { this.SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 名称 Name
        /// <summary>
        /// 名称
        /// </summary>
        [Label("名称")]
        public static readonly Property<string> NameProperty = P<WarehouseCriteria>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return this.GetProperty(NameProperty); }
            set { this.SetProperty(NameProperty, value); }
        }
        #endregion

        #region 类型 LibraryType
        /// <summary>
        /// 类型
        /// </summary>
        [Label("类型")]
        public static readonly Property<LibraryType?> LibraryTypeProperty = P<WarehouseCriteria>.Register(e => e.LibraryType);

        /// <summary>
        /// 类型
        /// </summary>
        public LibraryType? LibraryType
        {
            get { return this.GetProperty(LibraryTypeProperty); }
            set { this.SetProperty(LibraryTypeProperty, value); }
        }
        #endregion

        #region 分类 Category
        /// <summary>
        /// 分类
        /// </summary>
        [Label("分类")]
        public static readonly Property<string> CategoryProperty = P<WarehouseCriteria>.Register(e => e.Category);

        /// <summary>
        /// 分类
        /// </summary>
        public string Category
        {
            get { return this.GetProperty(CategoryProperty); }
            set { this.SetProperty(CategoryProperty, value); }
        }
        #endregion

        #region 是否冻结 IsFrozen
        /// <summary>
        /// 是否冻结
        /// </summary>
        [Label("是否冻结")]
        public static readonly Property<bool?> IsFrozenProperty = P<WarehouseCriteria>.Register(e => e.IsFrozen);

        /// <summary>
        /// 是否冻结
        /// </summary>
        public bool? IsFrozen
        {
            get { return this.GetProperty(IsFrozenProperty); }
            set { this.SetProperty(IsFrozenProperty, value); }
        }
        #endregion

        #region 状态 State
        /// <summary>
        /// 状态
        /// </summary>
        public static readonly Property<State?> StateProperty = P<WarehouseCriteria>.Register(e => e.State);

        /// <summary>
        /// 状态
        /// </summary>
        public State? State
        {
            get { return this.GetProperty(StateProperty); }
            set { this.SetProperty(StateProperty, value); }
        }
        #endregion

        #region 是否员工权限仓库 IsEmployeeWarehouse
        /// <summary>
        /// 是否员工权限仓库
        /// </summary>
        [Label("是否员工权限仓库")]
        public static readonly Property<bool> IsEmployeeWarehouseProperty = P<WarehouseCriteria>.Register(e => e.IsEmployeeWarehouse);

        /// <summary>
        /// 是否员工权限仓库
        /// </summary>
        public bool IsEmployeeWarehouse
        {
            get { return this.GetProperty(IsEmployeeWarehouseProperty); }
            set { this.SetProperty(IsEmployeeWarehouseProperty, value); }
        }
        #endregion

        #region 是否立库 IsAutomated
        /// <summary>
        /// 是否立库
        /// </summary>
        [Label("是否立库")]
        public static readonly Property<bool?> IsAutomatedProperty = P<WarehouseCriteria>.Register(e => e.IsAutomated);

        /// <summary>
        /// 是否立库
        /// </summary>
        public bool? IsAutomated
        {
            get { return this.GetProperty(IsAutomatedProperty); }
            set { this.SetProperty(IsAutomatedProperty, value); }
        }
        #endregion

        #region 是否过滤线边仓 IsLine
        /// <summary>
        /// 是否线边仓
        /// </summary>
        [Label("是否线边仓")]
        public static readonly Property<bool?> IsLineProperty = P<WarehouseCriteria>.Register(e => e.IsLine);

        /// <summary>
        /// 是否线边仓
        /// </summary>
        public bool? IsLine
        {
            get { return this.GetProperty(IsLineProperty); }
            set { this.SetProperty(IsLineProperty, value); }
        }
        #endregion

        /// <summary>
        /// 查询逻辑
        /// </summary>
        /// <returns>返回查询后的数据</returns>
        protected override EntityList Fetch()
        {
            var result = RT.Service.Resolve<WarehouseController>().GetWarehouseData(this);
            return result;
        }
    }
}
