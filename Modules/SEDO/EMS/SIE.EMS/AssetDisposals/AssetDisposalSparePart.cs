using SIE.Common.Prints;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.SpareParts;
using SIE.EMS.SpareParts.Enums;
using SIE.Equipments.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Warehouses;
using System;
using System.Text;

namespace SIE.EMS.AssetDisposals
{
	/// <summary>
	/// 资产处置备件回收
	/// </summary>
	[ChildEntity, Serializable]	
	[Label("备件回收")]
	public partial class AssetDisposalSparePart : DataEntity
	{
		#region 批次号 LotNo
		/// <summary>
		/// 批次号
		/// </summary>
		[Label("批次号")]
		public static readonly Property<string> LotNoProperty = P<AssetDisposalSparePart>.Register(e => e.LotNo);

		/// <summary>
		/// 批次号
		/// </summary>
		public string LotNo
		{
			get { return GetProperty(LotNoProperty); }
			set { SetProperty(LotNoProperty, value); }
		}
		#endregion

		#region 序列号 Sn
		/// <summary>
		/// 序列号
		/// </summary>
		[Label("序列号")]
		public static readonly Property<string> SnProperty = P<AssetDisposalSparePart>.Register(e => e.Sn);

		/// <summary>
		/// 序列号
		/// </summary>
		public string Sn
		{
			get { return GetProperty(SnProperty); }
			set { SetProperty(SnProperty, value); }
		}
		#endregion

		#region 回收数量 Qty
		/// <summary>
		/// 回收数量
		/// </summary>
		[Label("回收数量")]
		public static readonly Property<int> QtyProperty = P<AssetDisposalSparePart>.Register(e => e.Qty);

		/// <summary>
		/// 回收数量
		/// </summary>
		public int Qty
		{
			get { return GetProperty(QtyProperty); }
			set { SetProperty(QtyProperty, value); }
		}
		#endregion

		#region 质量状态 QualityStatus
		/// <summary>
		/// 质量状态
		/// </summary>
		[Label("质量状态")]
		public static readonly Property<QualityStatus?> QualityStatusProperty = P<AssetDisposalSparePart>.Register(e => e.QualityStatus);

		/// <summary>
		/// 质量状态
		/// </summary>
		public QualityStatus? QualityStatus
		{
			get { return GetProperty(QualityStatusProperty); }
			set { SetProperty(QualityStatusProperty, value); }
		}
		#endregion

		#region 备件 SparePart
		/// <summary>
		/// 备件Id
		/// </summary>
		[Label("备件编码")]
		public static readonly IRefIdProperty SparePartIdProperty = P<AssetDisposalSparePart>.RegisterRefId(e => e.SparePartId, ReferenceType.Normal);

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
		public static readonly RefEntityProperty<SparePart> SparePartProperty = P<AssetDisposalSparePart>.RegisterRef(e => e.SparePart, SparePartIdProperty);

		/// <summary>
		/// 备件
		/// </summary>
		public SparePart SparePart
		{
			get { return GetRefEntity(SparePartProperty); }
			set { SetRefEntity(SparePartProperty, value); }
		}
		#endregion

		#region 入库仓库 Warehouse
		/// <summary>
		/// 入库仓库Id
		/// </summary>
		[Label("入库仓库")]
		public static readonly IRefIdProperty WarehouseIdProperty = P<AssetDisposalSparePart>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

		/// <summary>
		/// 入库仓库Id
		/// </summary>
		public double WarehouseId
		{
			get { return (double)GetRefId(WarehouseIdProperty); }
			set { SetRefId(WarehouseIdProperty, value); }
		}

		/// <summary>
		/// 入库仓库
		/// </summary>
		public static readonly RefEntityProperty<Warehouse> WarehouseProperty = P<AssetDisposalSparePart>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

		/// <summary>
		/// 入库仓库
		/// </summary>
		public Warehouse Warehouse
		{
			get { return GetRefEntity(WarehouseProperty); }
			set { SetRefEntity(WarehouseProperty, value); }
		}
		#endregion

