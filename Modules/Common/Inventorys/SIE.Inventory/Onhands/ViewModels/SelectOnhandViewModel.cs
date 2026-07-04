using SIE.Domain;
using SIE.Inventory.Commom;
using SIE.Inventory.Onhands;
using SIE.Items;
using SIE.ObjectModel;
using SIE.Warehouses;
using System;

namespace SIE.Inventory.Onhands
{
    /// <summary>
    /// 选择库存添加ViewModel
    /// </summary>
    [RootEntity, Serializable]   
    [Label("选择库存添加")]
    public class BaseSelectViewModel : SecondUnitViewModel
    {
        #region 发货仓库ID WarehouseId
        /// <summary>
        /// 发货仓库ID
        /// </summary>
        [Label("发货仓库ID")]
        public static readonly Property<double> WarehouseIdProperty = P<BaseSelectViewModel>.Register(e => e.WarehouseId);

        /// <summary>
        /// 发货仓库ID
        /// </summary>
        public double WarehouseId
        {
            get { return this.GetProperty(WarehouseIdProperty); }
            set { this.SetProperty(WarehouseIdProperty, value); }
        }
        #endregion

        #region 分配数 ExpectQty
        /// <summary>
        /// 分配数
        /// </summary>
        [Label("分配数")]
        public static readonly Property<decimal?> ExpectQtyProperty = P<BaseSelectViewModel>.Register(e => e.ExpectQty);

        /// <summary>
        /// 分配数
        /// </summary>
        public decimal? ExpectQty
        {
            get { return this.GetProperty(ExpectQtyProperty); }
            set { this.SetProperty(ExpectQtyProperty, value); }
        }
        #endregion

        #region 可用量 AvailableQty
        /// <summary>
        /// 可用量
        /// </summary>
        [Label("可用量")]
        public static readonly Property<decimal> AvailableQtyProperty = P<BaseSelectViewModel>.Register(e => e.AvailableQty);

        /// <summary>
        /// 可用量
        /// </summary>
        public decimal AvailableQty
        {
            get { return GetProperty(AvailableQtyProperty); }
            set { SetProperty(AvailableQtyProperty, value); }
        }
        #endregion

        #region 物料ID ItemId
        /// <summary>
        /// 物料ID
        /// </summary>
        [Label("物料ID")]
        public static readonly Property<double> ItemIdProperty = P<BaseSelectViewModel>.Register(e => e.ItemId);

        /// <summary>
        /// 物料ID
        /// </summary>
        public double ItemId
        {
            get { return this.GetProperty(ItemIdProperty); }
            set { this.SetProperty(ItemIdProperty, value); }
        }
        #endregion

        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<BaseSelectViewModel>.Register(e => e.ItemCode);

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
        public static readonly Property<string> ItemNameProperty = P<BaseSelectViewModel>.Register(e => e.ItemName);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
            set { this.SetProperty(ItemNameProperty, value); }
        }
        #endregion

        #region 物料扩展属性 ItemExtProp
        /// <summary>
        /// 物料扩展属性
        /// </summary>
        [Label("物料扩展属性")]
        public static readonly Property<string> ItemExtPropProperty = P<BaseSelectViewModel>.Register(e => e.ItemExtProp);

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtProp
        {
            get { return this.GetProperty(ItemExtPropProperty); }
            set { this.SetProperty(ItemExtPropProperty, value); }
        }
        #endregion

        #region 物料扩展属性名称 ItemExtPropName
        /// <summary>
        /// 物料扩展属性名称
        /// </summary>
        [Label("物料扩展属性名称")]
        public static readonly Property<string> ItemExtPropNameProperty = P<BaseSelectViewModel>.Register(e => e.ItemExtPropName);

        /// <summary>
        /// 物料扩展属性名称
        /// </summary>
        public string ItemExtPropName
        {
            get { return this.GetProperty(ItemExtPropNameProperty); }
            set { this.SetProperty(ItemExtPropNameProperty, value); }
        }
        #endregion

        #region 单位(主) ItemUnitName
        /// <summary>
        /// 单位(主)
        /// </summary>
        [Label("单位(主)")]
        public static readonly Property<string> ItemUnitNameProperty = P<BaseSelectViewModel>.Register(e => e.ItemUnitName);

