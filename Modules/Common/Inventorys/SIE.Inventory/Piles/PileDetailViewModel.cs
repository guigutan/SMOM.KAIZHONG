using SIE.Domain;
using SIE.Core.Enums;
using SIE.Inventory.Onhands;
using SIE.Items;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Inventory.Piles
{
    /// <summary>
    /// 垛表物料明细
    /// </summary>
    [RootEntity, Serializable]
    [Label("物料明细")]
    public class PileDetailViewModel : ViewModel
    {
        #region 条码 Sn
        /// <summary>
        /// 条码
        /// </summary>
        [Label("条码")]
        public static readonly Property<string> SnProperty = P<PileDetailViewModel>.Register(e => e.Sn);

        /// <summary>
        /// 条码
        /// </summary>
        public string Sn
        {
            get { return this.GetProperty(SnProperty); }
            set { this.SetProperty(SnProperty, value); }
        }
        #endregion

        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<PileDetailViewModel>.Register(e => e.ItemCode);

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
        public static readonly Property<string> ItemNameProperty = P<PileDetailViewModel>.Register(e => e.ItemName);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
            set { this.SetProperty(ItemNameProperty, value); }
        }
        #endregion

        #region 规格 SpecificationModel
        /// <summary>
        /// 规格
        /// </summary>
        [Label("规格")]
        public static readonly Property<string> SpecificationModelProperty = P<PileDetailViewModel>.Register(e => e.SpecificationModel);

        /// <summary>
        /// 规格
        /// </summary>
        public string SpecificationModel
        {
            get { return this.GetProperty(SpecificationModelProperty); }
            set { this.SetProperty(SpecificationModelProperty, value); }
        }
        #endregion

        #region 数量 Qty
        /// <summary>
        /// 数量
        /// </summary>
        [Label("数量")]
		public static readonly Property<decimal> QtyProperty = P<PileDetailViewModel>.Register(e => e.Qty);

		/// <summary>
		/// 数量
		/// </summary>
		public decimal Qty
		{
			get { return GetProperty(QtyProperty); }
			set { SetProperty(QtyProperty, value); }
		}
		#endregion

		#region 单位 ItemUnitName
		/// <summary>
		/// 单位
		/// </summary>
		[Label("单位")]
        public static readonly Property<string> ItemUnitNameProperty = P<PileDetailViewModel>.Register(e => e.ItemUnitName);

        /// <summary>
        /// 单位
        /// </summary>
        public string ItemUnitName
        {
            get { return this.GetProperty(ItemUnitNameProperty); }
            set { this.SetProperty(ItemUnitNameProperty, value); }
        }
        #endregion

        #region 物料扩展属性 ItemExtPropName
        /// <summary>
        /// 物料扩展属性
        /// </summary>
        [Label("物料扩展属性")]
		public static readonly Property<string> ItemExtPropNameProperty = P<PileDetailViewModel>.Register(e => e.ItemExtPropName);

		/// <summary>
		/// 物料扩展属性
		/// </summary>
		public string ItemExtPropName
		{
			get { return GetProperty(ItemExtPropNameProperty); }
			set { SetProperty(ItemExtPropNameProperty, value); }
		}
        #endregion

        #region 批次 LotCode
        /// <summary>
        /// 批次
        /// </summary>
        [Label("批次号")]
        public static readonly Property<string> LotCodeProperty = P<PileDetailViewModel>.Register(e => e.LotCode);

        /// <summary>
        /// 批次
        /// </summary>
        public string LotCode
        {
            get { return this.GetProperty(LotCodeProperty); }
            set { this.SetProperty(LotCodeProperty, value); }
        }
        #endregion

        #region 仓库 Warehouse
        /// <summary>
        /// 仓库
        /// </summary>
        [Label("仓库")]
        public static readonly Property<string> WarehouseProperty = P<PileDetailViewModel>.Register(e => e.Warehouse);

        /// <summary>
        /// 仓库
        /// </summary>
        public string Warehouse
        {
            get { return this.GetProperty(WarehouseProperty); }
            set { this.SetProperty(WarehouseProperty, value); }
        }
        #endregion

        #region 库区 StorageArea
        /// <summary>
        /// 库区
        /// </summary>
        [Label("库区")]
        public static readonly Property<string> StorageAreaProperty = P<PileDetailViewModel>.Register(e => e.StorageArea);

        /// <summary>
        /// 库区
        /// </summary>
        public string StorageArea
        {
            get { return this.GetProperty(StorageAreaProperty); }
            set { this.SetProperty(StorageAreaProperty, value); }
        }
        #endregion

        #region 库位 StorageLocation
        /// <summary>
        /// 库位
        /// </summary>
        [Label("库位")]
        public static readonly Property<string> StorageLocationProperty = P<PileDetailViewModel>.Register(e => e.StorageLocation);

        /// <summary>
        /// 库位
        /// </summary>
        public string StorageLocation
        {
            get { return this.GetProperty(StorageLocationProperty); }
            set { this.SetProperty(StorageLocationProperty, value); }
        }
        #endregion

        #region 货主 StorerCode
        /// <summary>
        /// 货主
        /// </summary>
        [Label("货主")]
		public static readonly Property<string> StorerCodeProperty = P<PileDetailViewModel>.Register(e => e.StorerCode);

		/// <summary>
		/// 货主
		/// </summary>
		public string StorerCode
		{
			get { return GetProperty(StorerCodeProperty); }
			set { SetProperty(StorerCodeProperty, value); }
		}
		#endregion

		#region 项目号 ProjectNo
		/// <summary>
		/// 项目号
		/// </summary>
		[Label("项目号")]
		public static readonly Property<string> ProjectNoProperty = P<PileDetailViewModel>.Register(e => e.ProjectNo);

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
		public static readonly Property<string> TaskNoProperty = P<PileDetailViewModel>.Register(e => e.TaskNo);

		/// <summary>
		/// 任务号
		/// </summary>
		public string TaskNo
		{
			get { return GetProperty(TaskNoProperty); }
			set { SetProperty(TaskNoProperty, value); }
		}
		#endregion

		#region 库存状态 OnhandState
		/// <summary>
		/// 库存状态
		/// </summary>
		[Label("库存状态")]
		public static readonly Property<OnhandState> OnhandStateProperty = P<PileDetailViewModel>.Register(e => e.OnhandState);

		/// <summary>
		/// 库存状态
		/// </summary>
		public OnhandState OnhandState
		{
			get { return GetProperty(OnhandStateProperty); }
			set { SetProperty(OnhandStateProperty, value); }
		}
		#endregion

		#region 物料状态 ItemState
		/// <summary>
		/// 物料状态
		/// </summary>
		[Label("物料状态")]
		public static readonly Property<ItemState> ItemStateProperty = P<PileDetailViewModel>.Register(e => e.ItemState);

		/// <summary>
		/// 物料状态
		/// </summary>
		public ItemState ItemState
		{
			get { return GetProperty(ItemStateProperty); }
			set { SetProperty(ItemStateProperty, value); }
		}
		#endregion
	}
}
