using SIE.Domain;
using SIE.Inventory.Commom;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Packages.Boxs;
using System;

namespace SIE.Inventory.Piles
{
    /// <summary>
    /// 垛日志
    /// </summary>
    [RootEntity, Serializable]
	[CriteriaQuery]
	[Label("垛日志")]
	public partial class PileLog : DataEntity
	{
        #region 垛编码 PileCode
        /// <summary>
        /// 垛编码
        /// </summary>
        [Label("垛编码")]
        public static readonly Property<string> PileCodeProperty = P<PileLog>.Register(e => e.PileCode);

        /// <summary>
        /// 垛编码
        /// </summary>
        public string PileCode
        {
            get { return this.GetProperty(PileCodeProperty); }
            set { this.SetProperty(PileCodeProperty, value); }
        }
        #endregion

        #region 当前位置 CurLocation
        /// <summary>
        /// 当前位置
        /// </summary>
        [MaxLength(80)]
		[Label("当前位置")]
		public static readonly Property<string> CurLocationProperty = P<PileLog>.Register(e => e.CurLocation);

		/// <summary>
		/// 当前位置
		/// </summary>
		public string CurLocation
		{
			get { return GetProperty(CurLocationProperty); }
			set { SetProperty(CurLocationProperty, value); }
		}
		#endregion

		#region 单据号 BillNo
		/// <summary>
		/// 单据号
		/// </summary>
		[Label("单据号")]
		public static readonly Property<string> BillNoProperty = P<PileLog>.Register(e => e.BillNo);

		/// <summary>
		/// 单据号
		/// </summary>
		public string BillNo
		{
			get { return GetProperty(BillNoProperty); }
			set { SetProperty(BillNoProperty, value); }
		}
		#endregion

		#region 业务类型 BusinessType
		/// <summary>
		/// 业务类型
		/// </summary>
		[Label("业务类型")]
		public static readonly Property<BusinessType?> BusinessTypeProperty = P<PileLog>.Register(e => e.BusinessType);

		/// <summary>
		/// 业务类型
		/// </summary>
		public BusinessType? BusinessType
		{
			get { return GetProperty(BusinessTypeProperty); }
			set { SetProperty(BusinessTypeProperty, value); }
		}
		#endregion

		#region 重量(KG) Weight
		/// <summary>
		/// 重量(KG)
		/// </summary>
		[Label("重量(KG)")]
		public static readonly Property<decimal?> WeightProperty = P<PileLog>.Register(e => e.Weight);

		/// <summary>
		/// 重量(KG)
		/// </summary>
		public decimal? Weight
		{
			get { return GetProperty(WeightProperty); }
			set { SetProperty(WeightProperty, value); }
		}
		#endregion

		#region 长度(CM) Length
		/// <summary>
		/// 长度(CM)
		/// </summary>
		[Label("长度(CM)")]
		public static readonly Property<decimal?> LengthProperty = P<PileLog>.Register(e => e.Length);

		/// <summary>
		/// 长度(CM)
		/// </summary>
		public decimal? Length
		{
			get { return GetProperty(LengthProperty); }
			set { SetProperty(LengthProperty, value); }
		}
		#endregion

		#region 宽度(CM) Width
		/// <summary>
		/// 宽度(CM)
		/// </summary>
		[Label("宽度(CM)")]
		public static readonly Property<decimal?> WidthProperty = P<PileLog>.Register(e => e.Width);

		/// <summary>
		/// 宽度(CM)
		/// </summary>
		public decimal? Width
		{
			get { return GetProperty(WidthProperty); }
			set { SetProperty(WidthProperty, value); }
		}
		#endregion

		#region 高度(CM) Height
		/// <summary>
		/// 高度(CM)
		/// </summary>
		[Label("高度(CM)")]
		public static readonly Property<decimal?> HeightProperty = P<PileLog>.Register(e => e.Height);

		/// <summary>
		/// 高度(CM)
		/// </summary>
		public decimal? Height
		{
			get { return GetProperty(HeightProperty); }
			set { SetProperty(HeightProperty, value); }
		}
		#endregion

		#region 操作类型 PileOpType
		/// <summary>
		/// 操作类型
		/// </summary>
		[Label("操作类型")]
		public static readonly Property<PileOpType> PileOpTypeProperty = P<PileLog>.Register(e => e.PileOpType);

		/// <summary>
		/// 操作类型
		/// </summary>
		public PileOpType PileOpType
		{
			get { return GetProperty(PileOpTypeProperty); }
			set { SetProperty(PileOpTypeProperty, value); }
		}
        #endregion

        #region 物料状态 ItemState
        /// <summary>
        /// 物料状态
        /// </summary>
        [Label("物料状态")]
        public static readonly Property<ItemState?> ItemStateProperty = P<PileLog>.Register(e => e.ItemState);

        /// <summary>
        /// 物料状态
        /// </summary>
        public ItemState? ItemState
        {
            get { return this.GetProperty(ItemStateProperty); }
            set { this.SetProperty(ItemStateProperty, value); }
        }
        #endregion

        #region 垛状态 PileState
        /// <summary>
        /// 垛状态
        /// </summary>
        [Label("垛状态")]
        public static readonly Property<BoxState> PileStateProperty = P<PileLog>.Register(e => e.PileState);

        /// <summary>
        /// 垛状态
        /// </summary>
        public BoxState PileState
        {
            get { return this.GetProperty(PileStateProperty); }
            set { this.SetProperty(PileStateProperty, value); }
        }
        #endregion

    }

	/// <summary>
	/// 垛日志 实体配置
	/// </summary>
	internal class PileLogConfig : EntityConfig<PileLog>
	{
		/// <summary>
      	  	/// 配置元数据
    	    	/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("PILE_LOG").MapAllProperties();
			Meta.Property(PileLog.CurLocationProperty).ColumnMeta.HasLength(160);
			Meta.EnablePhantoms();
			Meta.DisableDataSync();
		}
	}
}