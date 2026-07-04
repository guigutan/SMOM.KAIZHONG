using SIE;
using SIE.Common.Configs;
using SIE.Common.Configs.CommonConfigs;
using SIE.Core.Anomalymonitors;
using SIE.CSM.Suppliers;
using SIE.Domain;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.RedCardManagment.RedCards.Config;
using SIE.Resources;
using System;
using System.Reflection.Emit;

namespace SIE.RedCardManagment.RedCards
{
	/// <summary>
	/// 红牌管理
	/// </summary>
	[RootEntity, Serializable]
	[EntityWithConfig(typeof(NoConfig), "红牌管理编码设置", "红牌管理编码规则")]
	[EntityWithConfig(typeof(SyncDaysConfig), "信息追溯天数配置项", "信息追溯天数配置项")]
	[EntityWithConfig(typeof(GeneralAbnormalTaskConfig),"自动生成异常任务", "自动生成异常任务")]
	[EntityWithConfig(typeof(AutoLockProductSNConfig), "自动锁定产品SN", "自动锁定产品SN")]
	[EntityExtensionConfig(typeof(RecardTaskExtConfigValue), "红牌管理", "红牌管理")]
	[ConditionQueryType(typeof(RedCardCriteria))]
	[Label("红牌管理")]
	public partial class RedCard : DataEntity
	{
		#region 红牌单号 No
		/// <summary>
		/// 红牌单号
		/// </summary>
		[Required]
		[NotDuplicate]
		[Label("红牌单号")]
		public static readonly Property<string> NoProperty = P<RedCard>.Register(e => e.No);

		/// <summary>
		/// 红牌单号
		/// </summary>
		public string No
		{
			get { return GetProperty(NoProperty); }
			set { SetProperty(NoProperty, value); }
		}
		#endregion

		#region 生产批次 Batch
		/// <summary>
		/// 生产批次
		/// </summary>
		[Label("生产批次")]
		public static readonly Property<string> BatchProperty = P<RedCard>.Register(e => e.Batch);

		/// <summary>
		/// 生产批次
		/// </summary>
		public string Batch
		{
			get { return GetProperty(BatchProperty); }
			set { SetProperty(BatchProperty, value); }
		}
		#endregion

		#region 物料SN ItemSN
		/// <summary>
		/// 物料SN
		/// </summary>
		[Label("物料SN")]
		public static readonly Property<string> ItemSNProperty = P<RedCard>.Register(e => e.ItemSN);

		/// <summary>
		/// 物料SN
		/// </summary>
		public string ItemSN
		{
			get { return GetProperty(ItemSNProperty); }
			set { SetProperty(ItemSNProperty, value); }
		}
		#endregion

		#region 生产周期开始时间 ProductDateStart
		/// <summary>
		/// 生产周期开始时间
		/// </summary>
		[Label("生产周期开始时间")]
		public static readonly Property<DateTime?> ProductDateStartProperty = P<RedCard>.Register(e => e.ProductDateStart);

		/// <summary>
		/// 生产周期开始时间
		/// </summary>
		public DateTime? ProductDateStart
		{
			get { return GetProperty(ProductDateStartProperty); }
			set { SetProperty(ProductDateStartProperty, value); }
		}
		#endregion

		#region 生产周期结束时间 ProductDateEnd
		/// <summary>
		/// 生产周期结束时间
		/// </summary>
		[Label("生产周期结束时间")]
		public static readonly Property<DateTime?> ProductDateEndProperty = P<RedCard>.Register(e => e.ProductDateEnd);

		/// <summary>
		/// 生产周期结束时间
		/// </summary>
		public DateTime? ProductDateEnd
		{
			get { return GetProperty(ProductDateEndProperty); }
			set { SetProperty(ProductDateEndProperty, value); }
		}
		#endregion

		#region 执行时间 ApplyTime
		/// <summary>
		/// 执行时间
		/// </summary>
		[Label("执行时间")]
		public static readonly Property<DateTime?> ApplyTimeProperty = P<RedCard>.Register(e => e.ApplyTime);

