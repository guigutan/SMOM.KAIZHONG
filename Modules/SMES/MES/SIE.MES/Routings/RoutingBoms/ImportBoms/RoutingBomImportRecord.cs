using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.MES.Routings.RoutingBoms.ImportBoms
{
	/// <summary>
	/// 工序Bom导入记录
	/// </summary>
	[RootEntity, Serializable]
	[Label("工序Bom导入记录")]
	public class RoutingBomImportRecord : DataEntity
	{

		#region 工序BOM主表 RoutingBom
		/// <summary>
		/// 工艺路线版本Id
		/// </summary>
		[Label("工序BOM主表ID")]
		public static readonly IRefIdProperty RoutingBomIdProperty =
			P<RoutingBomImportRecord>.RegisterRefId(e => e.RoutingBomId, ReferenceType.Parent);

		/// <summary>
		/// 工序BOM主表Id
		/// </summary>
		public double RoutingBomId
		{
			get { return (double)GetRefId(RoutingBomIdProperty); }
			set { SetRefId(RoutingBomIdProperty, value); }
		}

		/// <summary>
		/// 工序BOM主表
		/// </summary>
		public static readonly RefEntityProperty<RoutingBom> RoutingBomProperty =
			P<RoutingBomImportRecord>.RegisterRef(e => e.RoutingBom, RoutingBomIdProperty);

		/// <summary>
		/// 工序BOM主表
		/// </summary>
		public RoutingBom RoutingBom
		{
			get { return GetRefEntity(RoutingBomProperty); }
			set { SetRefEntity(RoutingBomProperty, value); }
		}
		#endregion

		#region 操作时间 ImportDate
		/// <summary>
		/// 操作时间
		/// </summary>
		[Label("操作时间")]
		public static readonly Property<DateTime> ImportDateProperty = P<RoutingBomImportRecord>.Register(e => e.ImportDate);

		/// <summary>
		/// 操作时间
		/// </summary>
		public DateTime ImportDate
		{
			get { return GetProperty(ImportDateProperty); }
			set { SetProperty(ImportDateProperty, value); }
		}
		#endregion

		#region 操作人 Operator
		/// <summary>
		/// 操作人Id
		/// </summary>
		[Label("操作人")]
		public static readonly IRefIdProperty OperatorIdProperty = P<RoutingBomImportRecord>.RegisterRefId(e => e.OperatorId, ReferenceType.Normal);

		/// <summary>
		/// 操作人Id
		/// </summary>
		public double OperatorId
		{
			get { return (double)GetRefId(OperatorIdProperty); }
			set { SetRefId(OperatorIdProperty, value); }
		}

		/// <summary>
		/// 操作人
		/// </summary>
		public static readonly RefEntityProperty<Employee> OperatorProperty = P<RoutingBomImportRecord>.RegisterRef(e => e.Operator, OperatorIdProperty);

		/// <summary>
		/// 操作人
		/// </summary>
		public Employee Operator
		{
			get { return GetRefEntity(OperatorProperty); }
			set { SetRefEntity(OperatorProperty, value); }
		}
		#endregion

		#region 日志附件 Attachment
		/// <summary>
		/// 日志附件Id
		/// </summary>
		[Label("日志附件")]
		public static readonly IRefIdProperty AttachmentIdProperty =
			P<RoutingBomImportRecord>.RegisterRefId(e => e.AttachmentId, ReferenceType.Normal);

		/// <summary>
		/// 日志附件Id
		/// </summary>
		public double? AttachmentId
		{
			get { return (double?)this.GetRefNullableId(AttachmentIdProperty); }
			set { this.SetRefNullableId(AttachmentIdProperty, value); }
		}

		/// <summary>
		/// 日志附件
		/// </summary>
		public static readonly RefEntityProperty<RoutingBomAttachment> AttachmentProperty =
			P<RoutingBomImportRecord>.RegisterRef(e => e.Attachment, AttachmentIdProperty);

		/// <summary>
		/// 日志附件
		/// </summary>
		public RoutingBomAttachment Attachment
		{
			get { return this.GetRefEntity(AttachmentProperty); }
			set { this.SetRefEntity(AttachmentProperty, value); }
		}
		#endregion

		#region 视图属性(关联实体属性平铺显示，一般用于Web)

		#region 工艺路线 RoutingName
		/// <summary>
		/// 工艺路线
		/// </summary>
		[Label("工艺路线")]
		public static readonly Property<string> RoutingNameProperty =
			P<RoutingBomImportRecord>.RegisterView(e => e.RoutingName, p => p.RoutingBom.Routing.Name);

		/// <summary>
		/// 工艺路线
		/// </summary>
		public string RoutingName
		{
			get { return this.GetProperty(RoutingNameProperty); }
		}
		#endregion

		#region 版本 VersionName
		/// <summary>
		/// 版本
		/// </summary>
		[Label("版本")]
		public static readonly Property<string> VersionNameProperty = P<RoutingBomImportRecord>.RegisterView(e => e.VersionName, p => p.RoutingBom.RoutingVersion.Name);

		/// <summary>
		/// 版本
		/// </summary>
		public string VersionName
		{
			get { return this.GetProperty(VersionNameProperty); }
		}
		#endregion

		#region 产品编码 ProductCode
		/// <summary>
		/// 产品编码
		/// </summary>
		[Label("产品编码")]
		public static readonly Property<string> ProductCodeProperty =
			P<RoutingBomImportRecord>.RegisterView(e => e.ProductCode, p => p.RoutingBom.Product.Code);

		/// <summary>
		/// 产品编码
		/// </summary>
		public string ProductCode
		{
			get { return this.GetProperty(ProductCodeProperty); }
		}
		#endregion

		#region 操作人 OperatorName

		/// <summary>
		/// 操作人
		/// </summary>
		[Label("操作人")]
		public static readonly Property<string> OperatorNameProperty = P<RoutingBomImportRecord>.RegisterView(e => e.OperatorName, p => p.Operator.Name);

		/// <summary>
		/// 操作人
		/// </summary>
		public string OperatorName
		{
			get { return GetProperty(OperatorNameProperty); }
			set { SetProperty(OperatorNameProperty, value); }
		}
		#endregion

		#endregion
	}

	/// <summary>
	/// 工序Bom 实体配置
	/// </summary>
	internal class RoutingBomImportRecordConfig : EntityConfig<RoutingBomImportRecord>
	{
		/// <summary>
		/// 配置元数据
		/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("TECT_ROUTING_BOM_IMP_REC").MapAllProperties();
			Meta.Property(RoutingBomImportRecord.RoutingBomIdProperty).ColumnMeta.HasIndex();
			Meta.EnablePhantoms();
		}
	}
}
