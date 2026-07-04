using SIE.Domain;
using SIE.Inventory.Onhands;
using SIE.ObjectModel;
using System;

namespace SIE.Inventory.Onhands
{
    /// <summary>
    /// 选择库存添加查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("选择库存添加查询视图")]
    public class SelectOnhandCriteria : Criteria
    {
        #region 仓库ID WarehouseId
        /// <summary>
        /// 发货仓库ID
        /// </summary>
        [Label("仓库ID")]
        public static readonly Property<double> WarehouseIdProperty = P<SelectOnhandCriteria>.Register(e => e.WarehouseId);

        /// <summary>
        /// 供应商ID
        /// </summary>
        public double WarehouseId
        {
            get { return this.GetProperty(WarehouseIdProperty); }
            set { this.SetProperty(WarehouseIdProperty, value); }
        }
        #endregion

        #region 货主 ShipperCode
        /// <summary>
        /// 货主
        /// </summary>
        [Label("货主")]
        public static readonly Property<string> ShipperCodeProperty = P<SelectOnhandCriteria>.Register(e => e.ShipperCode);

        /// <summary>
        /// 货主
        /// </summary>
        public string ShipperCode
        {
            get { return this.GetProperty(ShipperCodeProperty); }
            set { this.SetProperty(ShipperCodeProperty, value); }
        }
        #endregion

        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<SelectOnhandCriteria>.Register(e => e.ItemCode);

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
        public static readonly Property<string> ItemNameProperty = P<SelectOnhandCriteria>.Register(e => e.ItemName);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
            set { this.SetProperty(ItemNameProperty, value); }
        }
        #endregion

        #region 库位编码 LocCode
        /// <summary>
        /// 库位编码
        /// </summary>
        [Label("库位编码")]
        public static readonly Property<string> LocCodeProperty = P<SelectOnhandCriteria>.Register(e => e.LocCode);

        /// <summary>
        /// 库位编码
        /// </summary>
        public string LocCode
        {
            get { return this.GetProperty(LocCodeProperty); }
            set { this.SetProperty(LocCodeProperty, value); }
        }
        #endregion

        #region LPN Lpn
        /// <summary>
        /// LPN
        /// </summary>
        [Label("LPN")]
        public static readonly Property<string> LpnProperty = P<SelectOnhandCriteria>.Register(e => e.Lpn);

        /// <summary>
        /// LPN
        /// </summary>
        public string Lpn
        {
            get { return this.GetProperty(LpnProperty); }
            set { this.SetProperty(LpnProperty, value); }
        }
        #endregion

        #region 批次号 LotCode
        /// <summary>
        /// 批次号
        /// </summary>
        [Label("批次号")]
        public static readonly Property<string> LotCodeProperty = P<SelectOnhandCriteria>.Register(e => e.LotCode);

        /// <summary>
        /// 批次号
        /// </summary>
        public string LotCode
        {
            get { return this.GetProperty(LotCodeProperty); }
            set { this.SetProperty(LotCodeProperty, value); }
        }
        #endregion

        #region 库存状态 OnhandState
        /// <summary>
        /// 库存状态
        /// </summary>
        [Label("库存状态")]
        public static readonly Property<OnhandState?> OnhandStateProperty = P<SelectOnhandCriteria>.Register(e => e.OnhandState);

        /// <summary>
        /// 库存状态
        /// </summary>
        public OnhandState? OnhandState
        {
            get { return this.GetProperty(OnhandStateProperty); }
            set { this.SetProperty(OnhandStateProperty, value); }
        }
        #endregion

        #region 项目号 ProjectNo
        /// <summary>
        /// 项目号
        /// </summary>
        [Label("项目号")]
        public static readonly Property<string> ProjectNoProperty = P<SelectOnhandCriteria>.Register(e => e.ProjectNo);

        /// <summary>
        /// 项目号
        /// </summary>
        public string ProjectNo
        {
            get { return GetProperty(ProjectNoProperty); }
            set { SetProperty(ProjectNoProperty, value); }
        }
        #endregion

        #region 任务号 TaskNo
        /// <summary>
        /// 任务号
        /// </summary>
        [Label("任务号")]
        public static readonly Property<string> TaskNoProperty = P<SelectOnhandCriteria>.Register(e => e.TaskNo);

        /// <summary>
        /// 任务号
        /// </summary>
        public string TaskNo
        {
            get { return GetProperty(TaskNoProperty); }
            set { SetProperty(TaskNoProperty, value); }
        }
        #endregion

        #region 忽略物料扩展属性 IsIgnoreItemExtProp
        /// <summary>
        /// 忽略物料扩展属性
        /// </summary>
        [Label("忽略物料扩展属性")]
        public static readonly Property<bool?> IsIgnoreItemExtPropProperty = P<SelectOnhandCriteria>.Register(e => e.IsIgnoreItemExtProp);

        /// <summary>
        /// 忽略物料扩展属性
        /// </summary>
        public bool? IsIgnoreItemExtProp
        {
            get { return GetProperty(IsIgnoreItemExtPropProperty); }
            set { SetProperty(IsIgnoreItemExtPropProperty, value); }
        }
        #endregion

        #region 物料扩展属性 ItemExtProp
        /// <summary>
        /// 物料扩展属性
        /// </summary>
        [Label("物料扩展属性")]
        public static readonly Property<string> ItemExtPropProperty = P<SelectOnhandCriteria>.Register(e => e.ItemExtProp);

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtProp
        {
            get { return this.GetProperty(ItemExtPropProperty); }
            set { this.SetProperty(ItemExtPropProperty, value); }
        }
        #endregion

        #region 是否拣货库位 IsPick
        /// <summary>
        /// 是否拣货库位
        /// </summary>
        [Label("是否拣货库位")]
        public static readonly Property<bool> IsPickProperty = P<SelectOnhandCriteria>.Register(e => e.IsPick);

        /// <summary>
        /// 是否拣货库位
        /// </summary>
        public bool IsPick
        {
            get { return this.GetProperty(IsPickProperty); }
            set { this.SetProperty(IsPickProperty, value); }
        }
        #endregion

        #region 库区 AreaId
        /// <summary>
        /// 库区
        /// </summary>
        [Label("库区")]
        public static readonly Property<double?> AreaIdProperty = P<SelectOnhandCriteria>.Register(e => e.AreaId);

        /// <summary>
        /// 库区
        /// </summary>
        public double? AreaId
        {
            get { return this.GetProperty(AreaIdProperty); }
            set { this.SetProperty(AreaIdProperty, value); }
        }
        #endregion

        #region 自动分配数量 AutoExpectQty
        /// <summary>
        /// 自动分配数量
        /// </summary>
        [Label("自动分配数量")]
        public static readonly Property<bool?> AutoExpectQtyProperty = P<SelectOnhandCriteria>.Register(e => e.AutoExpectQty);

        /// <summary>
        /// 自动分配数量
        /// </summary>
        public bool? AutoExpectQty
        {
            get { return this.GetProperty(AutoExpectQtyProperty); }
            set { this.SetProperty(AutoExpectQtyProperty, value); }
        }
        #endregion

        /// <summary>
        /// 查询逻辑
        /// </summary>
        /// <returns>返回查询后的数据</returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<InvOnhandController>().GetSelectOnhandViewModels(this);
        }
    }
}