		/// <summary>
		/// 执行时间
		/// </summary>
		public DateTime? ApplyTime
		{
			get { return GetProperty(ApplyTimeProperty); }
			set { SetProperty(ApplyTimeProperty, value); }
		}
		#endregion

		#region 申请单号 ApplyBillNo
		/// <summary>
		/// 申请单号
		/// </summary>
		[Label("申请单号")]
		public static readonly Property<string> ApplyBillNoProperty = P<RedCard>.Register(e => e.ApplyBillNo);

		/// <summary>
		/// 申请单号
		/// </summary>
		public string ApplyBillNo
		{
			get { return GetProperty(ApplyBillNoProperty); }
			set { SetProperty(ApplyBillNoProperty, value); }
		}
		#endregion

		#region 物料批次 ItemBatch
		/// <summary>
		/// 物料批次
		/// </summary>
		[Label("物料批次")]
		public static readonly Property<string> ItemBatchProperty = P<RedCard>.Register(e => e.ItemBatch);

		/// <summary>
		/// 物料批次
		/// </summary>
		public string ItemBatch
		{
			get { return GetProperty(ItemBatchProperty); }
			set { SetProperty(ItemBatchProperty, value); }
		}
		#endregion

		#region 供应商 Supplier
		/// <summary>
		/// 供应商Id
		/// </summary>
		public static readonly IRefIdProperty SupplierIdProperty = P<RedCard>.RegisterRefId(e => e.SupplierId, ReferenceType.Normal);

		/// <summary>
		/// 供应商Id
		/// </summary>
		public double? SupplierId
		{
			get { return (double?)GetRefNullableId(SupplierIdProperty); }
			set { SetRefNullableId(SupplierIdProperty, value); }
		}

		/// <summary>
		/// 供应商
		/// </summary>
		public static readonly RefEntityProperty<Supplier> SupplierProperty = P<RedCard>.RegisterRef(e => e.Supplier, SupplierIdProperty);

		/// <summary>
		/// 供应商
		/// </summary>
		public Supplier Supplier
		{
			get { return GetRefEntity(SupplierProperty); }
			set { SetRefEntity(SupplierProperty, value); }
		}
		#endregion

		#region 状态 Status
		/// <summary>
		/// 状态
		/// </summary>
		[Label("物料红牌状态")]
		public static readonly Property<RedCardState> StatusProperty = P<RedCard>.Register(e => e.Status);

		/// <summary>
		/// 状态
		/// </summary>
		public RedCardState Status
		{
			get { return GetProperty(StatusProperty); }
			set { SetProperty(StatusProperty, value); }
		}
		#endregion

		#region 添加方式 AddWay
		/// <summary>
		/// 添加方式
		/// </summary>
		[Label("添加方式")]
		public static readonly Property<RecordAddWay> AddWayProperty = P<RedCard>.Register(e => e.AddWay);

		/// <summary>
		/// 添加方式
		/// </summary>
		public RecordAddWay AddWay
		{
			get { return GetProperty(AddWayProperty); }
			set { SetProperty(AddWayProperty, value); }
		}
		#endregion

		#region 执行人 Applicant
		/// <summary>
		/// 执行人Id
		/// </summary>
		public static readonly IRefIdProperty ApplicantIdProperty = P<RedCard>.RegisterRefId(e => e.ApplicantId, ReferenceType.Normal);

		/// <summary>
		/// 执行人Id
		/// </summary>
		public double? ApplicantId
		{
			get { return (double?)GetRefId(ApplicantIdProperty); }
			set { SetRefId(ApplicantIdProperty, value); }
		}

		/// <summary>
		/// 执行人
		/// </summary>
		public static readonly RefEntityProperty<Employee> ApplicantProperty = P<RedCard>.RegisterRef(e => e.Applicant, ApplicantIdProperty);

		/// <summary>
		/// 执行人
		/// </summary>
		public Employee Applicant
		{
			get { return GetRefEntity(ApplicantProperty); }
			set { SetRefEntity(ApplicantProperty, value); }
		}
		#endregion