        /// <summary>
        /// 单位(主)
        /// </summary>
        public string ItemUnitName
        {
            get { return this.GetProperty(ItemUnitNameProperty); }
            set { this.SetProperty(ItemUnitNameProperty, value); }
        }
        #endregion

        #region 库位编码 StorageLocation
        /// <summary>
        /// 库位编码
        /// </summary>
        [Label("库位编码")]
        public static readonly IRefIdProperty StorageLocationIdProperty = P<BaseSelectViewModel>.RegisterRefId(e => e.StorageLocationId, ReferenceType.Normal);

        /// <summary>
        /// 库位编码
        /// </summary>
        public double StorageLocationId
        {
            get { return (double)GetRefId(StorageLocationIdProperty); }
            set { SetRefId(StorageLocationIdProperty, value); }
        }

        /// <summary>
        /// 库位编码
        /// </summary>
        public static readonly RefEntityProperty<StorageLocation> StorageLocationProperty = P<BaseSelectViewModel>.RegisterRef(e => e.StorageLocation, StorageLocationIdProperty);

        /// <summary>
        /// 库位编码
        /// </summary>
        public StorageLocation StorageLocation
        {
            get { return GetRefEntity(StorageLocationProperty); }
            set { SetRefEntity(StorageLocationProperty, value); }
        }
        #endregion

        #region 库位编码 LocCode
        /// <summary>
        /// 库位编码
        /// </summary>
        [Label("库位编码")]
        public static readonly Property<string> LocCodeProperty = P<BaseSelectViewModel>.RegisterView(e => e.LocCode, p => p.StorageLocation.Code);

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
        public static readonly Property<string> LpnProperty = P<BaseSelectViewModel>.Register(e => e.Lpn);

        /// <summary>
        /// LPN
        /// </summary>
        public string Lpn
        {
            get { return this.GetProperty(LpnProperty); }
            set { this.SetProperty(LpnProperty, value); }
        }
        #endregion

        #region 批次 Lot
        /// <summary>
        /// 批次
        /// </summary>
        [Label("批次")]
        public static readonly IRefIdProperty LotIdProperty =
            P<BaseSelectViewModel>.RegisterRefId(e => e.LotId, ReferenceType.Normal);

        /// <summary>
        /// 批次
        /// </summary>
        public double LotId
        {
            get { return (double)this.GetRefId(LotIdProperty); }
            set { this.SetRefId(LotIdProperty, value); }
        }

        /// <summary>
        /// 批次
        /// </summary>
        public static readonly RefEntityProperty<Lot> LotProperty =
            P<BaseSelectViewModel>.RegisterRef(e => e.Lot, LotIdProperty);

        /// <summary>
        /// 批次
        /// </summary>
        public Lot Lot
        {
            get { return this.GetRefEntity(LotProperty); }
            set { this.SetRefEntity(LotProperty, value); }
        }
        #endregion

        #region 批次号 LotCode
        /// <summary>
        /// 批次号
        /// </summary>
        [Label("批次号")]
        public static readonly Property<string> LotCodeProperty = P<BaseSelectViewModel>.Register(e => e.LotCode);

        /// <summary>
        /// 批次
        /// </summary>
        public string LotCode
        {
            get { return GetProperty(LotCodeProperty); }
            set { SetProperty(LotCodeProperty, value); }
        }
        #endregion

        #region 库存状态 OnhandState
        /// <summary>
        /// 库存状态
        /// </summary>
        [Label("库存状态")]
        public static readonly Property<OnhandState> OnhandStateProperty = P<BaseSelectViewModel>.Register(e => e.OnhandState);

        /// <summary>
        /// 库存状态
        /// </summary>
        public OnhandState OnhandState
        {
            get { return GetProperty(OnhandStateProperty); }
            set { SetProperty(OnhandStateProperty, value); }
        }
        #endregion

        #region 货主 StorerCode
        /// <summary>
        /// 货主
        /// </summary>
        [Label("货主")]
        public static readonly Property<string> StorerCodeProperty = P<BaseSelectViewModel>.Register(e => e.StorerCode);

        /// <summary>
        /// 货主
        /// </summary>
        public string StorerCode
        {
            get { return this.GetProperty(StorerCodeProperty); }
            set { this.SetProperty(StorerCodeProperty, value); }
        }
        #endregion

