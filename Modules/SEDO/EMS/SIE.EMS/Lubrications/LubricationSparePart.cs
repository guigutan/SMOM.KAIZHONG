using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.Enums;
using SIE.EMS.SpareParts;
using SIE.EMS.SpareParts.Enums;
using SIE.EMS.SpareParts.OutDepots.Details;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Lubrications
{
    /// <summary>
    /// 润滑备件更换
    /// </summary>
    [ChildEntity, Serializable]
	[Label("润滑备件更换")]
	public partial class LubricationSparePart : DataEntity
	{
		#region 润滑项目 Lubrication
		/// <summary>
		/// 润滑项目Id
		/// </summary>
		public static readonly IRefIdProperty LubricationIdProperty = P<LubricationSparePart>.RegisterRefId(e => e.LubricationId, ReferenceType.Parent);

		/// <summary>
		/// 润滑项目Id
		/// </summary>
		public double LubricationId
		{
			get { return (double)GetRefId(LubricationIdProperty); }
			set { SetRefId(LubricationIdProperty, value); }
		}

		/// <summary>
		/// 润滑项目
		/// </summary>
		public static readonly RefEntityProperty<Lubrication> LubricationProperty = P<LubricationSparePart>.RegisterRef(e => e.Lubrication, LubricationIdProperty);

		/// <summary>
		/// 润滑项目
		/// </summary>
		public Lubrication Lubrication
		{
			get { return GetRefEntity(LubricationProperty); }
			set { SetRefEntity(LubricationProperty, value); }
		}
		#endregion

		#region 备件 SparePart
		/// <summary>
		/// 备件Id
		/// </summary>
		public static readonly IRefIdProperty SparePartIdProperty = P<LubricationSparePart>.RegisterRefId(e => e.SparePartId, ReferenceType.Normal);

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
		public static readonly RefEntityProperty<SparePart> SparePartProperty = P<LubricationSparePart>.RegisterRef(e => e.SparePart, SparePartIdProperty);

		/// <summary>
		/// 备件
		/// </summary>
		public SparePart SparePart
		{
			get { return GetRefEntity(SparePartProperty); }
			set { SetRefEntity(SparePartProperty, value); }
		}
		#endregion

		#region 备件更换状态 State
		/// <summary>
		/// 备件更换状态
		/// </summary>
		[Label("备件更换状态")]
		public static readonly Property<ChangeSparePartState> StateProperty = P<LubricationSparePart>.Register(e => e.State);

		/// <summary>
		/// 备件更换状态
		/// </summary>
		public ChangeSparePartState State
		{
			get { return GetProperty(StateProperty); }
			set { SetProperty(StateProperty, value); }
		}
		#endregion

		#region 备件出库单 PartOutDepotDetail
		/// <summary>
		/// 备件出库单Id
		/// </summary>
		[Label("备件出库单")]
		public static readonly IRefIdProperty PartOutDepotDetailIdProperty = P<LubricationSparePart>.RegisterRefId(e => e.PartOutDepotDetailId, ReferenceType.Normal);

		/// <summary>
		/// 备件出库单Id
		/// </summary>
		public double? PartOutDepotDetailId
		{
			get { return (double?)GetRefNullableId(PartOutDepotDetailIdProperty); }
			set { SetRefNullableId(PartOutDepotDetailIdProperty, value); }
		}

		/// <summary>
		/// 备件出库单
		/// </summary>
		public static readonly RefEntityProperty<PartOutDepotDetail> PartOutDepotDetailProperty = P<LubricationSparePart>.RegisterRef(e => e.PartOutDepotDetail, PartOutDepotDetailIdProperty);

		/// <summary>
		/// 备件出库单
		/// </summary>
		public PartOutDepotDetail PartOutDepotDetail
		{
			get { return GetRefEntity(PartOutDepotDetailProperty); }
			set { SetRefEntity(PartOutDepotDetailProperty, value); }
		}
		#endregion

		#region 更换数量 ChangeQty
		/// <summary>
		/// 更换数量
		/// </summary>
		[Label("更换数量")]
		public static readonly Property<int> ChangeQtyProperty = P<LubricationSparePart>.Register(e => e.ChangeQty);

		/// <summary>
		/// 更换数量
		/// </summary>
		public int ChangeQty
		{
			get { return GetProperty(ChangeQtyProperty); }
			set { SetProperty(ChangeQtyProperty, value); }
		}
		#endregion

		#region 旧序列号 OldSequence
		/// <summary>
		/// 旧序列号
		/// </summary>
		[Label("旧序列号")]
		public static readonly Property<string> OldSequenceProperty = P<LubricationSparePart>.Register(e => e.OldSequence);

		/// <summary>
		/// 旧序列号
		/// </summary>
		public string OldSequence
		{
			get { return GetProperty(OldSequenceProperty); }
			set { SetProperty(OldSequenceProperty, value); }
		}
		#endregion

		#region 备注 Remark
		/// <summary>
		/// 备注
		/// </summary>
		[MaxLength(1000)]
		[Label("备注")]
		public static readonly Property<string> RemarkProperty = P<LubricationSparePart>.Register(e => e.Remark);

		/// <summary>
		/// 备注
		/// </summary>
		public string Remark
		{
			get { return GetProperty(RemarkProperty); }
			set { SetProperty(RemarkProperty, value); }
		}
		#endregion

		#region 非映射字段

		#region 剩余数量 RemainingQty
		/// <summary>
		/// 剩余数量
		/// </summary>
		[Label("剩余数量")]
		public static readonly Property<int?> RemainingQtyProperty = P<LubricationSparePart>.Register(e => e.RemainingQty);

		/// <summary>
		/// 剩余数量
		/// </summary>
		public int? RemainingQty
		{
			get { return this.GetProperty(RemainingQtyProperty); }
			set { this.SetProperty(RemainingQtyProperty, value); }
		}
		#endregion



		#endregion

		#region 视图属性

		#region 备件编码 SparePartCodeView
		/// <summary>
		/// 备件编码
		/// </summary>
		[Label("备件编码")]
		public static readonly Property<string> SparePartCodeViewProperty = P<LubricationSparePart>.RegisterView(e => e.SparePartCodeView, p => p.SparePart.SparePartCode);

		/// <summary>
		/// 备件编码
		/// </summary>
		public string SparePartCodeView
		{
			get { return this.GetProperty(SparePartCodeViewProperty); }
		}
		#endregion

		#region 备件名称 SparePartNameView
		/// <summary>
		/// 备件名称
		/// </summary>
		[Label("备件名称")]
		public static readonly Property<string> SparePartNameViewProperty = P<LubricationSparePart>.RegisterView(e => e.SparePartNameView, p => p.SparePart.SparePartName);

		/// <summary>
		/// 备件名称
		/// </summary>
		public string SparePartNameView
		{
			get { return this.GetProperty(SparePartNameViewProperty); }
		}
		#endregion

		#region 规格型号 SpecificationView
		/// <summary>
		/// 规格型号
		/// </summary>
		[Label("规格型号")]
		public static readonly Property<string> SpecificationViewProperty = P<LubricationSparePart>.RegisterView(e => e.SpecificationView, p => p.SparePart.Specification);

		/// <summary>
		/// 是否以旧换新
		/// </summary>
		public string SpecificationView
		{
			get { return this.GetProperty(SpecificationViewProperty); }
		}
		#endregion

		#region 单位 UnitView
		/// <summary>
		/// 单位
		/// </summary>
		[Label("单位")]
		public static readonly Property<string> UnitViewProperty = P<LubricationSparePart>.RegisterView(e => e.UnitView, p => p.SparePart.Unit.Name);

		/// <summary>
		/// 单位
		/// </summary>
		public string UnitView
		{
			get { return this.GetProperty(UnitViewProperty); }
		}
		#endregion

		#region 出库单号 OutDepotNoView
		/// <summary>
		/// 申请单号
		/// </summary>
		[Label("出库单号")]
		public static readonly Property<string> OutDepotNoViewProperty = P<LubricationSparePart>.RegisterView(e => e.OutDepotNoView, p => p.PartOutDepotDetail.OutDepot.No);

		/// <summary>
		/// 申请单号
		/// </summary>
		public string OutDepotNoView
		{
			get { return this.GetProperty(OutDepotNoViewProperty); }
			set { this.SetProperty(OutDepotNoViewProperty, value); }
		}
		#endregion

		#region 序列号 SeriaNoView
		/// <summary>
		/// 序列号
		/// </summary>
		[Label("序列号")]
		public static readonly Property<string> SeriaNoViewProperty = P<LubricationSparePart>.RegisterView(e => e.SeriaNoView, p => p.PartOutDepotDetail.SeriaNoRef.OrderNumberCode);

		/// <summary>
		/// 序列号
		/// </summary>
		public string SeriaNoView
		{
			get { return this.GetProperty(SeriaNoViewProperty); }
			set { this.SetProperty(SeriaNoViewProperty, value); }
		}
		#endregion

		#region 批次号 BatchNoView
		/// <summary>
		/// 批次号
		/// </summary>
		[Label("批次号")]
		public static readonly Property<string> BatchNoViewProperty = P<LubricationSparePart>.RegisterView(e => e.BatchNoView, p => p.PartOutDepotDetail.BatchNoRef.BatchNumber);

		/// <summary>
		/// 批次号
		/// </summary>
		public string BatchNoView
		{
			get { return this.GetProperty(BatchNoViewProperty); }
			set { this.SetProperty(BatchNoViewProperty, value); }
		}
		#endregion

		#region 管控方式 ControlMethod
		/// <summary>
		/// 管控方式
		/// </summary>
		[Label("管控方式")]
		public static readonly Property<ControlMethod> ControlMethodProperty = P<LubricationSparePart>.RegisterView(e => e.ControlMethod, p => p.SparePart.ControlMethod);

		/// <summary>
		/// 管控方式
		/// </summary>
		public ControlMethod ControlMethod
		{
			get { return this.GetProperty(ControlMethodProperty); }
		}
		#endregion

		#endregion

	}

	/// <summary>
	/// 润滑备件更换 实体配置
	/// </summary>
	internal class LubricationSparePartConfig : EntityConfig<LubricationSparePart>
	{
		protected override void AddValidations(IValidationDeclarer rules)
		{
			base.AddValidations(rules);
			rules.AddRule(LubricationSparePart.RemarkProperty, new StringLengthRangeRule() { Max = 2000 });
			rules.AddRule(LubricationSparePart.ChangeQtyProperty, new PositiveNumberRule());
		}
		/// <summary>
		/// 配置元数据
		/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("EMS_LUBR_SP").MapAllProperties();
			Meta.Property(LubricationSparePart.RemarkProperty).ColumnMeta.HasLength(2000);
			Meta.Property(LubricationSparePart.RemainingQtyProperty).DontMapColumn();
			Meta.EnablePhantoms();
		}
	}
}