		#region 资产处置 AssetDisposal
		/// <summary>
		/// 资产处置Id
		/// </summary>
		[Label("资产处置")]
		public static readonly IRefIdProperty AssetDisposalIdProperty = P<AssetDisposalSparePart>.RegisterRefId(e => e.AssetDisposalId, ReferenceType.Parent);

		/// <summary>
		/// 资产处置Id
		/// </summary>
		public double AssetDisposalId
		{
			get { return (double)GetRefId(AssetDisposalIdProperty); }
			set { SetRefId(AssetDisposalIdProperty, value); }
		}

		/// <summary>
		/// 资产处置
		/// </summary>
		public static readonly RefEntityProperty<AssetDisposal> AssetDisposalProperty = P<AssetDisposalSparePart>.RegisterRef(e => e.AssetDisposal, AssetDisposalIdProperty);

		/// <summary>
		/// 资产处置
		/// </summary>
		public AssetDisposal AssetDisposal
		{
			get { return GetRefEntity(AssetDisposalProperty); }
			set { SetRefEntity(AssetDisposalProperty, value); }
		}
		#endregion

		#region 视图属性

		#region 备件编码 SparePartCode
		/// <summary>
		/// 备件编码
		/// </summary>
		[Label("备件编码")]
		public static readonly Property<string> SparePartCodeProperty = P<AssetDisposalSparePart>.RegisterView(e => e.SparePartCode, p => p.SparePart.SparePartCode);

		/// <summary>
		/// 备件编码
		/// </summary>
		public string SparePartCode
		{
			get { return this.GetProperty(SparePartCodeProperty); }
			set { SetProperty(SparePartCodeProperty, value); }
		}
		#endregion

		#region 备件名称 SparePartName
		/// <summary>
		/// 备件名称
		/// </summary>
		[Label("备件名称")]
		public static readonly Property<string> SparePartNameProperty = P<AssetDisposalSparePart>.RegisterView(e => e.SparePartName, p => p.SparePart.SparePartName);

		/// <summary>
		/// 备件名称
		/// </summary>
		public string SparePartName
		{
			get { return this.GetProperty(SparePartNameProperty); }
			set { SetProperty(SparePartNameProperty, value); }
		}
		#endregion

		#region 规格型号 Specification
		/// <summary>
		/// 规格型号
		/// </summary>
		[Label("规格型号")]
		public static readonly Property<string> SpecificationProperty = P<AssetDisposalSparePart>.RegisterView(e => e.Specification, p => p.SparePart.Specification);

		/// <summary>
		/// 规格型号
		/// </summary>
		public string Specification
		{
			get { return GetProperty(SpecificationProperty); }
			set { SetProperty(SpecificationProperty, value); }
		}
		#endregion

		#region 类型 SpartType
		/// <summary>
		/// 类型
		/// </summary>
		[Label("类型")]
		public static readonly Property<SparePartType?> SpartTypeProperty = P<AssetDisposalSparePart>.RegisterView(e => e.SpartType, p => p.SparePart.SpartType);

		/// <summary>
		/// 类型
		/// </summary>
		public SparePartType? SpartType
		{
			get { return this.GetProperty(SpartTypeProperty); }
			set { SetProperty(SpartTypeProperty, value); }
		}
		#endregion

		#region 管控方式 ControlMethod
		/// <summary>
		/// 管控方式
		/// </summary>
		[Label("管控方式")]
		public static readonly Property<ControlMethod?> ControlMethodProperty = P<AssetDisposalSparePart>.RegisterView(e => e.ControlMethod, p => p.SparePart.ControlMethod);

		/// <summary>
		/// 管控方式
		/// </summary>
		public ControlMethod? ControlMethod
		{
			get { return this.GetProperty(ControlMethodProperty); }
			set { SetProperty(ControlMethodProperty, value); }
		}
		#endregion