        #region 项目号 ProjectNo
        /// <summary>
        /// 项目号
        /// </summary>
        [Label("项目号")]
        public static readonly Property<string> ProjectNoProperty = P<BaseSelectViewModel>.Register(e => e.ProjectNo);

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
        public static readonly Property<string> TaskNoProperty = P<BaseSelectViewModel>.Register(e => e.TaskNo);

        /// <summary>
        /// 任务号
        /// </summary>
        public string TaskNo
        {
            get { return GetProperty(TaskNoProperty); }
            set { SetProperty(TaskNoProperty, value); }
        }
        #endregion

        #region 生产日期 LotAtt01
        /// <summary>
        /// 生产日期
        /// </summary>
        [Label("生产日期")]
        public static readonly Property<DateTime?> LotAtt01Property = P<BaseSelectViewModel>.RegisterView(e => e.LotAtt01, p => p.Lot.LotAtt01);

        /// <summary>
        /// 生产日期
        /// </summary>
        public DateTime? LotAtt01
        {
            get { return this.GetProperty(LotAtt01Property); }
            set { this.SetProperty(LotAtt01Property, value); }
        }
        #endregion

        #region 失效日期 LotAtt02
        /// <summary>
        /// 失效日期
        /// </summary>
        [Label("失效日期")]
        public static readonly Property<DateTime?> LotAtt02Property = P<BaseSelectViewModel>.RegisterView(e => e.LotAtt02, p => p.Lot.LotAtt02);

        /// <summary>
        /// 失效日期
        /// </summary>
        public DateTime? LotAtt02
        {
            get { return this.GetProperty(LotAtt02Property); }
            set { this.SetProperty(LotAtt02Property, value); }
        }
        #endregion

        #region 收货日期 LotAtt03
        /// <summary>
        /// 收货日期
        /// </summary>
        [Label("收货日期")]
        public static readonly Property<DateTime?> LotAtt03Property = P<BaseSelectViewModel>.RegisterView(e => e.LotAtt03, p => p.Lot.LotAtt03);

        /// <summary>
        /// 收货日期
        /// </summary>
        public DateTime? LotAtt03
        {
            get { return this.GetProperty(LotAtt03Property); }
            set
            {
                this.SetProperty(LotAtt03Property, value);
            }
        }
        #endregion

        #region 生产批次 LotAtt04
        /// <summary>
        /// 生产批次
        /// </summary>
        [Label("生产批次")]
        public static readonly Property<string> LotAtt04Property = P<BaseSelectViewModel>.RegisterView(e => e.LotAtt04, p => p.Lot.LotAtt04);

        /// <summary>
        /// 生产批次
        /// </summary>
        public string LotAtt04
        {
            get { return this.GetProperty(LotAtt04Property); }
            set
            {
                this.SetProperty(LotAtt04Property, value);
            }
        }
        #endregion

        #region 复检次数 LotAtt05
        /// <summary>
        /// 复检次数
        /// </summary>
        [Label("复检次数")]
        public static readonly Property<decimal?> LotAtt05Property = P<BaseSelectViewModel>.RegisterView(e => e.LotAtt05, p => p.Lot.LotAtt05);

        /// <summary>
        /// 复检次数
        /// </summary>
        public decimal? LotAtt05
        {
            get { return this.GetProperty(LotAtt05Property); }
            set
            {
                this.SetProperty(LotAtt05Property, value);
            }
        }
        #endregion

        #region 是否特采 LotAtt07
        /// <summary>
        /// 是否特采
        /// </summary>
        [Label("是否特采")]
        public static readonly Property<bool?> LotAtt07Property = P<BaseSelectViewModel>.RegisterView(e => e.LotAtt07, p => p.Lot.LotAtt07);

        /// <summary>
        /// 是否特采
        /// </summary>
        public bool? LotAtt07
        {
            get { return this.GetProperty(LotAtt07Property); }
            set
            {
                this.SetProperty(LotAtt07Property, value);
            }
        }
        #endregion

        #region 现有量 Qty
        /// <summary>
        /// 现有量
        /// </summary>
        [Label("现有量")]
        public static readonly Property<decimal> QtyProperty = P<BaseSelectViewModel>.Register(e => e.Qty);

