using System;
using SIE.Domain;
using SIE.ERPInterface.Smom.InventoryControl.datas;
using SIE.MetaModel;
using SIE.ObjectModel;

namespace SIE.ERPInterface.Smom.InventoryControl
{
    /// <summary>
    /// 库存对照表设置
    /// </summary>
    [RootEntity, Serializable]
    [Label("库存对照表设置")]
    public class InventoryControlSetting:DataEntity
    {
        #region 物料 IsItem
        /// <summary>
        /// 物料
        /// </summary>
        [Label("物料")]
        public static readonly Property<bool?> IsItemProperty = P<InventoryControlSetting>.Register(e => e.IsItem);

        /// <summary>
        /// 物料
        /// </summary>
        public bool? IsItem
        {
            get { return this.GetProperty(IsItemProperty); }
            set { this.SetProperty(IsItemProperty, value); }
        }
        #endregion

        #region 批次 IsLot
        /// <summary>
        /// 批次
        /// </summary>
        [Label("批次")]
        public static readonly Property<bool?> IsLotProperty = P<InventoryControlSetting>.Register(e => e.IsLot);

        /// <summary>
        /// 批次
        /// </summary>
        public bool? IsLot
        {
            get { return this.GetProperty(IsLotProperty); }
            set { this.SetProperty(IsLotProperty, value); }
        }
        #endregion

        #region 仓库 IsWareHouse
        /// <summary>
        /// 仓库
        /// </summary>
        [Label("仓库")]
        public static readonly Property<bool?> IsWareHouseProperty = P<InventoryControlSetting>.Register(e => e.IsWareHouse);

        /// <summary>
        /// 仓库
        /// </summary>
        public bool? IsWareHouse
        {
            get { return this.GetProperty(IsWareHouseProperty); }
            set { this.SetProperty(IsWareHouseProperty, value); }
        }
        #endregion

        #region 合格库存 IsOkInv
        /// <summary>
        /// 合格库存
        /// </summary>
        [Label("合格库存")]
        public static readonly Property<bool?> IsOkInvProperty = P<InventoryControlSetting>.Register(e => e.IsOkInv);

        /// <summary>
        /// 合格库存
        /// </summary>
        public bool? IsOkInv
        {
            get { return this.GetProperty(IsOkInvProperty); }
            set { this.SetProperty(IsOkInvProperty, value); }
        }
        #endregion

        #region 不合格库存 IsNgInv
        /// <summary>
        /// 不合格库存
        /// </summary>
        [Label("不合格库存")]
        public static readonly Property<bool?> IsNgInvProperty = P<InventoryControlSetting>.Register(e => e.IsNgInv);

        /// <summary>
        /// 不合格库存
        /// </summary>
        public bool? IsNgInv
        {
            get { return this.GetProperty(IsNgInvProperty); }
            set { this.SetProperty(IsNgInvProperty, value); }
        }
        #endregion

        #region 仓库:ERP子库 EbsToWarehouse
        /// <summary>
        /// 仓库:ERP子库
        /// </summary>
        [Label("仓库:ERP子库")]
        public static readonly Property<EbsToWareHouse> EbsToWarehouseProperty = P<InventoryControlSetting>.Register(e => e.EbsToWarehouse);

        /// <summary>
        /// 仓库:ERP子库
        /// </summary>
        public EbsToWareHouse EbsToWarehouse
        {
            get { return GetProperty(EbsToWarehouseProperty); }
            set { SetProperty(EbsToWarehouseProperty, value); }
        }
        #endregion

        #region ERP批次对照
        /// <summary>
        /// ERP批次对照
        /// </summary>
        [Label("ERP批次对照")]
        public static readonly Property<EbsToLot> EbsToLotProperty = P<InventoryControlSetting>.Register(e => e.EbsToLot);

        /// <summary>
        /// 仓库:ERP子库
        /// </summary>
        public EbsToLot EbsToLot
        {
            get { return GetProperty(EbsToLotProperty); }
            set { SetProperty(EbsToLotProperty, value); }
        }
        #endregion

        //#region 员工 CloseBy
        ///// <summary>
        ///// 员工
        ///// </summary>
        //[Label("员工")]
        //public static readonly IRefIdProperty EmployeeIdProperty =
        //    P<InventoryControlSetting>.RegisterRefId(e => e.EmployeeId, ReferenceType.Normal);

        ///// <summary>
        ///// 员工
        ///// </summary>
        //public double? EmployeeId
        //{
        //    get { return (double?)this.GetRefNullableId(EmployeeIdProperty); }
        //    set { this.SetRefNullableId(EmployeeIdProperty, value); }
        //}

        ///// <summary>
        ///// 员工
        ///// </summary>
        //public static readonly RefEntityProperty<Employee> EmployeeProperty =
        //    P<InventoryControlSetting>.RegisterRef(e => e.Employee, EmployeeIdProperty);

        ///// <summary>
        ///// 员工
        ///// </summary>
        //public Employee Employee
        //{
        //    get { return this.GetRefEntity(EmployeeProperty); }
        //    set { this.SetRefEntity(EmployeeProperty, value); }
        //}
        //#endregion
    }

    /// <summary>
    /// ASN明细 实体配置
    /// </summary>
    internal class InventoryControlSettingConfig : EntityConfig<InventoryControlSetting>
    {
        

        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("INV_CRO_SETTING").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
