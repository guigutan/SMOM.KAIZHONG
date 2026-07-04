using SIE.Domain;
using SIE.EMS.SpareParts;
using SIE.EMS.SpareParts.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.InventoryTasks
{
	/// <summary>
	/// 盘点任务备件汇总
	/// </summary>
	[ChildEntity, Serializable]	
	[Label("盘点任务备件汇总")]
	public partial class InventoryTaskSparePart : DataEntity
	{
		#region 良品数 GoodQty
		/// <summary>
		/// 良品数
		/// </summary>
		[Label("良品数")]
		public static readonly Property<int> GoodQtyProperty = P<InventoryTaskSparePart>.Register(e => e.GoodQty);

		/// <summary>
		/// 良品数
		/// </summary>
		public int GoodQty
		{
			get { return GetProperty(GoodQtyProperty); }
			set { SetProperty(GoodQtyProperty, value); }
		}
		#endregion

		#region 不良品数 NgQty
		/// <summary>
		/// 不良品数
		/// </summary>
		[Label("不良品数")]
		public static readonly Property<int> NgQtyProperty = P<InventoryTaskSparePart>.Register(e => e.NgQty);

		/// <summary>
		/// 不良品数
		/// </summary>
		public int NgQty
		{
			get { return GetProperty(NgQtyProperty); }
			set { SetProperty(NgQtyProperty, value); }
		}
		#endregion

		#region 总数量 Total
		/// <summary>
		/// 总数量
		/// </summary>
		[Label("总数量")]
		public static readonly Property<int> TotalProperty = P<InventoryTaskSparePart>.Register(e => e.Total);

		/// <summary>
		/// 总数量
		/// </summary>
		public int Total
		{
			get { return GetProperty(TotalProperty); }
			set { SetProperty(TotalProperty, value); }
		}
		#endregion

		#region 初盘良品数 FirstGood
		/// <summary>
		/// 初盘良品数
		/// </summary>
		[Label("初盘良品数")]
		public static readonly Property<int?> FirstGoodProperty = P<InventoryTaskSparePart>.Register(e => e.FirstGood);

		/// <summary>
		/// 初盘良品数
		/// </summary>
		public int? FirstGood
		{
			get { return GetProperty(FirstGoodProperty); }
			set { SetProperty(FirstGoodProperty, value); }
		}
		#endregion

		#region 初盘不良品数 FirstNg
		/// <summary>
		/// 初盘不良品数
		/// </summary>
		[Label("初盘不良品数")]
		public static readonly Property<int?> FirstNgProperty = P<InventoryTaskSparePart>.Register(e => e.FirstNg);

		/// <summary>
		/// 初盘不良品数
		/// </summary>
		public int? FirstNg
		{
			get { return GetProperty(FirstNgProperty); }
			set { SetProperty(FirstNgProperty, value); }
		}
		#endregion

		#region 初盘总数 FirstTotal
		/// <summary>
		/// 初盘总数
		/// </summary>
		[Label("初盘总数")]
		public static readonly Property<int?> FirstTotalProperty = P<InventoryTaskSparePart>.Register(e => e.FirstTotal);

		/// <summary>
		/// 初盘总数
		/// </summary>
		public int? FirstTotal
		{
			get { return GetProperty(FirstTotalProperty); }
			set { SetProperty(FirstTotalProperty, value); }
		}
		#endregion

		#region 初盘差异数 FirstDiff
		/// <summary>
		/// 初盘差异数
		/// </summary>
		[Label("初盘差异数")]
		public static readonly Property<int?> FirstDiffProperty = P<InventoryTaskSparePart>.Register(e => e.FirstDiff);

		/// <summary>
		/// 初盘差异数
		/// </summary>
		public int? FirstDiff
		{
			get { return GetProperty(FirstDiffProperty); }
			set { SetProperty(FirstDiffProperty, value); }
		}
		#endregion

		#region 复盘良品数 SecondGoodQty
		/// <summary>
		/// 复盘良品数
		/// </summary>
		[Label("复盘良品数")]
		public static readonly Property<int?> SecondGoodQtyProperty = P<InventoryTaskSparePart>.Register(e => e.SecondGoodQty);

		/// <summary>
		/// 复盘良品数
		/// </summary>
		public int? SecondGoodQty
		{
			get { return GetProperty(SecondGoodQtyProperty); }
			set { SetProperty(SecondGoodQtyProperty, value); }
		}
		#endregion

		#region 复盘不良品数 SecondNgQty
		/// <summary>
		/// 复盘不良品数
		/// </summary>
		[Label("复盘不良品数")]
		public static readonly Property<int?> SecondNgQtyProperty = P<InventoryTaskSparePart>.Register(e => e.SecondNgQty);

		/// <summary>
		/// 复盘不良品数
		/// </summary>
		public int? SecondNgQty
		{
			get { return GetProperty(SecondNgQtyProperty); }
			set { SetProperty(SecondNgQtyProperty, value); }
		}
		#endregion

		#region 复盘总数 SecondTotal
		/// <summary>
		/// 复盘总数
		/// </summary>
		[Label("复盘总数")]
		public static readonly Property<int?> SecondTotalProperty = P<InventoryTaskSparePart>.Register(e => e.SecondTotal);

		/// <summary>
		/// 复盘总数
		/// </summary>
		public int? SecondTotal
		{
			get { return GetProperty(SecondTotalProperty); }
			set { SetProperty(SecondTotalProperty, value); }
		}
		#endregion

		#region 复盘差异数 SecondDiff
		/// <summary>
		/// 复盘差异数
		/// </summary>
		[Label("复盘差异数")]
		public static readonly Property<int?> SecondDiffProperty = P<InventoryTaskSparePart>.Register(e => e.SecondDiff);

		/// <summary>
		/// 复盘差异数
		/// </summary>
		public int? SecondDiff
		{
			get { return GetProperty(SecondDiffProperty); }
			set { SetProperty(SecondDiffProperty, value); }
		}
		#endregion

		#region 备件 SparePart
		/// <summary>
		/// 备件Id
		/// </summary>
		[Label("备件")]
		public static readonly IRefIdProperty SparePartIdProperty = P<InventoryTaskSparePart>.RegisterRefId(e => e.SparePartId, ReferenceType.Normal);

		/// <summary>
		/// 备件Id
		/// </summary>
		public double SparePartId
		{
			get { return (double)GetRefId(SparePartIdProperty); }
			set { SetRefId(SparePartIdProperty, value); }
		}

		/// <summary>
		/// 备件
		/// </summary>
		public static readonly RefEntityProperty<SparePart> SparePartProperty = P<InventoryTaskSparePart>.RegisterRef(e => e.SparePart, SparePartIdProperty);

		/// <summary>
		/// 备件
		/// </summary>
		public SparePart SparePart
		{
			get { return GetRefEntity(SparePartProperty); }
			set { SetRefEntity(SparePartProperty, value); }
		}
		#endregion

		#region 盘点任务 InventoryTask
		/// <summary>
		/// 盘点任务Id
		/// </summary>
		[Label("盘点任务")]
		public static readonly IRefIdProperty InventoryTaskIdProperty = P<InventoryTaskSparePart>.RegisterRefId(e => e.InventoryTaskId, ReferenceType.Parent);

		/// <summary>
		/// 盘点任务Id
		/// </summary>
		public double InventoryTaskId
		{
			get { return (double)GetRefId(InventoryTaskIdProperty); }
			set { SetRefId(InventoryTaskIdProperty, value); }
		}

		/// <summary>
		/// 盘点任务
		/// </summary>
		public static readonly RefEntityProperty<InventoryTask> InventoryTaskProperty = P<InventoryTaskSparePart>.RegisterRef(e => e.InventoryTask, InventoryTaskIdProperty);

		/// <summary>
		/// 盘点任务
		/// </summary>
		public InventoryTask InventoryTask
		{
			get { return GetRefEntity(InventoryTaskProperty); }
			set { SetRefEntity(InventoryTaskProperty, value); }
		}
        #endregion

        #region 视图属性
        #region 备件编码 SparePartCode
        /// <summary>
        /// 备件编码
        /// </summary>
        [Label("备件编码")]
        public static readonly Property<string> SparePartCodeProperty = P<InventoryTaskSparePart>.RegisterView(e => e.SparePartCode, p => p.SparePart.SparePartCode);

        /// <summary>
        /// 备件编码
        /// </summary>
        public string SparePartCode
        {
            get { return this.GetProperty(SparePartCodeProperty); }
        }
        #endregion

        #region 备件名称 SparePartName
        /// <summary>
        /// 备件名称
        /// </summary>
        [Label("备件名称")]
        public static readonly Property<string> SparePartNameProperty = P<InventoryTaskSparePart>.RegisterView(e => e.SparePartName, p => p.SparePart.SparePartName);

        /// <summary>
        /// 备件名称
        /// </summary>
        public string SparePartName
        {
            get { return this.GetProperty(SparePartNameProperty); }
        }
        #endregion

        #region 管控方式 ControlMethod
        /// <summary>
        /// 管控方式
        /// </summary>
        [Label("管控方式")]
        public static readonly Property<ControlMethod> ControlMethodProperty = P<InventoryTaskSparePart>.RegisterView(e => e.ControlMethod, p => p.SparePart.ControlMethod);

        /// <summary>
        /// 管控方式
        /// </summary>
        public ControlMethod ControlMethod
        {
            get { return this.GetProperty(ControlMethodProperty); }
        }
		#endregion

		#region 型号规格 Specification
		/// <summary>
		/// 型号规格
		/// </summary>
		[Label("型号规格")]
        public static readonly Property<string> SpecificationProperty = P<InventoryTaskSparePart>.RegisterView(e => e.Specification, p => p.SparePart.Specification);

        /// <summary>
        /// 型号规格
        /// </summary>
        public string Specification
        {
            get { return this.GetProperty(SpecificationProperty); }
        }
		#endregion

		#region 分类层级 ItemCategoryName
		/// <summary>
		/// 分类层级
		/// </summary>
		[Label("分类层级")]
        public static readonly Property<string> ItemCategoryNameProperty = P<InventoryTaskSparePart>.RegisterView(e => e.ItemCategoryName, p => p.SparePart.ItemCategory.Name);

        /// <summary>
        /// 分类层级
        /// </summary>
        public string ItemCategoryName
        {
            get { return this.GetProperty(ItemCategoryNameProperty); }
        }
		#endregion

		#region 类型 SpartType
		/// <summary>
		/// 类型
		/// </summary>
		[Label("类型")]
        public static readonly Property<SparePartType> SpartTypeProperty = P<InventoryTaskSparePart>.RegisterView(e => e.SpartType, p => p.SparePart.SpartType);

        /// <summary>
        /// 类型
        /// </summary>
        public SparePartType SpartType
        {
            get { return this.GetProperty(SpartTypeProperty); }
        }
        #endregion

        #region 单位 UnitName
        /// <summary>
        /// 单位
        /// </summary>
        [Label("单位")]
        public static readonly Property<string> UnitNameProperty = P<InventoryTaskSparePart>.RegisterView(e => e.UnitName, p => p.SparePart.Unit.Name);

        /// <summary>
        /// 单位
        /// </summary>
        public string UnitName
        {
            get { return this.GetProperty(UnitNameProperty); }
        }
        #endregion

        #region 备件任务单号 InventoryTaskNo
        /// <summary>
        /// 备件任务单号
        /// </summary>
        [Label("盘点任务单号")]
        public static readonly Property<string> InventoryTaskNoProperty = P<InventoryTaskSparePart>.RegisterView(e => e.InventoryTaskNo, p => p.InventoryTask.TaskNo);

        /// <summary>
        /// 备件任务单号
        /// </summary>
        public string InventoryTaskNo
        {
            get { return this.GetProperty(InventoryTaskNoProperty); }
        }
        #endregion


        #endregion
    }

    /// <summary>
    /// 盘点任务备件汇总 实体配置
    /// </summary>
    internal class InventoryTaskSparePartConfig : EntityConfig<InventoryTaskSparePart>
	{
		/// <summary>
		/// 配置元数据
		/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("EMS_INV_TSK_SP").MapAllProperties();
			Meta.EnablePhantoms();
		}
	}
}