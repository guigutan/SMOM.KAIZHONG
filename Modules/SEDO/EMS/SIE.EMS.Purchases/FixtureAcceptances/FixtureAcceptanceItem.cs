using SIE;
using SIE.Common;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Purchases.FixtureAcceptances
{
	/// <summary>
	/// 工治验收项目
	/// </summary>
	[ChildEntity, Serializable]
	//[CriteriaQuery]
	[Label("工治验收项目")]
	public partial class FixtureAcceptanceItem : DataEntity
	{
		#region 项目名称 ItemName
		/// <summary>
		/// 项目名称
		/// </summary>
		[Label("项目名称")]
		[Required]
		public static readonly Property<string> ItemNameProperty = P<FixtureAcceptanceItem>.Register(e => e.ItemName);

		/// <summary>
		/// 项目名称
		/// </summary>
		public string ItemName
		{
			get { return GetProperty(ItemNameProperty); }
			set { SetProperty(ItemNameProperty, value); }
		}
		#endregion

		#region 验收值 AcceptanceValue
		/// <summary>
		/// 验收值
		/// </summary>
		[Label("验收值")]
		[Required]
		public static readonly Property<string> AcceptanceValueProperty = P<FixtureAcceptanceItem>.Register(e => e.AcceptanceValue);

		/// <summary>
		/// 验收值
		/// </summary>
		public string AcceptanceValue
		{
			get { return GetProperty(AcceptanceValueProperty); }
			set { SetProperty(AcceptanceValueProperty, value); }
		}
		#endregion

		#region 备注 Remark
		/// <summary>
		/// 备注
		/// </summary>
		[MaxLength(1000)]
		[Label("备注")]
		public static readonly Property<string> RemarkProperty = P<FixtureAcceptanceItem>.Register(e => e.Remark);

		/// <summary>
		/// 备注
		/// </summary>
		public string Remark
		{
			get { return GetProperty(RemarkProperty); }
			set { SetProperty(RemarkProperty, value); }
		}
		#endregion

		#region 验收结果 InspectionResult
		/// <summary>
		/// 验收结果
		/// </summary>
		[Label("检验结果")]
		[Required]
		public static readonly Property<InspectionResult> InspectionResultProperty = P<FixtureAcceptanceItem>.Register(e => e.InspectionResult);

		/// <summary>
		/// 验收结果
		/// </summary>
		public InspectionResult InspectionResult
		{
			get { return GetProperty(InspectionResultProperty); }
			set { SetProperty(InspectionResultProperty, value); }
		}
		#endregion

		#region 验收项目 FixtureAcceptance
		/// <summary>
		/// 验收项目Id
		/// </summary>
		public static readonly IRefIdProperty FixtureAcceptanceIdProperty = P<FixtureAcceptanceItem>.RegisterRefId(e => e.FixtureAcceptanceId, ReferenceType.Parent);

		/// <summary>
		/// 验收项目Id
		/// </summary>
		public double FixtureAcceptanceId
		{
			get { return (double)GetRefId(FixtureAcceptanceIdProperty); }
			set { SetRefId(FixtureAcceptanceIdProperty, value); }
		}

		/// <summary>
		/// 验收项目
		/// </summary>
		public static readonly RefEntityProperty<FixtureAcceptance> FixtureAcceptanceProperty = P<FixtureAcceptanceItem>.RegisterRef(e => e.FixtureAcceptance, FixtureAcceptanceIdProperty);

		/// <summary>
		/// 验收项目
		/// </summary>
		public FixtureAcceptance FixtureAcceptance
		{
			get { return GetRefEntity(FixtureAcceptanceProperty); }
			set { SetRefEntity(FixtureAcceptanceProperty, value); }
		}
		#endregion
	}

	/// <summary>
	/// 工治验收项目 实体配置
	/// </summary>
	internal class FixtureAcceptanceItemConfig : EntityConfig<FixtureAcceptanceItem>
	{
		/// <summary>
      	  	/// 配置元数据
    	    	/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("EMS_FIXT_ACPT_ITEM").MapAllProperties();
			Meta.Property(FixtureAcceptanceItem.RemarkProperty).ColumnMeta.HasLength(2000);
			Meta.EnablePhantoms();
		}
	}
}