		#region 单位 UnitName
		/// <summary>
		/// 单位
		/// </summary>
		[Label("单位")]
		public static readonly Property<string> UnitNameProperty = P<AssetDisposalSparePart>.RegisterView(e => e.UnitName, p => p.SparePart.Unit.Name);

		/// <summary>
		/// 单位
		/// </summary>
		public string UnitName
		{
			get { return this.GetProperty(UnitNameProperty); }
			set { SetProperty(UnitNameProperty, value); }
		}
		#endregion

		#region 仓库编码 WarehouseCode
		/// <summary>
		/// 仓库编码
		/// </summary>
		[Label("仓库编码")]
		public static readonly Property<string> WarehouseCodeProperty = P<AssetDisposalSparePart>.RegisterView(e => e.WarehouseCode, p => p.Warehouse.Code);

		/// <summary>
		/// 仓库编码
		/// </summary>
		public string WarehouseCode
		{
			get { return this.GetProperty(WarehouseCodeProperty); }
			set { SetProperty(WarehouseCodeProperty, value); }
		}
		#endregion

		#endregion

		#region 不映射数据库的属性

		#region 打印模板 PrintTemplate
		/// <summary>
		/// 打印模板Id
		/// </summary>
		[Label("打印模板")]
        public static readonly IRefIdProperty PrintTemplateIdProperty =
            P<AssetDisposalSparePart>.RegisterRefId(e => e.PrintTemplateId, ReferenceType.Normal);

		/// <summary>
		/// 打印模板Id
		/// </summary>
		public double? PrintTemplateId
        {
            get { return (double?)this.GetRefNullableId(PrintTemplateIdProperty); }
            set { this.SetRefNullableId(PrintTemplateIdProperty, value); }
        }

		/// <summary>
		/// 打印模板
		/// </summary>
		public static readonly RefEntityProperty<PrintTemplate> PrintTemplateProperty =
            P<AssetDisposalSparePart>.RegisterRef(e => e.PrintTemplate, PrintTemplateIdProperty);

		/// <summary>
		/// 打印模板
		/// </summary>
		public PrintTemplate PrintTemplate
        {
            get { return this.GetRefEntity(PrintTemplateProperty); }
            set { this.SetRefEntity(PrintTemplateProperty, value); }
        }
        #endregion

        #endregion
    }

	/// <summary>
	/// 资产处置备件回收 实体配置
	/// </summary>
	internal class AssetDisposalSparePartConfig : EntityConfig<AssetDisposalSparePart>
	{
		/// <summary>
		/// 配置元数据
		/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("EMS_ASET_DSPO_SP").MapAllProperties();
			Meta.Property(AssetDisposalSparePart.PrintTemplateIdProperty).DontMapColumn();
			Meta.Property(AssetDisposalSparePart.PrintTemplateProperty).DontMapColumn();
			Meta.EnablePhantoms();
		}

		/// <summary>
		/// 校验规则
		/// </summary>
		/// <param name="rules">规则</param>
		protected override void AddValidations(IValidationDeclarer rules)
		{
			rules.AddRule(new HandlerRule()
			{
				Handler = (o, e) =>
				{
					var para = o.CastTo<AssetDisposalSparePart>();
					StringBuilder sb = new StringBuilder();

					if (para.Qty <= 0)
					{
						sb.AppendLine("备件编码【{0}】的回收数量须大于0！".L10nFormat(para.SparePart.SparePartCode));
					}

					if (para.QualityStatus == null)
					{
						sb.AppendLine("备件编码【{0}】的质量状态不能为空！".L10nFormat(para.SparePart.SparePartCode));
					}
					if (para.SparePart.ControlMethod == ControlMethod.Sn && para.Sn.IsNullOrEmpty())
					{
						sb.AppendLine("序列号管控的备件的【序列号】不能为空".L10N());
					}
					e.BrokenDescription = sb.ToString();
				}
			}, new RuleMeta() { Scope = EntityStatusScopes.Add | EntityStatusScopes.Update });
		}
	}
}