        /// <summary>
        /// 现有量
        /// </summary>
        public decimal Qty
        {
            get { return GetProperty(QtyProperty); }
            set { SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 冻结量 FreezingQty
        /// <summary>
        /// 冻结量
        /// </summary>
        [Label("冻结量")]
        public static readonly Property<decimal> FreezingQtyProperty = P<BaseSelectViewModel>.Register(e => e.FreezingQty);

        /// <summary>
        /// 冻结量
        /// </summary>
        public decimal FreezingQty
        {
            get { return GetProperty(FreezingQtyProperty); }
            set { SetProperty(FreezingQtyProperty, value); }
        }
        #endregion

        #region 分配量 AllottedQty
        /// <summary>
        /// 分配量
        /// </summary>
        [Label("分配量")]
        public static readonly Property<decimal> AllottedQtyProperty = P<BaseSelectViewModel>.Register(e => e.AllottedQty);

        /// <summary>
        /// 分配量
        /// </summary>
        public decimal AllottedQty
        {
            get { return GetProperty(AllottedQtyProperty); }
            set { SetProperty(AllottedQtyProperty, value); }
        }
        #endregion

        #region 创建人 CreateBy
        /// <summary>
        /// 创建人
        /// </summary>
        [Label("创建人")]
        public static readonly Property<string> CreateByProperty = P<BaseSelectViewModel>.Register(e => e.CreateBy);

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateBy
        {
            get { return this.GetProperty(CreateByProperty); }
            set { this.SetProperty(CreateByProperty, value); }
        }
        #endregion

        #region 创建时间 CreateDate
        /// <summary>
        /// 创建人
        /// </summary>
        [Label("创建时间")]
        public static readonly Property<DateTime> CreateDateProperty = P<BaseSelectViewModel>.Register(e => e.CreateDate);

        /// <summary>
        /// 创建人
        /// </summary>
        public DateTime CreateDate
        {
            get { return this.GetProperty(CreateDateProperty); }
            set { this.SetProperty(CreateDateProperty, value); }
        }
        #endregion

        #region 修改人 UpdateBy
        /// <summary>
        /// 修改人
        /// </summary>
        [Label("修改人")]
        public static readonly Property<string> UpdateByProperty = P<BaseSelectViewModel>.Register(e => e.UpdateBy);

        /// <summary>
        /// 修改人
        /// </summary>
        public string UpdateBy
        {
            get { return this.GetProperty(UpdateByProperty); }
            set { this.SetProperty(UpdateByProperty, value); }
        }
        #endregion

        #region 修改时间 UpdateDate
        /// <summary>
        /// 修改时间
        /// </summary>
        [Label("修改时间")]
        public static readonly Property<DateTime> UpdateDateProperty = P<BaseSelectViewModel>.Register(e => e.UpdateDate);

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime UpdateDate
        {
            get { return this.GetProperty(UpdateDateProperty); }
            set { this.SetProperty(UpdateDateProperty, value); }
        }
        #endregion

        #region 库存ID OnhandId
        /// <summary>
        /// 库存ID
        /// </summary>
        [Label("库存ID")]
        public static readonly Property<double> OnhandIdProperty = P<BaseSelectViewModel>.Register(e => e.OnhandId);

        /// <summary>
        /// 库存ID
        /// </summary>
        public double OnhandId
        {
            get { return GetProperty(OnhandIdProperty); }
            set { SetProperty(OnhandIdProperty, value); }
        }
        #endregion

        #region 分配数(辅) SecondExpectQty
        /// <summary>
        /// 分配数(辅)
        /// </summary>
        [Label("分配数(辅)")]
        public static readonly Property<decimal> SecondExpectQtyProperty = P<BaseSelectViewModel>.Register(e => e.SecondExpectQty);

        /// <summary>
        /// 分配数(辅)
        /// </summary>
        public decimal SecondExpectQty
        {
            get { return this.GetProperty(SecondExpectQtyProperty); }
            set { this.SetProperty(SecondExpectQtyProperty, value); }
        }
        #endregion

        #region 物料单位Id ItemUnitId
        /// <summary>
        /// 物料单位Id
        /// </summary>
        [Label("物料单位Id")]
        public static readonly Property<double> ItemUnitIdProperty = P<BaseSelectViewModel>.Register(e => e.ItemUnitId);

        /// <summary>
        /// 物料单位Id
        /// </summary>
        public double ItemUnitId
        {
            get { return this.GetProperty(ItemUnitIdProperty); }
            set { this.SetProperty(ItemUnitIdProperty, value); }
        }
        #endregion


    }         
}