		#region 物料 Item
		/// <summary>
		/// 物料Id
		/// </summary>
		public static readonly IRefIdProperty ItemIdProperty = P<RedCard>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

		/// <summary>
		/// 物料Id
		/// </summary>
		public double ItemId
		{
			get { return (double)GetRefId(ItemIdProperty); }
			set { SetRefId(ItemIdProperty, value); }
		}

		/// <summary>
		/// 物料
		/// </summary>
		[Label("物料")]
		[Required]
		public static readonly RefEntityProperty<Item> ItemProperty = P<RedCard>.RegisterRef(e => e.Item, ItemIdProperty);

		/// <summary>
		/// 物料
		/// </summary>
		public Item Item
		{
			get { return GetRefEntity(ItemProperty); }
			set { SetRefEntity(ItemProperty, value); }
		}
		#endregion

		#region 物料编号 ItemCode
		/// <summary>
		/// 物料编号
		/// </summary>
		[Label("物料编号")]
		public static readonly Property<string> ItemCodeProperty = P<RedCard>.RegisterView(e => e.ItemCode, p => p.Item.Code);

		/// <summary>
		/// 物料编号
		/// </summary>
		public string ItemCode
		{
			get { return this.GetProperty(ItemCodeProperty); }
		}
		#endregion

		#region 物料名称 ItemName
		/// <summary>
		/// 物料名称
		/// </summary>
		[Label("物料名称")]
        public static readonly Property<string> ItemNameProperty = P<RedCard>.RegisterView(e => e.ItemName, p => p.Item.Name);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
        }
		#endregion

		#region 供应商编号 SupplierCode
		/// <summary>
		/// 供应商编号
		/// </summary>
		[Label("供应商编号")]
		public static readonly Property<string> SupplierCodeProperty = P<RedCard>.RegisterView(e => e.SupplierCode, p => p.Supplier.Code);

		/// <summary>
		/// 供应商编号
		/// </summary>
		public string SupplierCode
		{
			get { return this.GetProperty(SupplierCodeProperty); }
		}
		#endregion

		#region 供应商名称 SupplierName
		/// <summary>
		/// 供应商名称
		/// </summary>
		[Label("供应商名称")]
		public static readonly Property<string> SupplierNameProperty = P<RedCard>.RegisterView(e => e.SupplierName, p => p.Supplier.Name);

		/// <summary>
		/// 供应商名称
		/// </summary>
		public string SupplierName
		{
			get { return this.GetProperty(SupplierNameProperty); }
		}
		#endregion

		#region 异常任务单号 AbnormalTaskNo
		/// <summary>
		/// 异常任务单号
		/// </summary>
		[Label("异常任务单号")]
		public static readonly Property<string> AbnormalTaskNoProperty = P<RedCard>.Register(e => e.AbnormalTaskNo);

		/// <summary>
		/// 异常任务单号
		/// </summary>
		public string AbnormalTaskNo
		{
			get { return GetProperty(AbnormalTaskNoProperty); }
			set { SetProperty(AbnormalTaskNoProperty, value); }
		}
		#endregion

		#region 视图属性
		#region 执行人名称 ApplicantName
		/// <summary>
		/// 执行人名称
		/// </summary>
		[Label("执行人")]
		public static readonly Property<string> ApplicantNameProperty = P<RedCard>.RegisterView(e => e.ApplicantName, p => p.Applicant.Name);

		/// <summary>
		/// 执行人名称
		/// </summary>
		public string ApplicantName
		{
			get { return this.GetProperty(ApplicantNameProperty); }
		}

		#endregion
		#endregion

	}

	/// <summary>
	/// 红牌管理 实体配置
	/// </summary>
	internal class RedCardConfig : EntityConfig<RedCard>
	{
		/// <summary>
      	  	/// 配置元数据
    	    	/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("QMS_RED_CARD").MapAllProperties();
			Meta.EnablePhantoms();
		}
	}
}